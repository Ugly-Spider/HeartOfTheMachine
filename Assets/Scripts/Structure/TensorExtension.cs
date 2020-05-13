namespace HeartOfTheMachine
{

    public static class TensorExtension
    {
        public static Tensor AsTensor(this int i)
        {
            return new Tensor(i);
        }
        
        public static Tensor AsTensor(this float f)
        {
            return new Tensor(f);
        }
        
        public static Tensor AsTensor(this int[] array, bool transpose = true)
        {
            return new Tensor(array, transpose);
        }
        
        public static Tensor AsTensor(this float[] array, bool transpose = true)
        {
            return new Tensor(array, transpose);
        }
        
        public static void ForEach(this Tensor source, System.Action<float> func)
        {
            var len0 = source.Shape.len0;
            var len1 = source.Shape.len1;
            var len2 = source.Shape.len2;
            var val = source.GetValues();

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        func(val[d0, d1, d2]);
                    }
                }
            }
        }
        
        public static void ForEach(this Tensor source, System.Action<int, int, int, float> func)
        {
            var len0 = source.Shape.len0;
            var len1 = source.Shape.len1;
            var len2 = source.Shape.len2;
            var val = source.GetValues();

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        func(d0, d1, d2, val[d0, d1, d2]);
                    }
                }
            }
        }

        public static Tensor Clone(this Tensor source, System.Func<float, float> func)
        {
            var result = new Tensor(source.Shape);
            var len0 = result.Shape.len0;
            var len1 = result.Shape.len1;
            var len2 = result.Shape.len2;
            var valA = source.GetValues();
            var valB = result.GetValues();

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        valB[d0, d1, d2] = func(valA[d0, d1, d2]);
                    }
                }
            }

            return result;
        }
        
        public static Tensor Clone(this Tensor source, System.Func<int, int, int, float, float> func)
        {
            var result = new Tensor(source.Shape);
            var len0 = result.Shape.len0;
            var len1 = result.Shape.len1;
            var len2 = result.Shape.len2;
            var val = result.GetValues();

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        val[d0, d1, d2] = func(d0, d1, d2, val[d0, d1, d2]);
                    }
                }
            }

            return result;
        }

    }
    
}