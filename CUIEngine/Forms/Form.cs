using CUIEngine.Widgets;

namespace CUIEngine.Forms
{
    /// <summary>
    /// 窗体抽象类,继承并实现这个类以创建一个新的窗体
    /// </summary>
    public abstract class Form : IWidgetOwner
    {
        RootWidget? rootWidget; //窗体的根控件

        /// <summary>
        /// 初始化窗体
        /// </summary>
        public void Initialize()
        {
            Widget.CreateWidget<RootWidget>()
            OnInitialize();
        }
        /// <summary>
        /// 销毁窗体
        /// </summary>
        public void Destroy()
        {
            OnDestroyed();
        }
        
        /// <summary>
        /// 在窗体进行初始化时调用
        /// </summary>
        protected abstract void OnInitialize();
        /// <summary>
        /// 在窗体被销毁前调用
        /// </summary>
        protected abstract void OnDestroyed();
    }
}