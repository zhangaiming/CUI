using System;
using System.Runtime.InteropServices;

namespace CUIEngine.Consoles
{
    public class FontManager
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool GetCurrentConsoleFontEx(
            IntPtr consoleOutput,
            bool maximumWindow,
            ref ConsoleFontInfoEx lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(
            IntPtr consoleOutput,
            bool maximumWindow,
            ref ConsoleFontInfoEx consoleCurrentFontEx);
        
        [StructLayout(LayoutKind.Sequential)]
        internal struct Coord
        {
            internal short X;
            internal short Y;

            internal Coord(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct ConsoleFontInfoEx
        {
            internal uint cbSize;
            internal uint nFont;
            internal Coord dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            internal fixed char FaceName[32];
        }
        
        const int StdOutputHandle = -11;
        static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public static void SetConsoleFontSize(short n)
        {
            IntPtr hnd = GetStdHandle(StdOutputHandle);
            if (hnd != INVALID_HANDLE_VALUE)
            {
                ConsoleFontInfoEx info = new ConsoleFontInfoEx();
                info.cbSize = (uint) Marshal.SizeOf(info);
                // First determine whether there's already a TrueType font.
                if (GetCurrentConsoleFontEx(hnd, false, ref info))
                {
                    info.dwFontSize.X = n;
                    info.dwFontSize.Y = n;
                    SetCurrentConsoleFontEx(hnd, false, ref info);
                }
            }
        }
    }
}