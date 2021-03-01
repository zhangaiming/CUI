using System;
using System.Threading;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.WidgetLib;

namespace CUIEngine.Widgets
{
    public static class Cursor
    {
        static Label cursor = null!;
        static Thread? cursorControllingThread;

        static bool isRunning = true;
        static bool shouldShow = false;

        /// <summary>
        /// 获取光标的当前坐标
        /// </summary>
        public static Vector2Int Coord => cursor.Coord;

        public static void Initialize()
        {
            //初始化光标
            cursor = Widget.CreateWidget<Label>(new Vector2Int(1, 1), Vector2Int.Zero, "Cursor", "Cursor",
                RootCanvas.Instance);
            cursor.Content = " ";
            cursor.TextColor = new Color(ConsoleColor.DarkGray, ConsoleColor.DarkGray);
            //默认不可见
            cursor.IsVisible = false;

            cursorControllingThread = new Thread(() =>
            {
                while (isRunning)
                {
                    if (shouldShow)
                    {
                        cursor.IsVisible = true;
                        RootCanvas.Instance.TopUpWidget(cursor);
                        Thread.Sleep(Settings.CursorBlinkingInterval);
                        if(!isRunning)
                            continue;
                        cursor.IsVisible = false;
                        Thread.Sleep(Settings.CursorBlinkingInterval);
                    }
                }
            });
            
            cursorControllingThread.Start();
        }

        public static void Shutdown()
        {
            shouldShow = false;
            isRunning = false;
        }
        
        public static void SetCursor(Vector2Int coord)
        {
            if(coord.X >= 0 && coord.Y >= 0 && coord.X < Settings.ScreenSize.X && coord.Y < Settings.ScreenSize.Y)
            {
                cursor.Coord = coord;
            }
            else
            {
                throw new ArgumentOutOfRangeException("光标的坐标应在屏幕范围内", (Exception?)null);
            }
        }

        public static void Show()
        {
            shouldShow = true;
        }

        public static void Hide()
        {
            shouldShow = false;
        }
    }
}