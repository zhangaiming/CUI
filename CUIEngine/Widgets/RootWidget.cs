using CUIEngine.Mathf;

namespace CUIEngine.Widgets
{
    public class RootWidget : WidgetContainer
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
            ClipChildren = true;
            Settings.OnScreenSizeChanged += Resize;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            Settings.OnScreenSizeChanged -= Resize;
        }

        /// <summary>
        /// 设置大小
        /// </summary>
        /// <param name="newSize"></param>
        void Resize(Vector2Int newSize)
        {
            Size = newSize;
        }
    }
}