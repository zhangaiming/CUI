using System.Collections.Concurrent;
using System.Threading;
using CUIEngine.Mathf;
using DevToolSet;

namespace CUIEngine.Render
{
    public abstract class Screen
    {
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
        
        ConcurrentQueue<RenderInfo> renderQueue = new ConcurrentQueue<RenderInfo>();
        Thread? drawingThread;
        bool shouldDraw = true;
        bool isPaused = false;
        
        /// <summary>
        /// 设置新的屏幕大小
        /// </summary>
        /// <param name="size"></param>
        public abstract void SetScreenSize(Vector2Int size);

        /// <summary>
        /// 在屏幕中绘制一个像素
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="unit"></param>
        protected abstract void DrawUnit(int x, int y, RenderUnit unit);
        
        /// <summary>
        /// 当初始化时调用
        /// </summary>
        protected virtual void OnInitialize(){}
        /// <summary>
        /// 当终止时调用
        /// </summary>
        protected virtual void OnShutdown(){}
        
        /// <summary>
        /// 初始化屏幕
        /// </summary>
        internal void Initialize()
        {
            Logger.Log("正在初始化屏幕...");
            OnInitialize();
            shouldDraw = true;

            //启动绘画线程
            drawingThread = new Thread(() =>
            {
                while (shouldDraw)
                {
                    if (!isPaused && !renderQueue.IsEmpty)
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
            drawingThread.IsBackground = true;
            drawingThread.Start();
            
            Logger.Log("屏幕初始化完毕!");
        }
        /// <summary>
        /// 终止屏幕
        /// </summary>
        internal void Shutdown()
        {
            Logger.Log("正在卸载屏幕...");
            OnShutdown();
            
            //终止绘画进程
            shouldDraw = false;
            drawingThread?.Join();
            Logger.Log("屏幕卸载完毕!");
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
            for(int j = 0; j < sizeY; j++)
            {
                for (int i = 0; i < sizeX; i++)
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
            Draw(x, y, new RenderUnit(Color.DefaultColor));
        }
        /// <summary>
        /// 控制暂停绘制
        /// </summary>
        /// <param name="s"></param>
        public void SetPause(bool s)
        {
            isPaused = s;
        }
    }
}