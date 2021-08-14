using System;
using CUIEngine;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.BasicWidgetLib;
using CUIEngine.Widgets;
using CUITest.Scripts;

namespace CUITest
{
    class Program
    {
        public static void Main(string[] args)
        {
            Settings.ScreenSize = new Vector2Int(20, 20);
            CUIEngine.CUIEngine.Initialize();
            Input.AttachHandler(Shutdown, ConsoleKey.Escape);

            Panel panel = Widget.Create<Panel>(new Vector2Int(6, 3), new Vector2Int(2, 2), "panel");
            panel.BorderColor = panel.FillColor = new ColorPair(CUIColor.DarkGray, CUIColor.DarkCyan);
            panel.DrawType = PanelDrawType.FillAndBorder;
            panel.AddScript<PanelScript>();
        }

        static void Shutdown(ConsoleKeyInfo info)
        {
            CUIEngine.CUIEngine.Shutdown();
        }
    }
}