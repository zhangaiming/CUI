using System;
using System.Threading;
using CUIEngine.Consoles;
using CUIEngine.Mathf;

namespace CUIEngine.Render
{
    public class ConsoleScreen : Screen
    {
        protected override void OnInitialize()
        {
            Console.CancelKeyPress += (obj, e) => e.Cancel = true;
            FontManager.SetConsoleFontSize(Settings.ConsoleFontSize);
            Console.CursorVisible = Settings.ShowCursor;
            ConsoleMouseManager.SetConsoleQuickEditMode(false);
        }
        
        public override void SetScreenSize(Vector2Int size)
        {
            SetPause(true);
            
            Console.ResetColor();
            Console.Clear();
            int x = size.X, y = size.Y;
            Console.SetWindowSize(x - 2, y - 1);
            Console.SetBufferSize(x, y);
            
            //等待控制台窗口设置
            Thread.Sleep(10);
            
            SetPause(false);
        }
        
        protected override void DrawUnit(int x, int y, RenderUnit unit)
        {
            if (unit.IsEmpty)
            {
                Console.ResetColor();
                Console.SetCursorPosition(x, y);
                Console.Write(' ');
            }
            else
            {
                Console.ForegroundColor = unit.Color.ForegroundColor;
                Console.BackgroundColor = unit.Color.BackgroundColor;
                Console.SetCursorPosition(x, y);
                Console.Write(unit.Content);
            }
        }
    }
}