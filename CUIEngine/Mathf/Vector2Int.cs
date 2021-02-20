using System.Text;

namespace CUIEngine.Mathf
{
    public struct Vector2Int
    {
        int x, y;

        /// <summary>
        /// 第一元素
        /// </summary>
        public int X
        {
            get => x;
            set => x = value;
        }

        /// <summary>
        /// 第二元素
        /// </summary>
        public int Y
        {
            get => y;
            set => y = value;
        }

        /// <summary>
        /// 长度为0的矢量
        /// </summary>
        public static Vector2Int Zero = new Vector2Int();
        
        public Vector2Int(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2Int)
            {
                Vector2Int o = (Vector2Int) obj;
                return o.x == x && o.y == y;
            }
            return false;
        }
        
        public static Vector2Int operator- (Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }
        
        public static Vector2Int operator- (Vector2Int a)
        {
            return new Vector2Int(-a.x, -a.y);
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }

        /// <summary>
        /// 点乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2Int Dot(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x * b.x, a.y * b.y);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(").Append(x).Append(",").Append(y).Append(")");
            return sb.ToString();
        }
    }
}