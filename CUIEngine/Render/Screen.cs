using System.Collections.Concurrent;
using System.Threading;
using CUIEngine.Mathf;
using Log;

namespace CUIEngine.Render
{
    public abstract class Screen
    {
        protected Thread? DrawingThread;
        protected bool ShouldDraw = true;
        protected bool IsPaused = false;

        /// <summary>
        /// 设置新的屏幕大小
        /// </summary>
        /// <param name="size"></param>
        public abstract void SetScreenSize(Vector2Int size);

        /// <summary>
        /// 当初始化时调用
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        /// <summary>
        /// 当终止时调用
        /// </summary>
        protected virtual void OnShutdown()
        {
        }

        /// <summary>
        /// 将一个渲染串加入绘制队列
        /// </summary>
        /// <param name="renderString">目标渲染串</param>
        public virtual void DrawCall(RenderString renderString)
        {
        }

        /// <summary>
        /// 将一个单元加入绘制队列
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="unit"></param>
        public virtual void DrawCall(int x, int y, RenderUnit unit)
        {
        }

        /// <summary>
        /// 绘画处理函数
        /// </summary>
        protected virtual void DrawProcess()
        {
        }

        /// <summary>
        /// 初始化屏幕
        /// </summary>
        internal void Initialize()
        {
            Logger.Log("正在初始化屏幕...");
            OnInitialize();
            ShouldDraw = true;

            //启动绘画线程
            DrawingThread = new Thread(() =>
            {
                while (ShouldDraw)
                {
                    DrawProcess();
                }
            });
            DrawingThread.IsBackground = true;
            DrawingThread.Start();

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
            ShouldDraw = false;
            DrawingThread?.Join();
            Logger.Log("屏幕卸载完毕!");
        }

        /// <summary>
        /// 控制暂停绘制
        /// </summary>
        /// <param name="s"></param>
        public void SetPause(bool s)
        {
            IsPaused = s;
        }
    }
}