using System;
using System.Threading;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.WidgetLib;

namespace CUIEngine
{
    internal class Program
    {
        static bool isRunning = true;
        public static void Main(string[] args)
        {
            CUIEngine.Initialize();
            Input.AttachHandler(Shutdown, ConsoleKey.Escape);

            Panel panel = new Panel(new Vector2Int(12, 3), new Vector2Int(2, 2), RootCanvas.Instance, "panel")
            {
                DrawType = PanelDrawType.FillAndBorder,
                FillColor = new ColorPair(CUIColor.Blue, CUIColor.Cyan)
            };

            Label label = new Label(new Vector2Int(10, 1), new Vector2Int(3, 3), RootCanvas.Instance, "label");
            label.Text = "What is that??";
            label.TextColor = new ColorPair(CUIColor.Black, CUIColor.NextForegroundColor);

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