using System;
using System.Collections.Generic;
using System.IO;
using BasicWidgetLib.Scripts;
using BasicWidgetLib.Widgets;
using CUIEngine;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Scripts;
using CUIEngine.Scripts.ScriptAttribute;
using CUIEngine.Widgets;

namespace DirectoryBrowser.Scripts
{
    [TargetWidget(typeof(WidgetContainer))]
    public class ItemContainerControl : Script
    {
        string curDirectory = "";

        List<ItemControl> items = new List<ItemControl>();

        TextBox? pathBox = null;

        int selectedItem = 0;

        void Awake()
        {
            Input.AttachHandler(OnInput);
            
            pathBox = Sprite.Find<TextBox>("current_path");
            SetDirectory("");
        }

        //输入响应函数
        void OnInput(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                SetSelectedItem(selectedItem - 1);
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                SetSelectedItem(selectedItem + 1);
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                SetSelectedItem(selectedItem - Owner.Size.Y / 2);
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                SetSelectedItem(selectedItem + Owner.Size.Y / 2);
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                Select();
            }
        }

        //选择项目
        void SetSelectedItem(int index)
        {
            if (items.Count == 0) return;
            int trueIndex = Math.Min(items.Count - 1, Math.Max(0, index));
            
            if(selectedItem >= 0 && selectedItem < items.Count)
            {
                items[selectedItem].Owner.GetScript<SelectableText>()?.SetSelect(false);
            }
            selectedItem = trueIndex;
            Widget itemOwner = items[selectedItem].Owner;
            itemOwner.GetScript<SelectableText>()?.SetSelect(true);
            
            int itemOffsetX = 0;

            if (itemOwner.Coord.X + 16 >= Owner.Coord.X + Owner.Size.X)
            {
                itemOffsetX = Owner.Coord.X + Owner.Size.X - 18 - itemOwner.Coord.X;
            }
            else if(itemOwner.Coord.X <= Owner.Coord.X)
            {
                itemOffsetX = Owner.Coord.X - itemOwner.Coord.X;
            }

            if (itemOffsetX != 0)
            {
                Vector2Int offset = new Vector2Int(itemOffsetX, 0);
                foreach (ItemControl itemControl in items)
                {
                    itemControl.Owner.Coord += offset;
                }
            }
        }

        /// <summary>
        /// 激活选中的目标项
        /// </summary>
        void Select()
        {
            if (items.Count > selectedItem && selectedItem >= 0)
            {
                if (items[selectedItem].IsDirectory)
                {
                    string targetPath = items[selectedItem].Path;
                    SetDirectory(targetPath);
                }
            }
        }

        /// <summary>
        /// 设置要显示的目录
        /// </summary>
        /// <param name="path">目标路径，若为空则显示为磁盘分区目录</param>
        public void SetDirectory(string path)
        {
            string[]? files = null, directories = null;
            if (path == "")
            {
                //获取磁盘分区
                directories = Directory.GetLogicalDrives();
            }
            else if (path == "..")
            {
                //返回上一级
                if (curDirectory.EndsWith(":\\"))
                {
                    SetDirectory("");
                }
                else
                {
                    SetDirectory(Directory.GetParent(curDirectory).FullName);
                }
                return;
            }
            else if (!Directory.Exists(path))
            {
                throw new Exception("Path not exist.");
            }
            else
            {
                files = Directory.GetFiles(path);
                directories = Directory.GetDirectories(path);
            }

            Clear();
            curDirectory = path;
            
            //设置当前路径文本
            if (pathBox != null)
            {
                pathBox.Text = path == "" ? "This Computer" : path;
            }

            //添加返回上级项
            if (path != "")
            {
                AddItem("..", true);
            }

            //添加目录项
            if (directories != null)
            {
                foreach (string directory in directories)
                {
                    AddItem(directory, true);
                }
            }
            
            //添加文件项
            if (files != null)
            {
                foreach (string file in files)
                {
                    AddItem(file, false);
                }
            }
            SetSelectedItem(0);
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="path">项目的路径</param>
        /// <param name="isDirectory">目标是否目录</param>
        void AddItem(string path, bool isDirectory)
        {
            int row = items.Count % (Owner.Size.Y / 2);
            int col = items.Count / (Owner.Size.Y / 2);
            TextBox item = Widget.Create<TextBox>(new Vector2Int(16, 1),
                new Vector2Int(col * 17, row * 2), "item", (WidgetContainer)Owner);
            ItemControl? script = item.AddScript<ItemControl>();
            SelectableText? selectableText = item.AddScript<SelectableText>();
            if (script == null)
            {
                item.Destroy();
            }
            else
            {
                item.TextColor = CUIColor.White;
                if (selectableText != null)
                {
                    selectableText.NormalColor = new ColorPair(CUIColor.White, CUIColor.NextBackgroundColor);
                    selectableText.SelectedColor = new ColorPair(CUIColor.Black, CUIColor.Blue);
                }
                script.IsDirectory = isDirectory;
                script.Path = path;
                items.Add(script);
            }
        }

        /// <summary>
        /// 清空现有项目
        /// </summary>
        void Clear()
        {
            foreach (ItemControl itemControl in items)
            {
                itemControl.Owner.Destroy();
            }
            items.Clear();
        }
    }
}