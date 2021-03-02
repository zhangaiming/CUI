using System;

namespace CUIEngine.Render
{
    /// <summary>
    /// 包括控制台前后景的颜色,不要将此结构体用作集合的索引
    /// </summary>
    public struct ColorPair
    {
        CUIColor foreColor, backColor;

        public static readonly ColorPair DefaultColorPair =
            new ColorPair(CUIColor.DarkGray, CUIColor.Black);

        /// <summary>
        /// 前景颜色
        /// </summary>
        public CUIColor ForegroundColor
        {
            get => foreColor;
            set => foreColor = value;
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public CUIColor BackgroundColor
        {
            get => backColor;
            set => backColor = value;
        }

        /// <summary>
        /// 创建颜色对
        /// </summary>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public ColorPair(CUIColor foregroundColor, CUIColor backgroundColor)
        {
            foreColor = foregroundColor;
            backColor = backgroundColor;
        }

        /// <summary>
        /// 创建前后景颜色一样的颜色对
        /// </summary>
        /// <param name="color"></param>
        public ColorPair(CUIColor color)
        {
            foreColor = backColor = color;
        }

        public bool Equals(ColorPair other)
        {
            return foreColor == other.foreColor && backColor == other.backColor;
        }
        
        public static bool operator ==(ColorPair a, ColorPair b)
        {
            return a.Equals(b);
        }
        
        public static bool operator !=(ColorPair a, ColorPair b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) foreColor, (int) backColor);
        }
        
        public override bool Equals(object? obj)
        {
            if (obj is ColorPair other)
            {
                return foreColor == other.foreColor && backColor == other.backColor;
            }

            return false;
        }
    }
}