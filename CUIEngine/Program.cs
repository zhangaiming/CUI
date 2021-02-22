using System;
using CUIEngine.Forms;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;
using DevToolSet;

namespace CUIEngine
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CUIEngine.Initialize();

            TestForm form = Widget.CreateWidget<TestForm>(new Vector2Int(30, 30), new Vector2Int(3, 3), "form",
                RootCanvas.Instance);

            Console.ReadKey();
            CUIEngine.Shutdown();
        }
    }
}