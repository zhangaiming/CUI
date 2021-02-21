using System;
using System.Threading;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;
using DevToolSet;

namespace CUIEngine
{
    internal class Canvas : ICanvas
    {
        public Vector2Int Size { get; }

        public RenderClip GetRenderClip()
        {
            return CreateFilledRenderClip(new Vector2Int(0, 0), new Vector2Int(20, 15), ConsoleColor.Green);
        }
        
        static RenderClip CreateFilledRenderClip(Vector2Int coord, Vector2Int size, ConsoleColor color, uint weight = 10000)
        {
            RenderClip clip = new RenderClip(size, coord);
            RenderUnit unit = new RenderUnit(new RenderUnitColor(ConsoleColor.Gray, color), ' ', weight);
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    clip.SetUnit(i, j, unit);
                }
            }

            return clip;
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            CUIEngine.Initialize();

            TestWidget w1 =
                Widget.CreateWidget<TestWidget>(new Vector2Int(5, 5), Vector2Int.Zero, WidgetManager.RootWidget);
            w1.BackColor = ConsoleColor.Blue;
            TestWidget w2=
                Widget.CreateWidget<TestWidget>(new Vector2Int(5, 5), new Vector2Int(2, 2), WidgetManager.RootWidget);
            w2.BackColor = ConsoleColor.Green;
            Logger.Log(string.Format("index of w1: {0}", WidgetManager.RootWidget.IndexOf(w1)));
            Logger.Log(string.Format("index of w2: {0}", WidgetManager.RootWidget.IndexOf(w2)));
            Console.ReadKey(true);
            WidgetManager.RootWidget.TopUpWidget(w1);
            Logger.Log(string.Format("index of w1: {0}", WidgetManager.RootWidget.IndexOf(w1)));
            Logger.Log(string.Format("index of w2: {0}", WidgetManager.RootWidget.IndexOf(w2)));
            
            Console.ReadKey();
            CUIEngine.Shutdown();
            //System.Environment.Exit(0);
        }
    }
}