using System;
using System.IO;
using BasicWidgetLib.Widgets;
using CUIEngine;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;
using DirectoryBrowser.Scripts;

namespace DirectoryBrowser
{
    public class Program
    {
        public static void Main()
        {
            CUIEngine.CUIEngine.Initialize();
            Input.AttachHandler(Shutdown, ConsoleKey.Escape);
            Initialize();
        }

        static void Shutdown(ConsoleKeyInfo keyInfo)
        {
            CUIEngine.CUIEngine.Shutdown();
        }

        public static void Initialize()
        {
            Settings.ScreenSize = new Vector2Int(60, 25);
            
            //初始化控件
            
            //当前目录
            TextBox currentPathLabel =
                Widget.Create<TextBox>(new Vector2Int(0, 1), new Vector2Int(2, 1), "current_path_label");
            currentPathLabel.Text = "Current Directory:";
            currentPathLabel.AutoSize = AutoSizeType.AutoWidth;

            Panel currentPathBg = Widget.Create<Panel>(new Vector2Int(54, 1), new Vector2Int(2, 2), "current_path_bg");
            currentPathBg.DrawType = PanelDrawType.FillOnly;
            currentPathBg.FillColor = new ColorPair(CUIColor.White, CUIColor.DarkGreen);
            
            TextBox currentPath = Widget.Create<TextBox>(new Vector2Int(54, 1), new Vector2Int(2, 2), "current_path");
            currentPath.Text = "N/A";
            currentPath.TextColor = CUIColor.NextForegroundColor;

            //浏览区域
            //浏览区域容器
            WidgetContainer explorerContainer =
                Widget.Create<WidgetContainer>(new Vector2Int(54, 21), new Vector2Int(2, 3), "exp_container");
            //背景板
            Panel background = Widget.Create<Panel>(new Vector2Int(54, 21), new Vector2Int(0, 0), "exp_bg", explorerContainer);
            background.DrawType = PanelDrawType.FillAndBorder;
            background.FillColor = new ColorPair(CUIColor.Black, CUIColor.Black);
            background.BorderColor = new ColorPair(CUIColor.Gray, CUIColor.DarkGray);
            //目录容器
            WidgetContainer container =
                Widget.Create<WidgetContainer>(new Vector2Int(52, 19), new Vector2Int(1, 1), "item_container", explorerContainer);
            container.AddScript<ItemContainerControl>();
            container.ClipChildren = true;
        }
    }
}