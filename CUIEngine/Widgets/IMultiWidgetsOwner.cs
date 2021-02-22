namespace CUIEngine.Widgets
{
    public interface IMultiWidgetsOwner : IWidgetOwner
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

        /// <summary>
        /// 获取某个控件的在该容器中的索引,索引越小说明优先级越高,索引为-1代表控件不在该容器中
        /// </summary>
        /// <param name="widget"></param>
        /// <returns>控件的索引,返回-1代表控件不在该容器中</returns>
        public int IndexOf(Widget widget);
    }
}