using System;
using System.Collections.Concurrent;
using System.Threading;
using CUIEngine.Consoles;
using CUIEngine.Mathf;

namespace CUIEngine.Render
{
    public class ConsoleScreen : Screen
    {
        ConcurrentQueue<RenderString> renderQueue = new ConcurrentQueue<RenderString>();
        protected override void OnInitialize()
        {
            Console.CancelKeyPress += (obj, e) => e.Cancel = true;
            FontManager.SetConsoleFontSize(Settings.ConsoleFontSize);
            Console.CursorVisible = Settings.ShowCursor;
            ConsoleMouseManager.SetConsoleQuickEditMode(false);
        }

        public override void DrawCall(int x, int y, RenderUnit unit)
        {
            if (!unit.IsEmpty)
            {
                renderQueue.Enqueue(new RenderString(unit, new Vector2Int(x, y)));
            }
        }

        public override void DrawCall(RenderString renderString)
        {
            renderQueue.Enqueue(renderString);
        }

        protected override void DrawProcess()
        {
            if (!IsPaused && !renderQueue.IsEmpty)
            {
                RenderString str;
                if (renderQueue.TryDequeue(out str))
                {
                    Draw(str);
                }
            }
        }

        public override void SetScreenSize(Vector2Int size)
        {
            SetPause(true);
            
            Console.ResetColor();
            Console.Clear();
            int x = size.X, y = size.Y;
            Console.SetWindowSize(x, y + 1);
            Console.SetBufferSize(x, y + 1);

            //等待控制台窗口设置
            Thread.Sleep(10);
            
            SetPause(false);
        }
        
        /// <summary>
        /// 打印一个渲染串
        /// </summary>
        /// <param name="str"></param>
        protected void Draw(RenderString str)
        {

            Console.ForegroundColor = ParseColor(str.Color.ForegroundColor);
            Console.BackgroundColor = ParseColor(str.Color.BackgroundColor);
            Console.SetCursorPosition(str.Origin.X, str.Origin.Y);
            Console.Write(str.Content);
        }

        /// <summary>
        /// 将Color转换成对应的ConsoleColor,若无对应的ConsoleColor默认返回ConsoleColor.Black
        /// </summary>
        /// <param name="cuiColor"></param>
        ConsoleColor ParseColor(CUIColor cuiColor)
        {
            switch (cuiColor)
            {
                case CUIColor.Black:
                    return ConsoleColor.Black;
                case CUIColor.DarkBlue:
                    return ConsoleColor.DarkBlue;
                case CUIColor.DarkGreen:
                    return ConsoleColor.DarkGreen;
                case CUIColor.DarkCyan:
                    return ConsoleColor.DarkCyan;
                case CUIColor.DarkRed:
                    return ConsoleColor.DarkRed;
                case CUIColor.DarkMagenta:
                    return ConsoleColor.DarkMagenta;
                case CUIColor.DarkYellow:
                    return ConsoleColor.DarkYellow;
                case CUIColor.Gray:
                    return ConsoleColor.Gray;
                case CUIColor.DarkGray:
                    return ConsoleColor.DarkGray;
                case CUIColor.Blue:
                    return ConsoleColor.Blue;
                case CUIColor.Green:
                    return ConsoleColor.Green;
                case CUIColor.Cyan:
                    return ConsoleColor.Cyan;
                case CUIColor.Red:
                    return ConsoleColor.Red;
                case CUIColor.Magenta:
                    return ConsoleColor.Magenta;
                case CUIColor.Yellow:
                    return ConsoleColor.Yellow;
                case CUIColor.White:
                    return ConsoleColor.White;
                default:
                    return ConsoleColor.Black;
            }
        }
    }
}