using System;
using System.IO;
using BasicWidgetLib.Scripts;
using BasicWidgetLib.Widgets;
using CUIEngine.Scripts;
using CUIEngine.Scripts.ScriptAttribute;

namespace DirectoryBrowser.Scripts
{
    [TargetWidget(typeof(TextBox))]
    public class ItemControl : Script
    {
        string path = "";
        bool isDirectory = false;

        /// <summary>
        /// 项目代表的路径
        /// </summary>
        public string Path
        {
            get => path;
            set
            {
                path = value;
                ((TextBox)Owner).Text = new DirectoryInfo(path).Name + "\\";
            }
        }
        
        /// <summary>
        /// 是否目录
        /// </summary>
        public bool IsDirectory
        {
            get => isDirectory;
            set => isDirectory = value;
        }
    }
}