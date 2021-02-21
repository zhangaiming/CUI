using System.Threading;
using CUIEngine.Render;
using CUIEngine.Widgets;
using DevToolSet;

namespace CUIEngine
{
    public static class CUIEngine
    {
        static Renderer? renderer;
        public static Renderer? Renderer => renderer;

        static ICanvas? rootCanvas;

        /// <summary>
        /// 初始化CUI引擎
        /// </summary>
        public static void Initialize()
        {
            Logger.Log("CUI启动中...");

            //日志初始化
            Logger.Initialize(@"C:\Users\legion\Documents\CUI\");

            //渲染器初始化
            renderer = new Renderer();
            renderer.Initialize();
            WidgetManager.Initialize();
            //因为如果立即开始渲染会导致出现奇怪的错误,所以在这里等5毫秒
            Thread.Sleep(5);
            renderer.StartRender();
            Logger.Log("CUI启动成功!");
        }

        /// <summary>
        /// 关闭CUI引擎
        /// </summary>
        public static void Shutdown()
        {
            Logger.Log("正在关闭CUI...");
            renderer?.Shutdown();
            WidgetManager.Shutdown();
            Logger.Log("CUI关闭成功!");
            
            
            Logger.Shutdown();
            
        }

        internal static void SetRootCanvas(ICanvas? canvas)
        {
            if (rootCanvas == null)
            {
                rootCanvas = canvas;
                if (renderer != null)
                {
                    renderer.Canvas = rootCanvas;
                }
            }
        }
    }
}