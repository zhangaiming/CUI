using System;

namespace CUIEngine.Render
{
    public struct RenderUnit
    {
        ColorPair colorPair;
        char content;
        bool isEmpty;

        /// <summary>
        /// 单位的颜色
        /// </summary>
        public ColorPair ColorPair
        {
            get => colorPair;
            set => colorPair = value;
        }
        /// <summary>
        /// 单位的内容, 即字符
        /// </summary>
        public char Content
        {
            get => content;
            set
            {
                if (value >= 32 && value <= 126)
                {
                    //可显示的英文字符
                    content = value;
                }
                else
                {
                    content = '?';
                }
            }
        }
        /// <summary>
        /// 单位是否为空
        /// </summary>
        public bool IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        public RenderUnit(char content) 
            : this(ColorPair.DefaultColorPair, content){}
        public RenderUnit(ColorPair colorPair, char content = ' ')
            : this(false, colorPair, content){}
        public RenderUnit(bool isEmpty, char content = ' ') 
            : this(isEmpty, ColorPair.DefaultColorPair, content){}
        public RenderUnit(bool isEmpty, ColorPair colorPair, char content = ' ')
        {
            this.content = content;
            this.colorPair = colorPair;
            this.isEmpty = isEmpty;
        }

        public bool Equals(RenderUnit other)
        {
            return colorPair.Equals(other.colorPair) && content == other.content && isEmpty == other.isEmpty;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(colorPair, content, isEmpty);
        }
    }
}