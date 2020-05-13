using HeartOfTheMachine;

namespace HeartOfTheMachine
{
    //3d numbers, you can treat it as a matrix array
    public class Tensor
    {
        private float[,,] _values;
        private Shape _shape;
        public Shape Shape => _shape;

        public float this[int d0, int d1, int d2]
        {
            get => _values[d0, d1, d2];
            set => _values[d0, d1, d2] = value;
        }

        public Tensor(int len0, int len1, int len2)
        {
            InternalTensor(new Shape(len0, len1, len2));
        }
        
        public Tensor(int len0, int len1, int len2, System.Func<int, int, int, float> func)
        {
            if(ReferenceEquals(func, null)) throw new System.ArgumentNullException();
            
            InternalTensor(new Shape(len0, len1, len2), func);
        }

        public Tensor(Shape shape)
        {
            InternalTensor(shape);
        }
        
        public Tensor(Shape shape, System.Func<int, int, int, float> func)
        {
            if(ReferenceEquals(func, null)) throw new System.ArgumentNullException();
            
            InternalTensor(shape, func);
        }

        //1x1x1
        public Tensor(float value)
        {
            InternalTensor(new Shape(1, 1, 1));
            _values[0, 0, 0] = value;
        }
        
        public Tensor(int value)
        {
            InternalTensor(new Shape(1, 1, 1));
            _values[0, 0, 0] = value;
        }

        //1xMx1
        public Tensor(float[] array1d, bool transpose = true)
        {
            if (ReferenceEquals(array1d, null) || array1d.Length == 0)
            {
                throw new InvalidInputException();
            }

            if (transpose)
            {
                InternalTensor(new Shape(1, array1d.Length, 1));
                int len1 = _shape.len1;
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    _values[0, d1, 0] = array1d[d1];
                }
            }
            else
            {
                InternalTensor(new Shape(1, 1, array1d.Length));
                int len2 = _shape.len2;
                for (int d2 = 0; d2 < len2; ++d2)
                {
                    _values[0, 0, d2] = array1d[d2];
                }
            }
        }
        
