using System;
using System.Threading;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.WidgetLib;
using DevToolSet;

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

            /*LabelEditField l = new LabelEditField(new Vector2Int(10, 1), new Vector2Int(3, 3), RootCanvas.Instance,
                "input")
            {
                Content = "Hello World!"
            };*/

            Label l = new Label(new Vector2Int(10, 1), new Vector2Int(3, 3), RootCanvas.Instance, "label")
            {
                Text = "Hello World!",
                FontColor = CUIColor.Green
            };

            while (isRunning)
            {
                Thread.Sleep(1);
            }
            
            Logger.Log(l.Coord);
            
            CUIEngine.Shutdown();
        }

        static void Shutdown(ConsoleKeyInfo info)
        {
            isRunning = false;
        }
    }
}