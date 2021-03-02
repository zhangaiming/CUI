using System;
using System.Threading;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.WidgetLib;
using CUIEngine.Widgets;

namespace CUIEngine
{
    internal class Program
    {
        static bool isRunning = true;
        public static void Main(string[] args)
        {
            CUIEngine.Initialize();
            Input.AttachHandler(Shutdown, ConsoleKey.Escape);

            Panel panel = Widget.CreateWidget<Panel>(new Vector2Int(12, 3), new Vector2Int(2, 2), "panel",
                RootCanvas.Instance);
            panel.DrawType = PanelDrawType.FillAndBorder;
            panel.FillColorPair = new ColorPair(CUIColor.Blue, CUIColor.Cyan);
            
            Label label = Widget.CreateWidget<Label>(new Vector2Int(10, 1), new Vector2Int(3, 3), "label",
                RootCanvas.Instance);
            label.Text = "What is that??";
            label.TextColorPair = new ColorPair(CUIColor.Black, CUIColor.NextForegroundColor);

            while (isRunning)
            {
                Thread.Sleep(1);
            }
            CUIEngine.Shutdown();
        }

        static void Shutdown(ConsoleKeyInfo info)
        {
            isRunning = false;
        }
    }
}