        //1xMx1
        public Tensor(int[] array1d, bool transpose)
        {
            if (ReferenceEquals(array1d, null) || array1d.Length == 0)
            {
                throw new InvalidInputException();
            }
            
            if (transpose)
            {
                InternalTensor(new Shape(1, array1d.Length, 1));
                int len1 = _shape.len1;
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    _values[0, d1, 0] = array1d[d1];
                }
            }
            else
            {
                InternalTensor(new Shape(1, 1, array1d.Length));
                int len2 = _shape.len2;
                for (int d2 = 0; d2 < len2; ++d2)
                {
                    _values[0, 0, d2] = array1d[d2];
                }
            }
        }

        //1xMxN
        public Tensor(float[,] array2d)
        {
            if (ReferenceEquals(array2d, null) || array2d.Length == 0)
            {
                throw new InvalidInputException();
            }
            
            InternalTensor(new Shape(1, array2d.GetLength(0), array2d.GetLength(1)));
            int len1 = _shape.len1;
            int len2 = _shape.len2;
            for (int d1 = 0; d1 < len1; ++d1)
            {
                for (int d2 = 0; d2 < len2; ++d2)
                {
                    _values[0, d1, d2] = array2d[d1, d2];
                }
            }
        }

        //AxMxN
        public Tensor(float[,,] array3d)
        {
            if (ReferenceEquals(array3d, null) || array3d.Length == 0)
            {
                throw new InvalidInputException();
            }
            
            InternalTensor(new Shape(array3d.GetLength(0), array3d.GetLength(1), array3d.GetLength(2)));
            int len0 = _shape.len0;
            int len1 = _shape.len1;
            int len2 = _shape.len2;

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        _values[d0, d1, d2] = array3d[d0, d1, d2];
                    }
                }
            }
        }

        public void FillWithRandomValue(float min, float max)
        {
            int len0 = _shape.len0;
            int len1 = _shape.len1;
            int len2 = _shape.len2;

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        _values[d0, d1, d2] = UnityEngine.Random.Range(min, max);
                    }
                }
            }
        }

        public Tensor Transpose()
        {
            var t = new Tensor(_shape.len0, _shape.len2, _shape.len1);
            
            var len0 = t._shape.len0;
            var len1 = t._shape.len1;
            var len2 = t._shape.len2;

            var values = t.GetValues();
            
            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        values[d0, d1, d2] = _values[d0, d2, d1];
                    }
                }
            }

            return t;
        }

        public Tensor Clone()
        {
            var t = new Tensor(_shape);
            var len0 = _shape.len0;
            var len1 = _shape.len1;
            var len2 = _shape.len2;

            var values = t.GetValues();

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        values[d0, d1, d2] = _values[d0, d1, d2];
                    }
                }
            }

            return t;
        }

        public Tensor Variant(float min, float max)
        {
            var result = new Tensor(_shape);
            var vals = result.GetValues();
            this.ForEach((d0, d1, d2, x) => { vals[d0, d1, d2] = x + UnityEngine.Random.Range(min, max); });
            return result;
        }

        private void InternalTensor(Shape shape)
        {
            this._shape = shape;
            _values = new float[shape.len0, shape.len1, shape.len2];
        }
        
        private void InternalTensor(Shape shape, System.Func<int, int, int, float> func)
        {
            this._shape = shape;
            _values = new float[shape.len0, shape.len1, shape.len2];
            int len0 = shape.len0;
            int len1 = shape.len1;
            int len2 = shape.len2;

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        _values[d0, d1, d2] = func(d0, d1, d2);
                    }
                }
            }
        }

        public bool TryAsNumber(out float val)
        {
            if (_shape.len0 != 1 || _shape.len1 != 1 || _shape.len2 != 1)
            {
                val = -1;
                return false;
            }

            val = _values[0, 0, 0];
            return true;
        }

        public bool TryAsArray(out float[] array1d)
        {
            if (_shape.len0 != 1 || _shape.len2 != 1)
            {
                array1d = null;
                return false;
            }

            int len1 = _shape.len1;
            array1d = new float[len1];
            for (int d1 = 0; d1 < len1; ++d1)
            {
                array1d[d1] = _values[0, d1, 0];
            }

            return true;
        }
        
        //get values of this tensor, for efficiency
        public float[,,] GetValues()
        {
            return _values;
        }
        
        public override string ToString()
        {
            return TensorHelper.ToString(this, ',', 10);
        }
        
        public static Tensor operator +(Tensor a, Tensor b)
        {
            Utils.Assert(a._shape == b._shape, $"Invalid shape to addition:{a._shape} {b._shape}");
            
            var c = new Tensor(a._shape);
            
            var valA = a.GetValues();
            var valB = b.GetValues();
            var valC = c.GetValues();

            int len0 = c._shape.len0;
            int len1 = c._shape.len1;
            int len2 = c._shape.len2;

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        valC[d0, d1, d2] = valA[d0, d1, d2] + valB[d0, d1, d2];
                    }
                }
            }

            return c;
        }

        public static Tensor operator -(Tensor a, Tensor b)
        {
            Utils.Assert(a._shape == b._shape, $"Invalid shape to subtraction:{a._shape} {b._shape}");
            
            var c = new Tensor(a._shape);
            
            var valA = a.GetValues();
            var valB = b.GetValues();
            var valC = c.GetValues();

            int len0 = c._shape.len0;
            int len1 = c._shape.len1;
            int len2 = c._shape.len2;

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        valC[d0, d1, d2] = valA[d0, d1, d2] - valB[d0, d1, d2];
                    }
                }
            }

            return c;
        }

        //matrix multiply each other of array
        public static Tensor operator *(Tensor a, Tensor b)
        {
            Utils.Assert(a._shape.len0 == b._shape.len0 && a._shape.len2 == b._shape.len1, $"Invalid shape to multiplication:{a._shape} {b._shape}");
            
            var c = new Tensor(a._shape.len0, a._shape.len1, b._shape.len2);

            var valA = a.GetValues();
            var valB = b.GetValues();
            var valC = c.GetValues();
            
            var len0 = c._shape.len0;
            var len1 = c._shape.len1;
            var len2 = c._shape.len2;
            
            var len = a._shape.len2;

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {

                        float sum = 0;
                        for (int i = 0; i < len; ++i)
                        {
                            sum += valA[d0, d1, i] * valB[d0, i, d2];
                        }

                        valC[d0, d1, d2] = sum;

                    }
                }
            }

            return c;
        }

        public static Tensor operator *(Tensor t, float f)
        {
            return t.Clone(x => x * f);
        }

        public static Tensor ElementsMul(Tensor a, Tensor b)
        {
            Utils.Assert(a._shape == b._shape, $"Invalid shape to {nameof(ElementsMul)}:{a._shape} {b._shape}");
            
            var c = new Tensor(a._shape);
            
            var valA = a.GetValues();
            var valB = b.GetValues();
            var valC = c.GetValues();
            
            var len0 = c._shape.len0;
            var len1 = c._shape.len1;
            var len2 = c._shape.len2;

            for (int d0 = 0; d0 < len0; ++d0)
            {
                for (int d1 = 0; d1 < len1; ++d1)
                {
                    for (int d2 = 0; d2 < len2; ++d2)
                    {
                        valC[d0, d1, d2] = valA[d0, d1, d2] * valB[d0, d1, d2];
                    }
                }
            }
            
            return c;
        }

        public static byte[] Serialize(Tensor t)
        {
            return TensorHelper.Serialize(t);
        }

        public static Tensor Deserialize(byte[] bytes)
        {
            return TensorHelper.Deserialize(bytes);
        }
    }
}