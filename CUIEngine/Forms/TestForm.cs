using System;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.WidgetLib;
using CUIEngine.Widgets;

namespace CUIEngine.Forms
{
    public class TestForm : Form
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
            TestWidget w1 =
                Widget.CreateWidget<TestWidget>(new Vector2Int(5, 5), Vector2Int.Zero, "w1", RootCanvas.Instance);
            w1.BackColor = ConsoleColor.Blue;
            Widget.CreateWidget<TestWidget>(new Vector2Int(5, 5), new Vector2Int(1, 1), "w2", RootCanvas.Instance);
            TestWidget? w2 = Widget.Find<TestWidget>("w2");
            w2!.BackColor = ConsoleColor.Green;
        }
    }
}