using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.WidgetLib;
using CUIEngine.Widgets;

namespace CUIEngine.Forms
{
    /// <summary>
    /// 窗体抽象类,继承并实现这个类以创建一个新的窗体
    /// </summary>
    public abstract class Form : Widget, IWidgetContainer
    {
        bool showTitle = true;
        bool isBorderless = false;
        string title = "";

        static Vector2Int defaultSize = new Vector2Int(30, 30);

        WidgetContainer rootWidget;
        Panel border;
        Label titleWidget;

        bool isInitialized = false;

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
                    border.IsVisible = !isBorderless;
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
                    titleWidget.Text = title;
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

        public void AddWidget(Widget widget)
        {
            if(isInitialized)
                widget.SetParent(rootWidget);
        }

        public void RemoveWidget(Widget widget)
        {
            if(isInitialized)
                rootWidget.RemoveWidget(widget);
        }

        public bool ContainWidget(Widget widget)
        {
            if(isInitialized)
            {
                return rootWidget.ContainWidget(widget);
            }
            else
            {
                return false;
            }
        }

        public int IndexOf(Widget widget)
        {
            if(isInitialized)
            {
                return rootWidget.IndexOf(widget);
            }
            else
            {
                return -1;
            }
        }

        public void TopUpWidget(Widget widget)
        {
            if(isInitialized)
                rootWidget.TopUpWidget(widget);
        }

        protected override void MakeRenderClip()
        {
            CurrentClip.Clear();
            RenderClip? clip;
            
            clip = rootWidget.GetRenderClip();
            if(clip != null)
                CurrentClip.MergeWith(clip, null, true);
            
            clip = border.GetRenderClip();
            if(clip != null)
                CurrentClip.MergeWith(clip, null, true);
            
            clip = titleWidget.GetRenderClip();
            if(clip != null)
                CurrentClip.MergeWith(clip, null, true);
            
        }

        protected override void OnCoordChanged(Vector2Int oldCoord, Vector2Int newCoord)
        {
            base.OnCoordChanged(oldCoord, newCoord);
            Vector2Int offset = newCoord - oldCoord;
            border.Coord += offset;
            
            titleWidget.Coord += offset;
            
            rootWidget.Coord += offset;
        }

        protected override void OnSizeChanged(Vector2Int oldSize, Vector2Int newSize)
        {
            base.OnSizeChanged(oldSize, newSize);
            border.Size = newSize;
            titleWidget.Size = new Vector2Int(newSize.X - 2, 1);
            rootWidget.Size = newSize + new Vector2Int(-2, -2);
        }

        protected Form(Vector2Int coord, string name, string tag = "") : base(defaultSize, coord, RootCanvas.Instance, name, tag)
        {
            isInitialized = false;
            //初始化边框
            border = new Panel(Size, Coord, this, Name + "Border", "FormBorder")
            {
                DrawType = PanelDrawType.BorderOnly
            };
            titleWidget = new Label(new Vector2Int(Size.X - 2, 1), Coord + new Vector2Int(1, 0), this, Name + "Title", "FormTitleWidget")
            {
                Text = title
            };

            isInitialized = true;
            
            //初始化根控件
            rootWidget = new WidgetContainer(Size + new Vector2Int(-2, -2), Coord + new Vector2Int(1, 1), this, Name + "Root", "RootWidget");
            rootWidget.ClipChildren = true;
        }
    }
}