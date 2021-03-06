using System;
using CUIEngine_Framework.Render;

namespace CUIEngine_Framework.Widgets
{
    public class TestWidget : Widget
    {
        ConsoleColor backColor = ConsoleColor.Black;
        public ConsoleColor BackColor
        {
            get => backColor;
            set
            {
                backColor = value;
                UpdateRenderClip();
            }
        }

        protected override void MakeRenderClip()
        {
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    CurrentClip?.SetUnit(i, j, new RenderUnit(new Color(ConsoleColor.Blue, BackColor)));
                }
            }
        }
    }
}