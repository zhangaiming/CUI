using System;
using Loggers;

namespace CUIEngine
{
    public static class CUIEngine
    {
        public static void Initialize()
        {
            Logger.Initialize(@"C:\Users\legion\Documents\CUI\");
        }

        public static void Shutdown()
        {
            Logger.Shutdown();
        }
    }
}