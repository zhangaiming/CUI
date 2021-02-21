using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CUIEngine.Mathf;
using CUIEngine.Render;

namespace CUIEngine.Widgets
{
    public abstract class Widget : ICanvas
    {
        /// <summary>
        /// 将被销毁时调用
        /// </summary>
        public event Action<Widget>? WidgetReadyToBeDestroyedEvent;
        
        protected RenderClip? CurrentClip;   //当前的渲染片段
        bool shouldUpdate = true;   //是否应该更新渲染片段
        bool isDestroyed = false;   //控件是否已被销毁,被销毁后的控件不应当进行渲染,也不应当对各类控件的方法作出回应
        string name = "";
        IWidgetOwner? parent = null;
        Vector2Int coord;
        Vector2Int size;

        /// <summary>
        /// 控件池,存放所有已创建的控件
        /// </summary>
        static Dictionary<string, List<Widget>> widgetPool = new Dictionary<string, List<Widget>>();
        
        /// <summary>
        /// 父控件,设为null代表无父控件
        /// </summary>
        public IWidgetOwner? Parent
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
        /// 控件是否已被销毁
        /// </summary>
        public bool IsDestroyed
        {
            get => isDestroyed;
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
            WidgetReadyToBeDestroyedEvent?.Invoke(this);
            OnDestroyed();
            DestroyWidget(this);
        }
        
        public RenderClip GetRenderClip()
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

        /// <summary>
        /// 设置父控件,设为null则默认为根控件
        /// </summary>
        /// <param name="owner"></param>
        public void SetParent(IWidgetOwner? owner)
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
            CurrentClip?.Resize(newSize,Vector2Int.Zero);
            size = newSize;
        }
        /// <summary>
        /// 对控件进行位移,对于某些控件可能不会成功
        /// </summary>
        /// <param name="newCoord"></param>
        public virtual void Move(Vector2Int newCoord)
        {
            CurrentClip?.Resize(size, newCoord - coord);
            coord = newCoord;
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
        public static TType CreateWidget<TType>(Vector2Int size, Vector2Int coord, string name, IWidgetOwner? parent = null) where TType : Widget, new()
        {
            TType widget = new TType();
            widget.coord = coord;
            widget.size = size;
            widget.name = name;
            widget.parent = parent;
            AddToPool(widget);
            
            parent?.AddWidget(widget);

            widget.Initialize();

            return widget;
        }
        /// <summary>
        /// 销毁控件,不会调用Widget自身的Destroy方法.如果需要在销毁前自动调用Widget的各类销毁前方法,则应该使用Widget对象的Destroy方法进行销毁.
        /// </summary>
        /// <param name="widget"></param>
        internal static void DestroyWidget(Widget widget)
        {
            widget.isDestroyed = true;
            RemoveFromPool(widget);
        }
        /// <summary>
        /// 通知控件进行渲染片段的更新,若此控件的拥有者也是一个控件,会同时更新拥有者的更新信息
        /// </summary>
        public void UpdateRenderClip()
        {
            shouldUpdate = true;
            if(shouldUpdate)
            {
                if (parent is Widget)
                {
                    ((Widget) parent).UpdateRenderClip();
                }
            }
        }
        /// <summary>
        /// 返回找到的第一个指定名字的控件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [return: MaybeNull]
        public static T FindWidget<T>(string name) where T : Widget
        {
            if (widgetPool.ContainsKey(name))
            {
                List<Widget> widgets = widgetPool[name];
                if (widgets.Count >= 1)
                {
                    return (T)widgets[0];
                }
            }

            return null;
        }

        /// <summary>
        /// 向控件池中添加指定控件
        /// </summary>
        /// <param name="widget"></param>
        static void AddToPool(Widget widget)
        {
            string name = widget.name;
            if (widgetPool.ContainsKey(name))
            {
                widgetPool[name].Add(widget);
            }
            else
            {
                widgetPool.Add(name, new List<Widget>() {widget});
            }
        }

        /// <summary>
        /// 从控件池中移除指定控件
        /// </summary>
        /// <param name="widget"></param>
        static void RemoveFromPool(Widget widget)
        {
            string name = widget.name;
            if (widgetPool.ContainsKey(name))
            {
                List<Widget> temp = widgetPool[name];
                temp.Remove(widget);
                if (temp.Count == 0)
                {
                    widgetPool.Remove(name);
                }
            }
        }
    }
}