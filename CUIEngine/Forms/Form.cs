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
        bool showTitle = true;
        bool isBorderless = false;
        string title = "";

        static Vector2Int defaultSize = new Vector2Int(30, 30);

        WidgetContainer? rootWidget = null!;

        /// <summary>
        /// 是否显示标题
        /// </summary>
        public bool ShowTitle
        {
            get => showTitle;
            set
            {
                if (showTitle != value)
                {
                    showTitle = value;
                    UpdateRenderClip();
                }
            }
        }
        
        /// <summary>
        /// 窗体是否无边框
        /// </summary>
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

        /// <summary>
        /// 窗体创建时使用的默认大小
        /// </summary>
        public static Vector2Int DefaultSize
        {
            get => defaultSize;
            protected set => defaultSize = value;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            RootCanvas.Instance.AddWidget(this);
            
            rootWidget = CreateWidget<WidgetContainer>(Size, Coord, Name + "Root", "RootWidget", this);
        }
        
        /// <summary>
        /// 创建窗体
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="name"></param>
        /// <param name="tag"></param>
        /// <typeparam name="TForm"></typeparam>
        /// <returns></returns>
        public static TForm Create<TForm>(Vector2Int coord, string name, string tag = "") where TForm : Form, new()
        {
            TForm form = CreateWidget<TForm>(Vector2Int.Zero, coord, name, tag, RootCanvas.Instance);
            form.Size = defaultSize;
            return form;
        }
        
        public void AddWidget(Widget widget)
        {
            rootWidget?.AddWidget(widget);
        }

        public void RemoveWidget(Widget widget)
        {
            rootWidget?.RemoveWidget(widget);
        }

        public bool ContainWidget(Widget widget)
        {
            return rootWidget?.ContainWidget(widget) ?? false;
        }

        public int IndexOf(Widget widget)
        {
            return rootWidget?.IndexOf(widget) ?? -1;
        }

        protected override void MakeRenderClip()
        {
            CurrentClip?.Clear();
            if(rootWidget != null)
                CurrentClip?.MergeWith(rootWidget.GetRenderClip(), null, true);
        }
    }
}