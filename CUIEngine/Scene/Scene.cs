using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace CUIEngine.Scene
{
    /// <summary>
    /// 场景类,实现此类并添加场景控件的回调函数
    /// </summary>
    public abstract class Scene
    {
        /// <summary>
        /// 在加载此场景时调用
        /// </summary>
        public virtual void OnLoad()
        {
            
        }

        /// <summary>
        /// 在卸载此场景时调用
        /// </summary>
        public virtual void OnUnload()
        {
            
        }
    }
}