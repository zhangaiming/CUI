using System.Collections.Concurrent;
using System.Threading;
using CUIEngine.Mathf;

namespace CUIEngine.Render
{
    public delegate void OnRenderBegin();
    public delegate void OnRenderFinished();
    public class Renderer
    {
        /// <summary>
        /// 当新的一帧开始渲染前调用
        /// </summary>
        public static OnRenderBegin OnRenderBeginHandlers;
        /// <summary>
        /// 当一帧渲染完毕后调用
        /// </summary>
        public static OnRenderFinished OnRenderFinishedHandlers;
        
        /// <summary>
        /// 控制台绘画者
        /// </summary>
        Drawer drawer;
        /// <summary>
        /// 帧渲染时间间隔
        /// </summary>
        int renderTimespan = 10;
        /// <summary>
        /// 屏幕缓冲片段
        /// </summary>
        RenderClip screenBufferClip;
        /// <summary>
        /// 是否已经过初始化
        /// </summary>
        bool isInitialized = false;
        /// <summary>
        /// 渲染线程开关
        /// </summary>
        bool shouldRender = true;
        /// <summary>
        /// 画布
        /// </summary>
        ICanvas canvas;
        /// <summary>
        /// 渲染线程
        /// </summary>
        Thread renderThread;

        /// <summary>
        /// 是否暂停渲染
        /// </summary>
        bool isPaused = true;

        /// <summary>
        /// 画布
        /// </summary>
        public ICanvas Canvas
        {
            get => canvas;
            set => canvas = value;
        }
        /// <summary>
        /// 帧间隔(毫秒)
        /// </summary>
        public int RenderTimespan
        {
            get => renderTimespan;
            set => renderTimespan = value;
        }

        /// <summary>
        /// 初始化渲染器
        /// </summary>
        internal void Initialize()
        {
            drawer = new Drawer();
            drawer.Initialize();
            
            //清空屏幕缓冲
            screenBufferClip = new RenderClip(Settings.ScreenSize, Vector2Int.Zero);
            
            //启动渲染线程
            renderThread = new Thread(() =>
            {
                while (shouldRender)
                {
                    if(!isPaused)
                    {
                        RenderOneFrame();
                    }
                    Thread.Sleep(renderTimespan);
                }
            });
            renderThread.Start();
            
            isInitialized = true;
        }
        /// <summary>
        /// 终止渲染器
        /// </summary>
        internal void Shutdown()
        {
            drawer.Shutdown();
            isInitialized = false;
        }
        /// <summary>
        /// 渲染新的一帧
        /// </summary>
        /// <param name="frameClip"></param>
        internal void RenderOneFrame()
        {
            if(isInitialized)
            {
                RenderClip clip = canvas?.GetRenderClip();
                if(clip != null)
                {
                    screenBufferClip.MergeWith(clip, MergeCallback, true);
                }
            }
        }

        void MergeCallback(int x, int y, RenderUnit unit)
        {
            drawer.Draw(x, y, unit);
        }
            
        /// <summary>
        /// 暂停渲染
        /// </summary>
        internal void PauseRender()
        {
            isPaused = true;
        }
        /// <summary>
        /// 继续渲染
        /// </summary>
        internal void StartRender()
        {
            isPaused = false;
        }
    }
}