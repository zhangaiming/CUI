namespace FontTest
{
   using System;
   using System.Runtime.InteropServices;

   public class Example
   {
      [DllImport("kernel32.dll", SetLastError = true)]
      static extern IntPtr GetStdHandle(int nStdHandle);

      [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
      static extern bool GetCurrentConsoleFontEx(
         IntPtr consoleOutput,
         bool maximumWindow,
         ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx);

      [DllImport("kernel32.dll", SetLastError = true)]
      static extern bool SetCurrentConsoleFontEx(
         IntPtr consoleOutput,
         bool maximumWindow,
         ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

      private const int StdOutputHandle = -11;
      private const int TmpfTruetype = 4;
      private const int LfFacesize = 32;
      private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

      public static unsafe void Main()
      {
         /*string fontName = "Lucida Console";
         IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
         if (hnd != INVALID_HANDLE_VALUE) {
            CONSOLE_FONT_INFO_EX info = new CONSOLE_FONT_INFO_EX();
            info.cbSize = (uint) Marshal.SizeOf(info);
            bool tt = false;
            // First determine whether there's already a TrueType font.
            if (GetCurrentConsoleFontEx(hnd, false, ref info)) {
               Console.WriteLine(info.cbSize);
               Console.WriteLine(info.nFont);
               Console.WriteLine(info.dwFontSize.X + "," + info.dwFontSize.Y);
               Console.WriteLine(info.FontFamily);
               Console.WriteLine(info.FontWeight);
               Console.WriteLine(info.FaceName->ToString());
               Console.ReadKey();
               info.dwFontSize.X = 16;
               info.dwFontSize.Y = 16;
               SetCurrentConsoleFontEx(hnd, false, ref info);
               Console.WriteLine(info.cbSize);
               Console.WriteLine(info.nFont);
               Console.WriteLine(info.dwFontSize.X + "," + info.dwFontSize.Y);
               Console.WriteLine(info.FontFamily);
               Console.WriteLine(info.FontWeight);
               Console.WriteLine(info.FaceName->ToString());*/

         /*tt = (info.FontFamily & TMPF_TRUETYPE) == TMPF_TRUETYPE;
         if (tt) {
            Console.WriteLine("The console already is using a TrueType font.");
            return;
         }
         // Set console font to Lucida Console.
         CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
         newInfo.cbSize = (uint) Marshal.SizeOf(newInfo);
         newInfo.FontFamily = TMPF_TRUETYPE;
         IntPtr ptr = new IntPtr(newInfo.FaceName);
         Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);
         // Get some settings from current font.
         newInfo.dwFontSize = new COORD(info.dwFontSize.X, info.dwFontSize.Y);
         newInfo.FontWeight = info.FontWeight;
         SetCurrentConsoleFontEx(hnd, false, newInfo);*/
         string fontName = "Lucida Console";
         IntPtr hnd = GetStdHandle(StdOutputHandle);
         if (hnd != INVALID_HANDLE_VALUE)
         {
            CONSOLE_FONT_INFO_EX info = new CONSOLE_FONT_INFO_EX();
            info.cbSize = (uint) Marshal.SizeOf(info);
            bool tt = false;
            // First determine whether there's already a TrueType font.
            if (GetCurrentConsoleFontEx(hnd, false, ref info))
            {
               Console.WriteLine(info.cbSize);
               Console.WriteLine(info.nFont);
               Console.WriteLine(info.dwFontSize.X + "," + info.dwFontSize.Y);
               Console.WriteLine(info.FontFamily);
               Console.WriteLine(info.FontWeight);
               Console.WriteLine(info.FaceName->ToString());
               Console.ReadKey();
               // Set console font to Lucida Console.
               CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
               newInfo.cbSize = (uint) Marshal.SizeOf(newInfo);
               newInfo.FontFamily = TmpfTruetype;
               IntPtr ptr = new IntPtr(newInfo.FaceName);
               Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);
               // Get some settings from current font.
               newInfo.dwFontSize = new COORD(8, 8);
               newInfo.FontWeight = info.FontWeight;
               /*info.dwFontSize.X = n;
               info.dwFontSize.Y = n;*/
               SetCurrentConsoleFontEx(hnd, false, ref info);
               Console.WriteLine(newInfo.cbSize);
               Console.WriteLine(newInfo.nFont);
               Console.WriteLine(newInfo.dwFontSize.X + "," + info.dwFontSize.Y);
               Console.WriteLine(newInfo.FontFamily);
               Console.WriteLine(newInfo.FontWeight);
               Console.WriteLine(newInfo.FaceName->ToString());
               Console.ReadKey();
            }
         }
      }

      [StructLayout(LayoutKind.Sequential)]
      internal struct COORD
      {
         internal short X;
         internal short Y;

         internal COORD(short x, short y)
         {
            X = x;
            Y = y;
         }
      }

      [StructLayout(LayoutKind.Sequential)]
      internal unsafe struct CONSOLE_FONT_INFO_EX
      {
         internal uint cbSize;
         internal uint nFont;
         internal COORD dwFontSize;
         internal int FontFamily;
         internal int FontWeight;
         internal fixed char FaceName[LfFacesize];
      }
   }
   }