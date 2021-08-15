using BasicWidgetLib.Widgets;
using CUIEngine.Scripts;
using CUIEngine.Widgets;

namespace DirectoryBrowser.Scripts
{
    public class ExplorerControl : Script
    {
        ItemContainerControl itemContainer;

        /// <summary>
        /// 项目容器
        /// </summary>
        public ItemContainerControl ItemContainer
        {
            get => itemContainer;
            set
            {
                itemContainer = value; 
                itemContainer.SetDirectory("");
            }
        }

        /// <summary>
        /// 当前目录
        /// </summary>
        public string CurrentPath { get; set; }
    }
}