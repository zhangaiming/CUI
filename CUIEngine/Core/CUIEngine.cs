using System;
using System.Threading;
using CUIEngine.Inputs;
using CUIEngine.Render;
using CUIEngine.Widgets;
using Log;

namespace CUIEngine
{
    public static class CUIEngine
    {
        /// <summary>
        /// 初始化CUI引擎
        /// </summary>
        public static void Initialize()
        {
            Logger.Log("CUI启动中...");

            //日志初始化
            Logger.Initialize(@"C:\Users\legion\Documents\CUI\");

            //渲染器初始化
            Renderer.Initialize();
            SetRootCanvas(Root.Instance);
            //因为如果立即开始渲染会导致出现奇怪的错误,所以在这里等5毫秒
            Thread.Sleep(5);
            Renderer.StartRender();

            //输入处理初始化
            Input.Initialize();
            Logger.Log("CUI启动成功!");
        }

        /// <summary>
        /// 关闭CUI引擎
        /// </summary>
        public static void Shutdown()
        {
            //关闭日志
            Logger.Shutdown();
            Environment.Exit(Environment.ExitCode);
        }

        internal static void SetRootCanvas(Root canvas)
        {
            Renderer.Canvas = canvas;
        }
    }
}