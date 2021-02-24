using System.Collections.Generic;
using CUIEngine.Forms;
using CUIEngine.Mathf;
using CUIEngine.WidgetLib;
using CUIEngine.Widgets;

namespace CUIEngine.Render
{
    public class RootCanvas : ICanvas, IMultiWidgetsOwner
    {
        static RootCanvas? instance;
        RenderClip? currentClip;
        bool shouldUpdate = true;

        /// <summary>
        /// 实例
        /// </summary>
        public static RootCanvas Instance
        {
            get
            {
                if (instance == null)
                {
                    Initialize();
                }

                return instance!;
            }
        }

        List<ICanvas> canvasList = new List<ICanvas>();
        Vector2Int size;


        public Vector2Int Size => size;

        RootCanvas() { }

        static void Initialize()
        {
            if (instance == null)
            {
                instance = new RootCanvas();
                instance.size = Settings.ScreenSize;
                instance.currentClip = new RenderClip(instance.size, Vector2Int.Zero);
                /*TestWidget background =
                    Widget.CreateWidget<TestWidget>(instance.size, Vector2Int.Zero, "UIBackground", instance);*/
                Settings.OnScreenSizeChanged += instance.Resize;
            }
        }
        
        public RenderClip GetRenderClip()
        {
            if(shouldUpdate)
            {
                if (currentClip == null)
                {
                    currentClip = new RenderClip(size, Vector2Int.Zero);
                }

                currentClip.Clear();
                int cnt = canvasList.Count;
                for (int i = 0; i < cnt; i++)
                {
                    ICanvas canvas = canvasList[i];
                    currentClip.MergeWith(canvas.GetRenderClip(), null, true);
                }

                shouldUpdate = false;
            }
            return currentClip!;
        }

        public void UpdateRenderClip()
        {
            shouldUpdate = true;
        }

        void Resize(Vector2Int newSize)
        {
            size = newSize;
            currentClip?.Resize(size, Vector2Int.Zero);
        }

        public void AddWidget(Widget widget)
        {
            if (!ContainWidget(widget))
            {
                canvasList.Add(widget);
            }
        }

        public void RemoveWidget(Widget widget)
        {
            if (ContainWidget(widget))
            {
                canvasList.Remove(widget);
            }
        }

        public bool ContainWidget(Widget widget)
        {
            return canvasList.Contains(widget);
        }

        public int IndexOf(Widget widget)
        {
            int count = canvasList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (canvasList[i].Equals(widget))
                {
                    return i;
                }
            }
            return -1; 
        }
    }
}