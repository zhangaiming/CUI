using System;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;

namespace CUIEngine.Forms
{
    /// <summary>
    /// 窗体抽象类,继承并实现这个类以创建一个新的窗体
    /// </summary>
    public abstract class Form : Widget, IMultiWidgetsOwner
    {
        bool isBorderless = false;
        string title = "";

        WidgetContainer? rootWidget = null!;
        
        public bool IsBorderless
        {
            get => isBorderless;
            set
            {
                if(isBorderless != value)
                {
                    isBorderless = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 窗体的标题
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                if(!title.Equals(value))
                {
                    title = value;
                    UpdateRenderClip();
                }
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            RootCanvas.Instance.AddWidget(this);
            
            rootWidget = Widget.CreateWidget<WidgetContainer>(Size, Coord, Name + "Root", "RootWidget", this);
        }

        public void AddWidget(Widget widget)
        {
            rootWidget?.AddWidget(widget);
        }

        public void RemoveWidget(Widget widget)
        {
            rootWidget.RemoveWidget(widget);
        }

        public bool ContainWidget(Widget widget)
        {
            return rootWidget.ContainWidget(widget);
        }

        public int IndexOf(Widget widget)
        {
            return rootWidget.IndexOf(widget);
        }

        protected override void MakeRenderClip()
        {
            CurrentClip?.Clear();
            CurrentClip?.MergeWith(rootWidget.GetRenderClip(), null, true);
        }
    }
}