using System;
using System.Collections.Concurrent;
using System.Threading;
using CUIEngine.Mathf;
using DevToolSet;

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
        Screen screen;
        /// <summary>
        /// 屏幕缓冲片段
        /// </summary>
        RenderClip bufferClip;
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
        public short RenderTimespan
        {
            get => Settings.RenderTimespan;
            set => Settings.RenderTimespan = value;
        }

        /// <summary>
        /// 初始化渲染器
        /// </summary>
        internal void Initialize()
        {
            Logger.Log("正在初始化渲染器...");

            //清空屏幕缓冲
            bufferClip = new RenderClip(Settings.ScreenSize, Vector2Int.Zero);

            //添加屏幕大小更新事件
            Settings.OnScreenSizeChanged += OnScreenSizeChanged;
            
            //初始化绘制器
            screen = new ConsoleScreen();
            screen.Initialize();
            
            //设置控制台大小
            screen.SetScreenSize(Settings.ScreenSize);

            //启动渲染线程
            renderThread = new Thread(() =>
            {
                while (shouldRender)
                {
                    if(!isPaused)
                    {
                        RenderOneFrame();
                    }
                    Thread.Sleep(RenderTimespan);
                }
            });
            renderThread.Start();
            
            isInitialized = true;
            
            Logger.Log("渲染器加载完毕!");
        }
        /// <summary>
        /// 终止渲染器
        /// </summary>
        internal void Shutdown()
        {
            Logger.Log("正在卸载渲染器...");
            Settings.OnScreenSizeChanged -= OnScreenSizeChanged;
            screen.Shutdown();
            isInitialized = false;
            Logger.Log("渲染器卸载完毕!");
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
                    bufferClip.MergeWith(clip, MergeCallback, true);
                }
            }
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
        void MergeCallback(int x, int y, RenderUnit unit)
        {
            screen.Draw(x, y, unit);
        }
        void OnScreenSizeChanged(Vector2Int newSize)
        {
            PauseRender();
            bufferClip.Resize(newSize, Vector2Int.Zero);
            screen.SetScreenSize(newSize);
            screen.Draw(bufferClip);
            StartRender();
        }
    }
}