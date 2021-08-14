using System.Collections.Generic;
using CUIEngine.Mathf;
using CUIEngine.Widgets;
using Log;

namespace CUIEngine.Render
{
    /// <summary>
    /// 根画布
    /// </summary>
    public class Root : ICanvas, IWidgetContainer
    {
        static Root? instance;
        RenderClip currentClip;
        bool shouldUpdate = true;

        RenderUnit fillStyle = new RenderUnit(new ColorPair(CUIColor.DarkGray, CUIColor.DarkGray));

        /// <summary>
        /// 实例
        /// </summary>
        public static Root Instance
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

        Root()
        {
            size = Settings.ScreenSize;
            currentClip = new RenderClip(size, Vector2Int.Zero);

            //关联屏幕尺寸调整事件
            Settings.OnScreenSizeChanged += Resize;
        }

        static void Initialize()
        {
            if (instance == null)
            {
                instance = new Root();
            }
        }
        
        public RenderClip GetRenderClip()
        {
            if(shouldUpdate)
            {
                currentClip.Clear();
                Drawer.Fill(currentClip, fillStyle);
                int cnt = canvasList.Count;
                for (int i = 0; i < cnt; i++)
                {
                    ICanvas canvas = canvasList[i];
                    RenderClip? clip = canvas.GetRenderClip();

                    //判断画布是否为空
                    if (clip != null)
                    {
                        currentClip.MergeWith(clip, null, true);
                        Logger.Log(clip.Coord);
                    }
                    
                    
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
            currentClip.Resize(size, Vector2Int.Zero);
            UpdateRenderClip();
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

        public void TopUpWidget(Widget widget)
        {
            int i = IndexOf(widget);
            if (i != -1)
            {
                canvasList.RemoveAt(i);
                canvasList.Add(widget);
                UpdateRenderClip();
            }
        }
    }
}