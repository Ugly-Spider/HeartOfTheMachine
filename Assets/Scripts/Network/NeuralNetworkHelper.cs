using System.IO;
using HeartOfTheMachine;

namespace HeartOfTheMachine
{
    public static class NeuralNetworkHelper
    {
        internal static void SaveNetwork(INeuralNetwork neuralNetwork, string filePath, string fileName)
        {
            var buffer = new NetworkBuffer();
            buffer.WriteString(Global.VERSION);
            buffer.WriteInt32(neuralNetwork.Layers.Count);
            for (int i = 0; i < neuralNetwork.Layers.Count; ++i)
            {
                var layer = neuralNetwork.Layers[i];
                buffer.WriteLayer(layer);
            }

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var path = $"{filePath}/{fileName}.{Global.FILE_SUFFIX}";
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            var bytes = buffer.ToBytes();
            buffer.Close();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        internal static NeuralNetwork LoadNetwork(string path)
        {
            if (!path.EndsWith("." + Global.FILE_SUFFIX))
            {
                path += "." + Global.FILE_SUFFIX;
            }
            if (!File.Exists(path)) throw new FileNotFoundException($"Can't find the file:{path}");
            
            NeuralNetwork neuralNetwork = new NeuralNetwork();
            
            FileStream stream = new FileStream(path, FileMode.Open);
            NetworkBuffer buff = new NetworkBuffer(stream);
            neuralNetwork.Version = buff.ReadString();
            int layerCount = buff.ReadInt32();
            for (int i = 0; i < layerCount; ++i)
            {
                var layer = buff.ReadLayer();
                neuralNetwork.AddLayer(layer);
            }

            buff.Close();
            return neuralNetwork;
        }

    }
}