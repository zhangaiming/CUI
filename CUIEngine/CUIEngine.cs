using System;
using System.Threading;
using CUIEngine.Render;
using DevToolSet;

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
            Logger.Log("CUI启动中...");
            //控制台初始化
            InitializeConsole();

            //日志初始化
            Logger.Initialize(@"C:\Users\legion\Documents\CUI\");

            //渲染器初始化
            renderer = new Renderer();
            renderer.Initialize();
            //因为如果立即开始渲染会导致出现奇怪的错误,所以在这里等5毫秒
            Thread.Sleep(5);
            renderer.StartRender();
            Logger.Log("CUI启动成功!");
        }

        static void InitializeConsole()
        {
            Console.CancelKeyPress += (obj, e) => e.Cancel = true;
            FontManager.SetConsoleFontSize(Settings.ConsoleFontSize);
            Console.CursorVisible = Settings.ShowCursor;
            ConsoleMouseManager.SetConsoleQuickEditMode(false);
        }
        
        /// <summary>
        /// 关闭CUI引擎
        /// </summary>
        public static void Shutdown()
        {
            Logger.Log("正在关闭CUI...");
            renderer?.Shutdown();
            Logger.Log("CUI关闭成功!");
            
            
            Logger.Shutdown();
            
        }
    }
}