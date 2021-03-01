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
        Color textColor = Color.DefaultColor;   //文本的颜色

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
        /// 文本的颜色
        /// </summary>
        public Color TextColor
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
            if(CurrentClip != null)
            {
                CurrentClip.Clear();
                
                RenderUnit unit = new RenderUnit(textColor);
                string temp = text;
                for (var x = 0; x < Math.Min(temp.Length, Size.X); x++)
                {
                    unit.Content = temp[x];
                    CurrentClip.SetUnit(x, 0, unit);
                }
            }
        }

        protected override void OnCoordChanged(Vector2Int oldCoord, Vector2Int newCoord)
        {
            base.OnCoordChanged(oldCoord, newCoord);
            Size = new Vector2Int(Size.X, 1);
        }
    }
}