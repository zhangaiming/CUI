using System;

namespace CUIEngine.Scripts.ScriptAttribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TargetWidgetAttribute : Attribute
    {
        /// <summary>
        /// 可以绑定此脚本的控件类型
        /// </summary>
        public Type WidgetType { get; set; }

        public TargetWidgetAttribute(Type type)
        {
            WidgetType = type;
        }
    }
}