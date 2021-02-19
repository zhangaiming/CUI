using CUIEngine.Mathf;

namespace CUIEngine
{
    public struct RenderUnit
    {
        RenderUnitColor color;
        char content;
        uint weight;
        Vector2Int coord;

        /// <summary>
        /// 单位的颜色
        /// </summary>
        public RenderUnitColor Color
        {
            get => color;
            set => color = value;
        }
        /// <summary>
        /// 单位的内容, 即字符
        /// </summary>
        public char Content
        {
            get => content;
            set => content = value;
        }
        /// <summary>
        /// 单位的权重, 权重越大的单位越在上层
        /// </summary>
        public uint Weight
        {
            get => weight;
            set => weight = value;
        }
        /// <summary>
        /// 单位的坐标, 方便进行并行处理
        /// </summary>
        public Vector2Int Coord
        {
            get => coord;
            set => coord = value;
        }

        
        public RenderUnit(Vector2Int coord, char content, uint weight = 10000) 
            : this(coord, RenderUnitColor.DefaultColor, content, weight){}

        public RenderUnit(Vector2Int coord, RenderUnitColor color, char content = ' ', uint weight = 10000)
        {
            this.weight = weight;
            this.content = content;
            this.color = color;
            this.coord = coord;
        }
    }
}