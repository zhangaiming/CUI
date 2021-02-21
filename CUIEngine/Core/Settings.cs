using System;
using CUIEngine.Consoles;
using CUIEngine.Mathf;

namespace CUIEngine
{
    public static class Settings
    {
        internal static event Action<Vector2Int>? OnScreenSizeChanged;
        
        /// <summary>
        /// 默认单元颜色
        /// </summary>
        public static ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.Gray;
        public static ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Blue;
        /// <summary>
        /// 渲染帧间隔
        /// </summary>
        public static short RenderTimespan { get; set; } = 10;
        /// <summary>
        /// 是否显示控制台光标
        /// </summary>
        public static bool ShowCursor
        {
            get => showCursor;
            set
            {
                showCursor = value;
                Console.CursorVisible = showCursor;
            }
        }
        /// <summary>
        /// 单元大小
        /// </summary>
        public static short ConsoleFontSize
        {
            get => consoleFontSize;
            set
            {
                consoleFontSize = value;
                FontManager.SetConsoleFontSize(consoleFontSize);
            }
        }
        /// <summary>
        /// 屏幕大小
        /// </summary>
        public static Vector2Int ScreenSize
        {
            get => screenSize;
            set
            {
                screenSize = value;
                OnScreenSizeChanged?.Invoke(screenSize);
            }
        }
        
        
        static Vector2Int screenSize = new Vector2Int(60, 35);
        static bool showCursor = false;
        static short consoleFontSize = 10;
    }
}