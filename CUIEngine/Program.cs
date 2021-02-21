using System;
using System.Threading;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;

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

            TestWidget w =
                Widget.CreateWidget<TestWidget>(new Vector2Int(5, 5), Vector2Int.Zero, WidgetManager.RootWidget);
            w.BackColor = ConsoleColor.Blue;

            Console.ReadKey();
        }
    }
}