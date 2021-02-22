using CUIEngine.Mathf;

namespace CUIEngine.Widgets
{
    public static class WidgetManager
    {
        static WidgetContainer? rootWidget;

        /// <summary>
        /// 根控件
        /// </summary>
        public static WidgetContainer RootWidget => rootWidget!;
        public static void Initialize()
        {
            CreateRootWidget();
        }

        public static void Shutdown()
        {
            
        }

        static void CreateRootWidget()
        {
            //rootWidget = Widget.CreateWidget<WidgetContainer>(Settings.ScreenSize, Vector2Int.Zero, "RootWidget", null);
            //CUIEngine.SetRootCanvas(rootWidget);
        }
    }
}