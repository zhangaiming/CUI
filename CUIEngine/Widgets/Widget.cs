﻿using System;
using System.Diagnostics;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using DevToolSet;

namespace CUIEngine.Widgets
{
    public abstract class Widget : Sprite, ICanvas
    {
        /// <summary>
        /// 将被销毁时调用
        /// </summary>
        public event Action<Widget>? ToBeDestroyedHandler;
        
        protected RenderClip? CurrentClip;    //当前的渲染片段
        bool shouldUpdate = true;   //是否应该更新渲染片段
        IWidgetOwner parent = null!;
        Vector2Int coord;
        Vector2Int size;
        
        static IInteractive? selectingWidget = null;   //当前被选择的控件

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
                    //修改值
                    Vector2Int oldCoord = coord;
                    coord = value;
                    CurrentClip?.Resize(size, value - oldCoord);
                    OnCoordChanged(oldCoord, coord);

                    //更新渲染片段
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 控件的相对坐标,即相对于父控件的坐标(当父控件不是Sprite时此属性与Coord相同)
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
            set
            {
                if(parent is Sprite)
                {
                    Coord = ((Sprite) parent).Coord + value;
                }
                else
                {
                    Coord = value;
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
                    UpdateRenderClip();
                    OnVisibilityChanged();
                }
            }
        }

        static Widget()
        {
            //为控件交互注册按键
            Input.AttachHandler(EnterActiveWidget, ConsoleKey.Enter);
        }
        
        /// <summary>
        /// 初始化时调用
        /// </summary>
        protected virtual void OnInitialize(){}
        /// <summary>
        /// 销毁时调用(在ToBeDestroyedHandler之后被调用)
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
        /// 当控件可视性发生改变时调用
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
            ToBeDestroyedHandler?.Invoke(this);
            OnDestroyed();
            DestroySprite(this);
        }
        
        /// <summary>
        /// 返回控件的渲染片段,当控件不可视时返回null
        /// </summary>
        /// <returns></returns>
        public RenderClip? GetRenderClip()
        {
            if(isVisible)
            {

                if (shouldUpdate)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    MakeRenderClip();
                    sw.Stop();
                    Logger.Log(Name + "生成渲染片段所用时间: " + sw.Elapsed);
                    shouldUpdate = false;
                }

                return CurrentClip;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置父控件,设为null则默认为根控件
        /// </summary>
        /// <param name="owner"></param>
        public void SetParent(IWidgetOwner owner)
        {
            if (parent is IWidgetContainer)
            {
                ((IWidgetContainer)parent).RemoveWidget(this);
            }
            if (owner is IWidgetContainer)
            {
                ((IWidgetContainer)owner).AddWidget(this);
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
            UpdateParentRenderClip();
        }

        /// <summary>
        /// 通知父控件更新渲染片段
        /// </summary>
        void UpdateParentRenderClip()
        {
            if (parent is ICanvas)
            {
                ((ICanvas) parent).UpdateRenderClip();
            }
        }

        /// <summary>
        /// 设置交互对象
        /// </summary>
        /// <param name="target"></param>
        public static void SetSelection(Widget? target)
        {
            if (target == null)
            {
                selectingWidget?.Select(false);
                selectingWidget = null;
            }
            //判断目标是否可进行互动
            else if (target is IInteractive)
            {
                selectingWidget?.Select(false);
                selectingWidget = (IInteractive)target;
                selectingWidget.Select(true);
            }
        }

        /// <summary>
        /// 与正被激活的控件进行互动
        /// </summary>
        static void EnterActiveWidget(ConsoleKeyInfo info)
        {
            selectingWidget?.Interact();
        }
    }
}