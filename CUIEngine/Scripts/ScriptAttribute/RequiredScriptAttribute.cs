using System;

namespace CUIEngine.Scripts.ScriptAttribute
{
    /// <summary>
    /// 要求添加此脚本的控件必须已拥有目标类型的脚本
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiredScriptAttribute : Attribute
    {
        /// <summary>
        /// 要求的脚本类型
        /// </summary>
        public Type Script { get; set; }

        public RequiredScriptAttribute(Type script)
        {
            Script = script;
        }
    }
}