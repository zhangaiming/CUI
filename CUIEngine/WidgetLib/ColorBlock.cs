using System;
using CUIEngine.Render;
using CUIEngine.Widgets;

namespace CUIEngine.WidgetLib
{
    /// <summary>
    /// 色块
    /// </summary>
    [Obsolete("此控件将不再进行维护,若需实现色块绘制功能,应选择通用性更高的Panel控件.")]
    public class ColorBlock : Widget
    {
        ConsoleColor color = ConsoleColor.Black;
        public ConsoleColor Color
        {
            get => color;
            set
            {
                color = value;
                UpdateRenderClip();
            }
        }

        protected override void MakeRenderClip()
        {
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    CurrentClip?.SetUnit(i, j, new RenderUnit(new Color(ConsoleColor.DarkGray, Color)));
                }
            }
        }
    }
}