using System;

namespace CUIEngine
{
    public struct RenderUnitColor
    {
        ConsoleColor foreColor, backColor;
        
        public static readonly RenderUnitColor DefaultColor =
            new RenderUnitColor(Settings.DefaultForegroundColor, Settings.DefaultBackgroundColor);

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

        public RenderUnitColor(ConsoleColor fore, ConsoleColor back)
        {
            foreColor = fore;
            backColor = back;
        }

        public override bool Equals(object o)
        {
            if (o is RenderUnitColor)
            {
                RenderUnitColor c = (RenderUnitColor)o;
                return foreColor == c.foreColor && backColor == c.backColor;
            }
            return false;
        }
    }
}