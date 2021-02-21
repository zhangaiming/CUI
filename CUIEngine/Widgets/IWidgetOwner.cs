namespace CUIEngine.Widgets
{
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
        /// 判断是否包含某控件
        /// </summary>
        /// <param name="widget"></param>
        bool ContainWidget(Widget widget);
    }
}