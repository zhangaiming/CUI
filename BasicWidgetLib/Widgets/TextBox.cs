using System;
using System.Collections.Generic;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;

namespace BasicWidgetLib.Widgets
{
    
    /// <summary>
    /// 控件大小自适应类型
    /// </summary>
    [Flags]
    public enum AutoSizeType
    {
        NoAutoSize = 0,
        AutoWidth = 1,
        AutoHeight = 2
    }
    
    /// <summary>
    /// 文本块，用于显示多行文本
    /// </summary>
    public class TextBox : Widget
    {
        ColorPair color = new ColorPair(CUIColor.DarkGray, CUIColor.NextBackgroundColor);

        string text = "";

        AutoSizeType autoSize = AutoSizeType.NoAutoSize;

        bool autoWrap = false;

        List<char[]> charMap = new List<char[]>();

        /// <summary>
        /// 文本的颜色
        /// </summary>
        public CUIColor TextColor
        {
            get => color.ForegroundColor;
            set
            {
                if (color.ForegroundColor != value)
                {
                    color.ForegroundColor = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 文本块的背景颜色
        /// </summary>
        public CUIColor BackgroundColor
        {
            get => color.BackgroundColor;
            set
            {
                if (color.BackgroundColor != value)
                {
                    color.BackgroundColor = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    text = value;
                    UpdateCharMap();
                }
            }
        }

        /// <summary>
        /// 是否根据文本内容自动调整控件高度
        /// </summary>
        public AutoSizeType AutoSize
        {
            get => autoSize;
            set
            {
                if (autoSize != value)
                {
                    autoSize = value;
                    UpdateCharMap();
                }
            }
        }

        /// <summary>
        /// 是否根据控件大小自动为文本换行
        /// </summary>
        public bool AutoWrap
        {
            get => autoWrap;
            set
            {
                if (autoWrap != value)
                {
                    autoWrap = value;
                    UpdateCharMap();
                }
            }
        }

        protected override void OnSizeChanged(Vector2Int oldSize, Vector2Int newSize)
        {
            base.OnSizeChanged(oldSize, newSize);
            if (AutoWrap)
            {
                UpdateCharMap();
            }
        }

        /// <summary>
        /// 更新字符map
        /// </summary>
        void UpdateCharMap()
        {
            List<char[]> tmpMap = new List<char[]>();
            string[] strings = text.Split(new char[] { '\n', '\r' });

            if (AutoWrap && !AutoSize.HasFlag(AutoSizeType.AutoWidth))
            {
                //处理自动换行
                foreach (string s in strings)
                {
                    int line = 0;
                    while (line * Size.X < s.Length)
                    {
                        tmpMap.Add(s.ToCharArray(line * Size.X, Math.Min(Size.X, s.Length - line * Size.X)));
                        ++line;
                    }
                }
            }
            else
            {
                foreach (string s in strings)
                {
                    tmpMap.Add(s.ToCharArray());
                }
            }

            int maxLength = 0;
            Vector2Int newSize = Size;

            if (autoSize.HasFlag(AutoSizeType.AutoWidth))
            {
                foreach (char[] chars in tmpMap)
                {
                    maxLength = Math.Max(chars.Length, maxLength);
                }

                newSize.X = maxLength;
            }

            if (AutoSize.HasFlag(AutoSizeType.AutoHeight))
            {
                newSize.Y = tmpMap.Count;
            }
            
            //更新字符图
            charMap = tmpMap;
            
            //调整控件大小
            Size = newSize;
            UpdateRenderClip();
        }
        
        protected override void MakeRenderClip()
        {
            CurrentClip.Clear();

            RenderUnit unit = new RenderUnit(color, ' ');

            int sizeX;
            int sizeY = Math.Min(charMap.Count, Size.Y);

            char[] chars;
            char c;
            
            for (int y = 0; y < sizeY; ++y)
            {
                chars = charMap[y];
                sizeX = chars.Length;
                for (int x = 0; x < sizeX; ++x)
                {
                    c = chars[x];
                    if (c != '\0')
                    {
                        unit.Content = c;
                        CurrentClip.SetUnit(x, y, unit);
                    }
                }
            }
        }
    }
}