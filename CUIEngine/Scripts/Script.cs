using System.Reflection;
using CUIEngine.Render;
using CUIEngine.Widgets;

namespace CUIEngine.Scripts
{
    /// <summary>
    /// 脚本基类
    /// Awake()-在脚本初始化完成后被调用
    /// Update()-在每一帧完成渲染后调用
    /// OnEnabled()-在脚本被启用时调用
    /// </summary>
    public abstract class Script
    {
        bool isEnabled = false;

        /// <summary>
        /// 拥有此脚本的控件
        /// </summary>
        public Widget Owner { get; private set; } = null!;

        /// <summary>
        /// 此脚本是否被启用
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    InvokeMethod("OnEnabled");
                }
            }
        }

        void CallUpdate()
        {
            if(isEnabled)
                InvokeMethod("Update");
        }

        /// <summary>
        /// 通过反射调用实例成员方法
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        void InvokeMethod(string methodName, object?[]? args = null)
        {
            MethodInfo? method = this.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(this, args);
        }

        /// <summary>
        /// 激活此脚本
        /// </summary>
        void AwakeThis(Widget owner)
        {
            Owner = owner;
            Renderer.OnRenderFinishedHandlers += CallUpdate;
            InvokeMethod("Awake");
            IsEnabled = true;
        }
    }
}