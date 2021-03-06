using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CUIEngine.Inputs;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Scripts;
using CUIEngine.Scripts.ScriptAttribute;

namespace CUIEngine.Widgets
{
    public abstract class Widget : Sprite, ICanvas, IScriptOwner
    {
        RenderClip? currentClip = null;    //当前的渲染片段
        bool shouldUpdate = true;   //是否应该更新渲染片段
        IWidgetOwner? parent = null!;
        Vector2Int coord;
        Vector2Int size;

        bool isVisible = true;    //是否可见

        #region 基本属性

        /// <summary>
        /// 当前的渲染片段
        /// </summary>
        public RenderClip CurrentClip
        {
            get
            {
                if (currentClip == null)
                {
                    currentClip = new RenderClip(Size, Coord);
                }

                return currentClip;
            }
        }

        /// <summary>
        /// 父控件,赋值null则设根画布为parent
        /// </summary>
        public IWidgetOwner? Parent
        {
            get => parent!;
            private set
            {
                if (value == null)
                {
                    parent = value;
                    return;
                }
                if (parent != value)
                {
                    Vector2Int oldLocalCoord = LocalCoord;
                    parent?.RemoveWidget(this);
                    value.AddWidget(this);
                    UpdateParentRenderClip();
                    parent = value;
                    LocalCoord = oldLocalCoord;
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
                    
                    CurrentClip.Remap(Size, Coord);
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
                    //CurrentClip.Remap(Size, Coord);
                    CurrentClip.Coord = value;
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
                if (parent is Sprite sprite)
                {
                    return Coord - sprite.Coord;
                }
                else
                {
                    return Coord;
                }
            }
            set
            {
                if(parent is Sprite sprite)
                {
                    Coord = sprite.Coord + value;
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

        #endregion

        #region 虚方法

        /// <summary>
        /// 控件被创建时调用
        /// </summary>
        protected virtual void OnCreated(){}
        
        /// <summary>
        /// 销毁时调用
        /// </summary>
        protected virtual void OnDestroyed(){}

        /// <summary>
        /// 重新绘制渲染片段
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

        #endregion

        #region 控件的创建与销毁方法

        /// <summary>
        /// 创建指定类型的控件控件
        /// </summary>
        /// <param name="size">控件的大小</param>
        /// <param name="localCoord">控件的相对坐标</param>
        /// <param name="name">控件的名称</param>
        /// <param name="parent">父控件，若为空则设置父控件为Root</param>
        /// <typeparam name="TWidget">控件类型</typeparam>
        /// <returns>控件实例</returns>
        public static TWidget Create<TWidget>(Vector2Int size, Vector2Int localCoord, string name, IWidgetOwner? parent = null)
            where TWidget : Widget, new()
        {
            TWidget widget = new TWidget();
            
            widget.LocalCoord = localCoord;
            widget.Size = size;
            widget.Name = name;
            widget.Tag = "";
            
            widget.SetParent(parent ?? Root.Instance);

            Sprite.Initialize(widget);

            return widget;
        }

        /// <summary>
        /// 销毁控件
        /// </summary>
        public void Destroy()
        {
            OnDestroyed();
            
            //断开与父控件的联系
            ResetParent();

            IsVisible = false;
            
            DestroySprite(this);
        }

        #endregion

        #region 渲染相关方法

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
                    CurrentClip.Clear();
                    MakeRenderClip();
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
            if (parent is ICanvas canvas)
            {
                canvas.UpdateRenderClip();
            }
        }

        #endregion

        #region 控件关系操作方法

        /// <summary>
        /// 设置父控件
        /// </summary>
        /// <param name="owner"></param>
        public void SetParent(IWidgetOwner owner)
        {
            Parent = owner;
            
        }

        /// <summary>
        /// 解除与父控件的关系
        /// </summary>
        void ResetParent()
        {
            parent?.RemoveWidget(this);
            Parent = null;
        }

        #endregion

        #region 脚本功能字段及函数

        List<Script> bindScripts = new List<Script>();

        bool CheckScriptAttribute<TScript>() where TScript : Script
        {
            //Check TargetWidgetAttribute
            TargetWidgetAttribute[] targetWidgetAttributes = (TargetWidgetAttribute[])Attribute.GetCustomAttributes(typeof(TScript).Assembly, typeof(TargetWidgetAttribute));
            if (targetWidgetAttributes.Length != 0)
            {
                foreach (TargetWidgetAttribute attribute in targetWidgetAttributes)
                {
                    if (attribute.WidgetType == this.GetType())
                    {
                        return false;
                    }
                }
            }
            
            //Check RequiredScriptAttribute
            RequiredScriptAttribute[] requiredScriptAttributes = (RequiredScriptAttribute[])Attribute.GetCustomAttributes(typeof(TScript).Assembly, typeof(RequiredScriptAttribute));
            if (requiredScriptAttributes.Length != 0)
            {
                foreach (RequiredScriptAttribute requiredScriptAttribute in requiredScriptAttributes)
                {
                    Type? type = requiredScriptAttribute.Script;
                    if (!HasScript(type))
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
        
        public TScript? AddScript<TScript>() where TScript : Script, new()
        {
            if (!CheckScriptAttribute<TScript>())
            {
                throw new Exception("绑定脚本失败。");
            }

            TScript script = new TScript();
            bindScripts.Add(script);
            typeof(Script).GetMethod("AwakeThis", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(script, new object?[]{this});
            return script;
        }

        public TScript? GetScript<TScript>() where TScript : Script, new()
        {
            foreach (Script bindScript in bindScripts)
            {
                if (bindScript.GetType() == typeof(TScript))
                {
                    return (TScript)bindScript;
                }
            }

            return null;
        }

        /// <summary>
        /// 控件有没有目标类型的脚本
        /// </summary>
        /// <param name="scriptType">脚本类型</param>
        /// <returns></returns>
        public bool HasScript(Type scriptType)
        {
            if (!scriptType.IsSubclassOf(typeof(Script)))
            {
                return false;
            }
            foreach (Script bindScript in bindScripts)
            {
                if (bindScript.GetType() == scriptType)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}