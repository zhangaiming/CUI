using System;
using System.Runtime.InteropServices;

namespace CUIEngine_Framework.Consoles
{
    public class ConsoleMouseManager
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint mode);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int hConsoleHandle);
        
        const int StdInputHandle = -10;
        const uint EnableQuickEditMode = 0x0040;

        public static void SetConsoleQuickEditMode(bool s)
        {
            IntPtr stdin = GetStdHandle(StdInputHandle);
            uint mode;
            GetConsoleMode(stdin, out mode);
            mode &= s ? EnableQuickEditMode : ~EnableQuickEditMode;
            SetConsoleMode(stdin, mode);
        }
    }
}