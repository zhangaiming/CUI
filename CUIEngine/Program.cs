using System;
using System.Threading;
using CUIEngine.Forms;
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

            Label label = Widget.CreateWidget<Label>(new Vector2Int(10, 1), new Vector2Int(3, 3), "label",
                RootCanvas.Instance);
            label.Text = "What is that??";

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