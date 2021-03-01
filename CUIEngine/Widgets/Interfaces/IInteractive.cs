namespace CUIEngine.Widgets
{
    /// <summary>
    /// 可进行交互的
    /// </summary>
    public interface IInteractive
    {
        /// <summary>
        /// 设置激活状态,此方法应由引擎调用
        /// </summary>
        /// <param name="state">激活状态</param>
        public void Select(bool state);

        /// <summary>
        /// 交互
        /// </summary>
        public void Interact();
    }
}