using System;
using CUIEngine.Mathf;
using CUIEngine.Widgets;
using DevToolSet;

namespace CUIEngine
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CUIEngine.Initialize();

            TestWidget w1 =
                Widget.CreateWidget<TestWidget>(new Vector2Int(5, 5), Vector2Int.Zero, "w1", WidgetManager.RootWidget);
            w1.BackColor = ConsoleColor.Blue;
            TestWidget w2=
                Widget.CreateWidget<TestWidget>(new Vector2Int(5, 5), new Vector2Int(2, 2), "w2", WidgetManager.RootWidget);
            w2.BackColor = ConsoleColor.Green;
            Logger.Log(string.Format("index of w1: {0}", WidgetManager.RootWidget.IndexOf(w1)));
            Logger.Log(string.Format("index of w2: {0}", WidgetManager.RootWidget.IndexOf(w2)));
            Console.ReadKey(true);
            WidgetManager.RootWidget.TopUpWidget(w1);
            Logger.Log(string.Format("index of w1: {0}", WidgetManager.RootWidget.IndexOf(w1)));
            Logger.Log(string.Format("index of w2: {0}", WidgetManager.RootWidget.IndexOf(w2)));
            
            Console.ReadKey();
            CUIEngine.Shutdown();
        }
    }
}