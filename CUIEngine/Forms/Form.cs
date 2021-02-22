using System;
using CUIEngine.Mathf;
using CUIEngine.Widgets;

namespace CUIEngine.Forms
{
    /// <summary>
    /// 窗体抽象类,继承并实现这个类以创建一个新的窗体
    /// </summary>
    public abstract class Form : WidgetContainer
    {
        bool isBorderless;
        string title = "";
        
        
    }
}