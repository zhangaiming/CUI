using System;
using CUIEngine.Mathf;
using CUIEngine.Render;

namespace CUIEngine.Widgets
{
    public abstract class Widget : Sprite, ICanvas
    {
        /// <summary>
        /// 将被销毁时调用
        /// </summary>
        public event Action<Widget>? WidgetReadyToBeDestroyedEvent;
        
        protected RenderClip? CurrentClip;   //当前的渲染片段
        bool shouldUpdate = true;   //是否应该更新渲染片段
        IWidgetOwner parent = null!;
        Vector2Int coord;
        Vector2Int size;

        bool isVisible = true;    //是否可见

        /// <summary>
        /// 父控件,设为null代表无父控件
        /// </summary>
        public IWidgetOwner Parent
        {
            get => parent!;
            private set
            {
                if (parent != value)
                {
                    UpdateParentRenderClip();
                    parent = value;
                    UpdateParentRenderClip();
                }
            }
        }

        /// <summary>
        /// 控件的大小
        /// </summary>
        public sealed override Vector2Int Size
        {
            get => size;
            set
            {
                value.X = Math.Max(0, value.X);
                value.Y = Math.Max(0, value.Y);
                if(size != value)
                {
                    Vector2Int oldSize = size;
                    size = value;
                    CurrentClip?.Resize(value, Vector2Int.Zero);
                    OnSizeChanged(oldSize, size);
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 控件的坐标,这里指绝对坐标
        /// </summary>
        public sealed override Vector2Int Coord
        {
            get => coord;
            set
            {
                if(coord != value)
                {
                    Vector2Int oldCoord = coord;
                    coord = value;
                    CurrentClip?.Resize(size, value - coord);
                    OnCoordChanged(oldCoord, coord);
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 控件的相对坐标,即相对于父控件的坐标
        /// </summary>
        public Vector2Int LocalCoord
        {
            get
            {
                if (parent is Sprite)
                {
                    return Coord - ((Sprite) parent).Coord;
                }
                else
                {
                    return Coord;
                }
            }
        }

        /// <summary>
        /// 控件是否可视
        /// </summary>
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    OnVisibilityChanged();
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
        protected abstract void MakeRenderClip();
        
        /// <summary>
        /// 当控件大小发生改变时调用 
        /// </summary>
        protected virtual void OnSizeChanged(Vector2Int oldSize, Vector2Int newSize){}
        
        /// <summary>
        /// 当控件位置发生改变时调用
        /// </summary>
        protected virtual void OnCoordChanged(Vector2Int oldCoord, Vector2Int newCoord){}
        
        /// <summary>
        /// 当控件可视度发生改变时调用
        /// </summary>
        protected virtual void OnVisibilityChanged(){}
        
        /// <summary>
        /// 初始化控件
        /// </summary>
        public void Initialize()
        {
            //初始化渲染片段
            CurrentClip = new RenderClip(size, coord);
            
            OnInitialize();
        }
        /// <summary>
        /// 销毁控件
        /// </summary>
        public void Destroy()
        {
            WidgetReadyToBeDestroyedEvent?.Invoke(this);
            OnDestroyed();
            DestroySprite(this);
        }
        
        public RenderClip GetRenderClip()
        {
            if(isVisible)
            {
                if (CurrentClip == null)
                {
                    CurrentClip = new RenderClip(size, coord);
                }

                if (shouldUpdate)
                {
                    MakeRenderClip();
                    shouldUpdate = false;
                }

                return CurrentClip;
            }
            else
            {
                //返回空片段
                return new RenderClip();
            }
        }

        /// <summary>
        /// 设置父控件,设为null则默认为根控件
        /// </summary>
        /// <param name="owner"></param>
        public void SetParent(IWidgetOwner owner)
        {
            if (parent is IMultiWidgetsOwner)
            {
                ((IMultiWidgetsOwner)parent).RemoveWidget(this);
            }
            if (owner is IMultiWidgetsOwner)
            {
                ((IMultiWidgetsOwner)owner).AddWidget(this);
            }
            Parent = owner;
            
        }

        /// <summary>
        /// 创建指定类型的控件,创建完毕后会自动调用控件的Initialize方法
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="tag"></param>
        /// <param name="parent"></param>
        /// <typeparam name="TType">控件的类型</typeparam>
        /// <returns></returns>
        public static TType CreateWidget<TType>(Vector2Int size, Vector2Int coord, string name, string tag, IWidgetOwner parent) where TType : Widget, new()
        {
            TType widget = new TType();
            widget.Coord = coord;
            widget.Size = size;
            widget.Name = name;
            widget.Tag = tag;
            widget.SetParent(parent);

            Sprite.Initialize(widget);

            widget.Initialize();

            return widget;
        }

        /// <summary>
        /// 创建指定类型的控件,创建完毕后会自动调用控件的Initialize方法
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <typeparam name="TType">控件的类型</typeparam>
        /// <returns></returns>
        public static TType CreateWidget<TType>(Vector2Int size, Vector2Int coord, string name, IWidgetOwner parent)
            where TType : Widget, new()
        {
            return CreateWidget<TType>(size, coord, name, "", parent);
        }
        
        /// <summary>
        /// 通知控件进行渲染片段的更新,若此控件的拥有者也是一个控件,会同时更新拥有者的更新信息
        /// </summary>
        public void UpdateRenderClip()
        {
            shouldUpdate = true;
            if(shouldUpdate)
            {
                UpdateParentRenderClip();
            }
        }

        /// <summary>
        /// 通知父控件更新渲染片段
        /// </summary>
        void UpdateParentRenderClip()
        {
            if (parent is Widget)
            {
                ((Widget) parent).UpdateRenderClip();
            }
        }
    }
}