using System.IO;
using System.Text;

namespace HeartOfTheMachine
{
    
    internal static class TensorHelper
    {

        private static StringBuilder _CachedStringBuilder;
        private const string FORMAT = "F3";
        
        internal static string ToString(Tensor t, char separator, int strLength)
        {
            if (ReferenceEquals(_CachedStringBuilder, null)) _CachedStringBuilder = new StringBuilder(1024);
            else _CachedStringBuilder.Clear();

            var newLine = System.Environment.NewLine;

            var val = t.GetValues();
            int len0 = t.Shape.len0;
            int len1 = t.Shape.len1;
            int len2 = t.Shape.len2;
            for (int d0 = 0; d0 < len0; ++d0)
            {
                if (d0 != 0)
                {
                    _CachedStringBuilder.Append("-------");
                    _CachedStringBuilder.Append(newLine);
                }

                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        if(d2 != 0) _CachedStringBuilder.Append(separator);
                        var s = val[d0, d1, d2].ToString(FORMAT);
                        _CachedStringBuilder.Append(s.PadLeft(strLength));
                    }

                    _CachedStringBuilder.Append(newLine);
                }
            }

            return _CachedStringBuilder.ToString();
        }
        
        
        internal static byte[] Serialize(Tensor t)
        {
            if(ReferenceEquals(t, null)) throw new System.ArgumentNullException();
            
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var len0 = t.Shape.len0;
            var len1 = t.Shape.len1;
            var len2 = t.Shape.len2;

            var val = t.GetValues();
            
            bw.Write(len0);
            bw.Write(len1);
            bw.Write(len2);

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        bw.Write(val[d0, d1, d2]);
                    }
                }
            }

            var bytes = ms.ToArray();
            bw.Close();
            return bytes;
        }

        internal static Tensor Deserialize(byte[] bytes)
        {
            var ms = new MemoryStream(bytes);
            var br = new BinaryReader(ms);

            var len0 = br.ReadInt32();
            var len1 = br.ReadInt32();
            var len2 = br.ReadInt32();
            
            Tensor t = new Tensor(len0, len1, len2);
            var val = t.GetValues();
            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        val[d0, d1, d2] = br.ReadSingle();
                    }
                }
            }

            br.Close();
            return t;
        }
        
    }
    
}