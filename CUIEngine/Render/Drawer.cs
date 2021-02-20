using System;
using System.Collections.Concurrent;
using System.Threading;
using CUIEngine.Mathf;

namespace CUIEngine.Render
{
    public class Drawer
    {
        ConcurrentQueue<RenderInfo> renderQueue = new ConcurrentQueue<RenderInfo>();
        Thread drawingThread;
        bool shouldDraw = true;
        
        struct RenderInfo
        {
            internal int x, y;
            internal RenderUnit Unit;

            internal RenderInfo(int x, int y, RenderUnit unit)
            {
                this.x = x;
                this.y = y;
                Unit = unit;
            }
        }

        /// <summary>
        /// 初始化Renderer
        /// </summary>
        internal void Initialize()
        {
            shouldDraw = true;
            
            //启动绘画线程
            drawingThread = new Thread(() =>
            {
                while (shouldDraw)
                {
                    if (!renderQueue.IsEmpty)
                    {
                        RenderInfo info;
                        if (renderQueue.TryDequeue(out info))
                        {
                            DrawUnit(info.x, info.y, info.Unit);
                        }
                    }
                    //Thread.Sleep(1);
                }
            });
            drawingThread.Start();
        }
        /// <summary>
        /// 终止Renderer
        /// </summary>
        internal void Shutdown()
        {
            shouldDraw = false;
            drawingThread.Join();
        }
        
        /// <summary>
        /// 将一个片段加入绘制队列
        /// </summary>
        /// <param name="clip"></param>
        public void Draw(RenderClip clip)
        {
            int sizeX = clip.Size.X, sizeY = clip.Size.Y;
            int cx = clip.Coord.X, cy = clip.Coord.Y;
            RenderUnit unit;
            for(int j = 0; j < clip.Size.Y; j++)
            {
                for (int i = 0; i < clip.Size.X; i++)
                {
                    unit = clip.GetUnit(i, j);
                    if (!unit.IsEmpty)
                    {
                        Draw(i + cx, j + cy, unit);
                    }
                }
            }
        }
        /// <summary>
        /// 将一个单元加入绘制队列
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="unit"></param>
        public void Draw(int x, int y, RenderUnit unit)
        {
            if (!unit.IsEmpty)
            {
                renderQueue.Enqueue(new RenderInfo(x, y, unit));
            }
        }
        /// <summary>
        /// 擦除一个像素
        /// </summary>
        public void Erase(int x, int y)
        {
            Draw(x, y, new RenderUnit(RenderUnitColor.DefaultColor, ' '));
        }
        /// <summary>
        /// 在控制台中绘制一个像素
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="unit"></param>
        void DrawUnit(int x, int y, RenderUnit unit)
        {
            if (!unit.IsEmpty)
            {
                Console.SetCursorPosition(x + 1, y + 1);
                Console.ForegroundColor = unit.Color.ForegroundColor;
                Console.BackgroundColor = unit.Color.BackgroundColor;
                Console.Write(unit.Content);
            }
        }
    }
}