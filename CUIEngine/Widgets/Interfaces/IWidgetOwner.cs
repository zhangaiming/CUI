namespace CUIEngine.Widgets
{
    /// <summary>
    /// 一个标志接口,实现这个接口以表明可以作为控件的父亲
    /// </summary>
    public interface IWidgetOwner
    {
        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="widget"></param>
        void AddWidget(Widget widget);
        /// <summary>
        /// 移除控件
        /// </summary>
        /// <param name="widget"></param>
        void RemoveWidget(Widget widget);
        /// <summary>
        /// 更新所有子控件的渲染片段
        /// </summary>
        void UpdateChildrenRenderClip();
    }
}