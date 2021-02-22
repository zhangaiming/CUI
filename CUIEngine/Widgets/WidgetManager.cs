﻿using CUIEngine.Mathf;

namespace CUIEngine.Widgets
{
    public static class WidgetManager
    {
        static RootWidget? rootWidget;

        /// <summary>
        /// 根控件
        /// </summary>
        public static RootWidget RootWidget => rootWidget!;
        public static void Initialize()
        {
            CreateRootWidget();
        }

        public static void Shutdown()
        {
            
        }

        static void CreateRootWidget()
        {
            rootWidget = Widget.CreateWidget<RootWidget>(Settings.ScreenSize, Vector2Int.Zero, "RootWidget", null);
            CUIEngine.SetRootCanvas(rootWidget);
        }
    }
}