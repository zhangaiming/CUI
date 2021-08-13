namespace CUIEngine.Scripts
{
    public interface IScriptOwner
    {
        /// <summary>
        /// 添加脚本
        /// </summary>
        /// <typeparam name="TScript">目标脚本类型</typeparam>
        void AddScript<TScript>() where TScript : Script, new();

        /// <summary>
        /// 从已绑定的脚本中获取指定类型的脚本
        /// </summary>
        /// <typeparam name="TScript">目标脚本类型</typeparam>
        /// <returns>若已绑定的脚本中有指定类型的脚本，则返回此脚本实例，否则返回null</returns>
        TScript? GetScript<TScript>() where TScript : Script, new();
    }
}