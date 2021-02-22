using System;

namespace CUIEngine.Render
{
    /// <summary>
    /// 包括控制台前后景的颜色,不要将此结构体用作集合的索引
    /// </summary>
    public struct Color
    {
        ConsoleColor foreColor, backColor;
        
        public static readonly Color DefaultColor =
            new Color(ConsoleColor.DarkGray, ConsoleColor.Black);

        /// <summary>
        /// 前景颜色
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get => foreColor;
            set => foreColor = value;
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get => backColor;
            set => backColor = value;
        }

        public Color(ConsoleColor fore, ConsoleColor back)
        {
            foreColor = fore;
            backColor = back;
        }

        public bool Equals(Color other)
        {
            return foreColor == other.foreColor && backColor == other.backColor;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) foreColor, (int) backColor);
        }
    }
}