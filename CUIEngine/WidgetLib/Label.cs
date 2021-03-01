using System.Collections.Generic;
using System.Text;
using CUIEngine.Render;
using CUIEngine.Widgets;

namespace CUIEngine.WidgetLib
{
    /// <summary>
    /// 标签,一种用于显示文本的控件
    /// </summary>
    public class Label : Widget
    {
        string content = "";    //内容
        bool autoWarp = false;  //自动换行
        Color textColor = Color.DefaultColor;   //文本的颜色

        /// <summary>
        /// 文本内容,使用'\r'换行
        /// </summary>
        public string Content
        {
            get => content;
            set
            {
                if (!content.Equals(value))
                {
                    content = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 自动换行
        /// </summary>
        public bool AutoWarp
        {
            get => autoWarp;
            set
            {
                if (autoWarp != value)
                {
                    autoWarp = value;
                    UpdateRenderClip();
                }
            }
        }

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

                int x = 0, y = 0;
                RenderUnit unit = new RenderUnit(textColor);
                string temp = content;
                for (var i = 0; i < temp.Length; i++)
                {
                    //限制y大小
                    if(y < Size.Y)
                    {
                        char c = temp[i];
                        //换行符
                        if (c == '\r')
                        {
                            x = 0;
                            y += 1;
                            continue;
                        }
                        else if (x < Size.X)
                        {
                            unit.Content = c;
                            CurrentClip.SetUnit(x, y, unit);
                        }
                        x++;
                        if (autoWarp)
                        {
                            if (x >= Size.X)
                            {
                                x = 0;
                                y += 1;
                            }
                        }
                    }
                }
            }
        }
    }
}