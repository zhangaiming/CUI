using System;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;

namespace CUIEngine.WidgetLib
{
    /// <summary>
    /// 标签,一种用于显示单行文本的控件
    /// </summary>
    public class Label : Widget
    {
        string text = "";    //内容
        ColorPair textColor = new ColorPair(CUIColor.DarkGray, CUIColor.NextBackgroundColor);   //文本的颜色

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if (!text.Equals(value))
                {
                    text = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public CUIColor FontColor
        {
            get => textColor.ForegroundColor;
            set
            {
                if (textColor.ForegroundColor != value)
                {
                    textColor.ForegroundColor = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 标签背景颜色
        /// </summary>
        public CUIColor BackgroundColor
        {
            get => textColor.BackgroundColor;
            set
            {
                if (textColor.BackgroundColor != value)
                {
                    textColor.BackgroundColor = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 标签颜色
        /// </summary>
        public ColorPair TextColor
        {
            get => textColor;
            set
            {
                if (textColor != value)
                {
                    textColor = value;
                    UpdateRenderClip();
                }
            }
        }

        protected override void MakeRenderClip()
        {
            CurrentClip.Clear();
                
            RenderUnit unit = new RenderUnit(textColor);
            string temp = text;
            int times = Math.Min(temp.Length, Size.X);
            for (var x = 0; x < times; x++)
            {
                unit.Content = temp[x];
                CurrentClip.SetUnit(x, 0, unit);
            }
        }

        protected override void OnSizeChanged(Vector2Int oldSize, Vector2Int newSize)
        {
            base.OnSizeChanged(oldSize, newSize);
            CurrentClip.Resize(new Vector2Int(newSize.X, 1), Vector2Int.Zero);
        }
    }
}