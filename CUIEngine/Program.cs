using System;
using CUIEngine.Mathf;
using CUIEngine.Render;

namespace CUIEngine
{
    internal class Canvas : ICanvas
    {
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

            Canvas c = new Canvas();
            CUIEngine.Renderer.Canvas = c;

            Console.ReadKey();
        }

        /*static void PaintRenderClip(RenderClip clip)
        {
            int x = clip.Size.X, y = clip.Size.Y;
            RenderUnit u;
            int ox = clip.Coord.X, oy = clip.Coord.Y;
            for (int j = 0; j < y + oy; j++)
            {
                for (int i = 0; i < x + ox; i++)
                {
                    if (i < ox || j < oy)
                    {
                        Console.ResetColor();
                        Console.Write(' ');
                        continue;
                    }
                    u = clip.GetUnit(i - ox, j - oy);
                    if(!u.IsEmpty)
                    {
                        Console.ForegroundColor = u.Color.ForegroundColor;
                        Console.BackgroundColor = u.Color.BackgroundColor;
                        Console.Write(u.Content);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
        }*/
    }
}