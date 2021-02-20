using System;
using System.Threading;
using CUIEngine.Render;
using Loggers;

namespace CUIEngine
{
    public static class CUIEngine
    {
        static Renderer renderer;
        public static Renderer Renderer => renderer;
        
        /// <summary>
        /// 初始化CUI引擎
        /// </summary>
        public static void Initialize()
        {
            //控制台初始化
            ResetConsoleSize();
            
            //日志初始化
            Logger.Initialize(@"C:\Users\legion\Documents\CUI\");
            
            //渲染器初始化
            renderer = new Renderer();
            renderer.Initialize();
            //因为如果立即开始渲染会导致出现奇怪的错误,所以在这里等5毫秒
            Thread.Sleep(5);
            renderer.StartRender();
        }
        
        /// <summary>
        /// 关闭CUI引擎
        /// </summary>
        public static void Shutdown()
        {
            Logger.Shutdown();
            renderer?.Shutdown();
        }

        /// <summary>
        /// 根据Settings重新设置控制台大小
        /// </summary>
        static void ResetConsoleSize()
        {
            if(renderer != null)
                renderer?.PauseRender();
            
            Console.Clear();
            int x = Settings.ScreenSize.X, y = Settings.ScreenSize.Y;
            Console.SetWindowSize(x, y);
            Console.SetBufferSize(x, y);
            
            //等待控制台窗口设置
            if(renderer != null)
            {
                Thread.Sleep(5);
                renderer.StartRender();
            }
        }
    }
}