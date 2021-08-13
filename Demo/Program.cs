using System;
using System.Threading;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.WidgetLib;
using CUIEngine.Widgets;

namespace CUITest
{
    class Program
    {
        public static void Main(string[] args)
        {
            CUIEngine.CUIEngine.Initialize();
            Input.AttachHandler(Shutdown, ConsoleKey.Escape);

            Panel panel = new Panel(new Vector2Int(12, 3), new Vector2Int(2, 2), RootCanvas.Instance, "panel")
            {
                DrawType = PanelDrawType.FillAndBorder,
                FillColor = new ColorPair(CUIColor.Blue, CUIColor.Cyan),
                BorderColor = new ColorPair(CUIColor.DarkGray, CUIColor.NextBackgroundColor)
            };
        }

        static void Shutdown(ConsoleKeyInfo info)
        {
            CUIEngine.CUIEngine.Shutdown();
        }
    }
}