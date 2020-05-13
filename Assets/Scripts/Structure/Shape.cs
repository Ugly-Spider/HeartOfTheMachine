namespace HeartOfTheMachine
{
    public struct Shape
    {
        
        public int len0;
        public int len1;
        public int len2;

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return len0;
                    case 1:
                        return len1;
                    case 2:
                        return len2;
                    default:
                        throw new System.IndexOutOfRangeException($"Index out of range, index:{index}");
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        len0 = value;
                        break;
                    case 1:
                        len1 = value;
                        break;
                    case 2:
                        len2 = value;
                        break;
                    default:
                        throw new System.IndexOutOfRangeException($"Index out of range, index:{index}");
                }
            }
        }

        public Shape(int len0, int len1, int len2)
        {
            this.len0 = len0;
            this.len1 = len1;
            this.len2 = len2;
        }
        
        public override string ToString()
        {
            return $"[{len0}, {len1}, {len2}]";
        }
        
        
        public static bool operator ==(Shape a, Shape b)
        {
            return a.len0 == b.len0 && a.len1 == b.len1 && a.len2 == b.len2;
        }
        
        public static bool operator !=(Shape a, Shape b)
        {
            return !(a == b);
        }

        public static Shape GetShape(float[,,] array3d)
        {
            return new Shape(array3d.GetLength(0), array3d.GetLength(1), array3d.GetLength(2));
        }
    }
    
}