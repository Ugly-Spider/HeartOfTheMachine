using System.IO;
using HeartOfTheMachine;
using HeartOfTheMachine;


namespace HeartOfTheMachine
{
    
    public class NetworkBuffer
    {
        private readonly Stream _stream;
        private readonly BinaryReader _reader;
        private readonly BinaryWriter _writer;
        
        public NetworkBuffer()
        {
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);
        }

        public NetworkBuffer(FileStream stream)
        {
            _stream = stream;
            _reader = new BinaryReader(_stream);
        }

        public NetworkBuffer(byte[] bytes)
        {
            _stream = new MemoryStream(bytes);
            _reader = new BinaryReader(_stream);
        }

        public void Close()
        {
            _reader?.Close();
            _writer?.Close();
        }

        public int ReadInt32()
        {
            return _reader.ReadInt32();
        }

        public string ReadString()
        {
            return _reader.ReadString();
        }
        
        public Shape ReadShape()
        {
            var len0 = _reader.ReadInt32();
            var len1 = _reader.ReadInt32();
            var len2 = _reader.ReadInt32();
            
            return new Shape(len0, len1, len2);
        }
        
        public Tensor ReadTensor()
        {
            var len0 = _reader.ReadInt32();
            var len1 = _reader.ReadInt32();
            var len2 = _reader.ReadInt32();
            
            var t = new Tensor(len0, len1, len2);
            var val = t.GetValues();
            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        val[d0, d1, d2] = _reader.ReadSingle();
                    }
                }
            }

            return t;
        }

        public Tensor[] ReadTensors()
        {
            int len = _reader.ReadInt32();
            var tensors = new Tensor[len];
            for (int i = 0; i < tensors.Length; ++i)
            {
                var t = ReadTensor();
                tensors[i] = t;
            }

            return tensors;
        }

        //layerType, bytesLen, bytes
        public LayerBase ReadLayer()
        {
            var layerType = (LayerType)_reader.ReadInt32();
            LayerBase layer = null;
            switch (layerType)
            {
                case LayerType.DenseLayer:
                    layer = new DenseLayer();
                    break;
            }
            
            Utils.Assert(layer != null, "Read layer failed!");

            int bytesLen = _reader.ReadInt32();
            var bytes = _reader.ReadBytes(bytesLen);
            layer.Deserialize(bytes);
            return layer;
        }

        public void WriteInt32(int val)
        {
            _writer.Write(val);
        }

        public void WriteString(string s)
        {
            _writer.Write(s);
        }

        public void WriteShape(Shape shape)
        {
            _writer.Write(shape.len0);
            _writer.Write(shape.len1);
            _writer.Write(shape.len2);
        }

        public void WriteTensor(Tensor t)
        {
            var len0 = t.Shape.len0;
            var len1 = t.Shape.len1;
            var len2 = t.Shape.len2;

            var val = t.GetValues();
            
            _writer.Write(len0);
            _writer.Write(len1);
            _writer.Write(len2);

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        _writer.Write(val[d0, d1, d2]);
                    }
                }
            }
        }

        public void WriteTensors(Tensor[] tensors)
        {
            _writer.Write(tensors.Length);
            foreach (var t in tensors)
            {
                WriteTensor(t);
            }
        }

        public void WriteLayer(LayerBase layer)
        {
            _writer.Write((int)layer.LayerType);
            var bytes = layer.Serialize();
            _writer.Write(bytes.Length);
            _writer.Write(bytes);
        }

        public byte[] ToBytes()
        {
            return ((MemoryStream)_stream).ToArray();
        }
    }
}

