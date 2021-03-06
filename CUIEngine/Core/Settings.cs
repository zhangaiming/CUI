using System;
using CUIEngine.Consoles;
using CUIEngine.Mathf;
using CUIEngine.Render;

namespace CUIEngine
{
    public static class Settings
    {
        internal static event Action<Vector2Int>? OnScreenSizeChanged;
        
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
                ConsoleFontManager.ConsoleFontManager.SetConsoleFont(consoleFontSize, consoleFontSize, "Terminal");
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

        /// <summary>
        /// 被激活的控件的前景色
        /// </summary>
        public static CUIColor ActiveForegroundCUIColor
        {
            get => selectedForegroundCUIColor;
            set => selectedForegroundCUIColor = value;
        }

        /// <summary>
        /// 被激活的控件的背景色
        /// </summary>
        public static CUIColor ActiveBackgroundCUIColor
        {
            get => selectedBackgroundCUIColor;
            set => selectedBackgroundCUIColor = value;
        }

        /// <summary>
        /// 光标闪烁间隔
        /// </summary>
        public static int CursorBlinkingInterval => cursorBlinkingInterval;


        static Vector2Int screenSize = new Vector2Int(80, 50);
        static bool showCursor = false;
        static short consoleFontSize = 10;
        static CUIColor selectedForegroundCUIColor = CUIColor.Black;
        static CUIColor selectedBackgroundCUIColor = CUIColor.Gray;
        static int cursorBlinkingInterval = 400;
    }
}