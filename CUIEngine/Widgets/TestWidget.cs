using System;
using System.Runtime.InteropServices;
using DevToolSet;

namespace CUIEngine.Widgets
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
                ShouldUpdate = true;
            }
        }

        public override void UpdateRenderClip()
        {
            Logger.Log("更新了测试用控件的渲染片段");
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    CurrentClip.SetUnit(i, j, new RenderUnit(new RenderUnitColor(ConsoleColor.Blue, BackColor)));
                }
            }
        }
    }
}