using System;

namespace CUIEngine.Scripts.ScriptAttribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiredWidgetTypeAttribute : Attribute
    {
        /// <summary>
        /// 可以绑定此脚本的控件类型
        /// </summary>
        public Type? WidgetType { get; set; } = null;

        public RequiredWidgetTypeAttribute(Type? type = null)
        {
            WidgetType = type;
        }
    }
}