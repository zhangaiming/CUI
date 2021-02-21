using CUIEngine.Mathf;

namespace CUIEngine
{
    public struct RenderUnit
    {
        RenderUnitColor color;
        char content;
        uint weight;
        bool isEmpty;

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
        /// 单位是否为空
        /// </summary>
        public bool IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        const uint DefaultWeight = 900000;
        
        public RenderUnit(char content, uint weight = DefaultWeight) 
            : this(RenderUnitColor.DefaultColor, content, weight){}
        public RenderUnit(RenderUnitColor color, char content = ' ', uint weight = DefaultWeight)
            : this(false, color, content, weight){}
        public RenderUnit(bool isEmpty, char content = ' ', uint weight = DefaultWeight) 
            : this(isEmpty, RenderUnitColor.DefaultColor, content, weight){}
        public RenderUnit(bool isEmpty, RenderUnitColor color, char content = ' ', uint weight = DefaultWeight)
        {
            this.weight = weight;
            this.content = content;
            this.color = color;
            this.isEmpty = isEmpty;
        }
    }
}