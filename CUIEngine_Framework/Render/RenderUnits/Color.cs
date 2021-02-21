using System;

namespace CUIEngine_Framework.Render
{
    /// <summary>
    /// 包括控制台前后景的颜色,不要将此结构体用作集合的索引
    /// </summary>
    public struct Color
    {
        ConsoleColor foreColor, backColor;
        
        public static readonly Color DefaultColor =
            new Color(Settings.DefaultForegroundColor, Settings.DefaultBackgroundColor);

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

#pragma warning disable 659
        public override bool Equals(object o)
#pragma warning restore 659
        {
            if (o is Color)
            {
                Color c = (Color)o;
                return foreColor == c.foreColor && backColor == c.backColor;
            }
            return false;
        }
    }
}