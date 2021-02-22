using System.Linq.Expressions;
using CUIEngine.Mathf;

namespace CUIEngine.Widgets
{
    public class RootWidget : WidgetContainer
    {
        /// <summary>
        /// 根控件的坐标
        /// </summary>
        public new Vector2Int Coord
        {
            get => base.Coord;
            private set => base.Coord = value;
        }

        /// <summary>
        /// 根控件的大小
        /// </summary>
        public new Vector2Int Size
        {
            get => base.Size;
            private set => base.Size = value;
        }

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
        internal void Resize(Vector2Int newSize)
        {
            Size = newSize;
        }
    }
}