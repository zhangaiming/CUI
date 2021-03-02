using System;

//todo: 给结构体添加了透明通道,但是RenderClip中的MergeWith方法还未进行相应的更新
namespace CUIEngine.Render
{
    /// <summary>
    /// 包括控制台前后景的颜色,不要将此结构体用作集合的索引
    /// </summary>
    public struct Color
    {
        ConsoleColor foreColor, backColor;
        bool isForeTransparent, isBackTransparent;
        
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

        /// <summary>
        /// 背景是否透明(采用下层的背景色)
        /// </summary>
        public bool IsBackgroundTransparent
        {
            get => isBackTransparent;
            set => isBackTransparent = value;
        }

        /// <summary>
        /// 前景是否透明(采用下层的前景色)
        /// </summary>
        public bool IsForegroundTransparent
        {
            get => isForeTransparent;
            set => isForeTransparent = value;
        }

        public Color(ConsoleColor foregroundColor, ConsoleColor backgroundColor, bool foreTransparent = false, bool backTransparent = false)
        {
            foreColor = foregroundColor;
            backColor = backgroundColor;
            isForeTransparent = foreTransparent;
            isBackTransparent = backTransparent;
        }

        public bool Equals(Color other)
        {
            return foreColor == other.foreColor && backColor == other.backColor;
        }
        
        public static bool operator ==(Color a, Color b)
        {
            return a.Equals(b);
        }
        
        public static bool operator !=(Color a, Color b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) foreColor, (int) backColor, isForeTransparent, isBackTransparent);
        }
        
        public override bool Equals(object? obj)
        {
            if (obj is Color other)
            {
                return isBackTransparent == other.isBackTransparent && isForeTransparent == other.isForeTransparent &&
                       foreColor == other.foreColor && backColor == other.backColor;
            }

            return false;
        }
    }
}