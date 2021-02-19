using System;
using CUIEngine.Mathf;

namespace CUIEngine
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            RenderClip clip1 = new RenderClip(10, 10);
            clip1.SetUnit(new Vector2Int(2,5), new RenderUnit('h'));
            clip1.SetUnit(new Vector2Int(2,6), new RenderUnit('e'));
            clip1.SetUnit(new Vector2Int(1,5), new RenderUnit('l'));
            clip1.SetUnit(new Vector2Int(1,4), new RenderUnit('l'));
            clip1.SetUnit(new Vector2Int(3,5), new RenderUnit('o'));
            PaintRenderClip(clip1);

            Console.ReadKey();
        }

        static void PaintRenderClip(RenderClip clip)
        {
            int x = clip.Size.X, y = clip.Size.Y;
            RenderUnit u;
            for (int j = 0; j < y; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    u = clip.GetUnit(i, j);
                    Console.ForegroundColor = u.Color.ForegroundColor;
                    Console.BackgroundColor = u.Color.BackgroundColor;
                    Console.Write(u.Content);
                }
                Console.WriteLine();
            }
        }
    }
}