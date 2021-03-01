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
            /*Form form = Form.Create<TestForm>(new Vector2Int(5, 3), "Form");
            Console.ReadKey(true);
            form.Size += new Vector2Int(10, 10);
            Console.ReadKey(true);
            form.IsVisible = false;*/
            InputField i = Widget.CreateWidget<InputField>(new Vector2Int(5, 3), new Vector2Int(5, 3), "input",
                RootCanvas.Instance);
            i.Content = "absllw222";

            Console.ReadKey(true);
            Widget.SetSelection(i);

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