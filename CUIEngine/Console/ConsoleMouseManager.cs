using System;
using System.Runtime.InteropServices;

namespace CUIEngine
{
    public class ConsoleMouseManager
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleMode(IntPtr _hConsoleHandle, out uint _mode);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleMode(IntPtr _hConsoleHandle, uint _mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int _hConsoleHandle);
        
        const int STD_INPUT_HANDLE = -10;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;

        public static void SetConsoleQuickEditMode(bool s)
        {
            IntPtr stdin = GetStdHandle(STD_INPUT_HANDLE);
            uint mode;
            GetConsoleMode(stdin, out mode);
            mode &= s ? ENABLE_QUICK_EDIT_MODE : ~ENABLE_QUICK_EDIT_MODE;
            SetConsoleMode(stdin, mode);
        }
    }
}