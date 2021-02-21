//todo: 对处于屏幕外的控件进行剔除

using System;
using CUIEngine.Mathf;
using CUIEngine.Render;

namespace CUIEngine.Widgets
{
    public abstract class Widget : ICanvas
    {
        /// <summary>
        /// 将被销毁时调用
        /// </summary>
        public event Action<Widget> WidgetReadyToBeDestroyedEvent;
        
        protected RenderClip CurrentClip;   //当前的渲染片段
        bool shouldUpdate = true; //是否应该更新渲染片段
        IWidgetOwner parent = null;
        Vector2Int coord;
        Vector2Int size;
        
        /// <summary>
        /// 父控件,设为null代表无父控件
        /// </summary>
        public IWidgetOwner Parent
        {
            get => parent;
            protected set => parent = value;
        }

        /// <summary>
        /// 控件的大小
        /// </summary>
        public Vector2Int Size
        {
            get => size;
            protected set => size = value;
        }

        /// <summary>
        /// 控件的坐标,这里指绝对坐标
        /// </summary>
        public Vector2Int Coord
        {
            get => size;
            protected set => size = value;
        }
        /// <summary>
        /// 是否应该更新渲染片段,若设置为应该更新,则会让父控件也进行更新
        /// </summary>
        public bool ShouldUpdate
        {
            get => shouldUpdate;
            protected set
            {
                shouldUpdate = value;
                if(shouldUpdate)
                {
                    if (parent is Widget)
                    {
                        ((Widget) parent).ShouldUpdate = value;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化时调用
        /// </summary>
        protected virtual void OnInitialize(){}
        /// <summary>
        /// 销毁时调用(在WidgetReadyToBeDestroyedEvent之后被调用)
        /// </summary>
        protected virtual void OnDestroyed(){}

        /// <summary>
        /// 更新渲染片段
        /// </summary>
        public abstract void UpdateRenderClip();
        
        /// <summary>
        /// 初始化控件
        /// </summary>
        public void Initialize()
        {
            OnInitialize();
            
            //初始化渲染片段
            CurrentClip = new RenderClip(size, coord);
        }
        /// <summary>
        /// 销毁控件
        /// </summary>
        public void Destroy()
        {
            OnDestroyed();
        }
        
        public RenderClip GetRenderClip()
        {
            if (ShouldUpdate)
            {
                UpdateRenderClip();
                ShouldUpdate = false;
            }
            return CurrentClip;
        }

        /// <summary>
        /// 设置父控件,设为null则默认为根控件
        /// </summary>
        /// <param name="owner"></param>
        public void SetParent(IWidgetOwner owner)
        {
            parent?.RemoveWidget(this);
            owner?.AddWidget(this);
            parent = owner;
        }
        /// <summary>
        /// 对控件进行缩放,对于某些控件可能不会成功
        /// </summary>
        /// <param name="newSize"></param>
        public virtual void Resize(Vector2Int newSize)
        {
            CurrentClip.Resize(newSize,Vector2Int.Zero);
            size = newSize;
        }
        /// <summary>
        /// 对控件进行位移,对于某些控件可能不会成功
        /// </summary>
        /// <param name="newCoord"></param>
        public virtual void Move(Vector2Int newCoord)
        {
            CurrentClip.Resize(size, newCoord - coord);
            coord = newCoord;
        }
        
        /// <summary>
        /// 创建指定类型的控件,创建完毕后会自动调用控件的Initialize方法
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="size"></param>
        /// <param name="parent"></param>
        /// <param name="weight"></param>
        /// <typeparam name="TType">控件的类型</typeparam>
        /// <returns></returns>
        public static TType CreateWidget<TType>(Vector2Int size, Vector2Int coord, IWidgetOwner parent = null) where TType : Widget, new()
        {
            //不可是基类
            if (typeof(TType) == typeof(Widget))
            {
                return null;
            }

            TType widget = new TType();
            widget.coord = coord;
            widget.size = size;
            widget.parent = parent;
            parent?.AddWidget(widget);

            widget.Initialize();

            return widget;
        }
    }
}