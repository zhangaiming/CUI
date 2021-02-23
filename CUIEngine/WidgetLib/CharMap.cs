using CUIEngine.Render;
using CUIEngine.Widgets;

namespace CUIEngine.WidgetLib
{
    /// <summary>
    /// 字符图
    /// </summary>
    public class CharMap : Widget
    {
        string content = "";
        bool autoWarp = false;

        /// <summary>
        /// 字符图的文本内容,使用'\r'换行
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

        public bool AutoWarp
        {
            get => autoWarp;
            set
            {
                if (!autoWarp == value)
                {
                    autoWarp = value;
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
                RenderUnit unit;
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
                            unit = new RenderUnit(c);
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