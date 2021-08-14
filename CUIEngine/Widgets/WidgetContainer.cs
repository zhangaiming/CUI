using System.Collections.Generic;
using CUIEngine.Mathf;
using CUIEngine.Render;

namespace CUIEngine.Widgets
{
    public class WidgetContainer : Widget, IWidgetContainer
    {
        List<Widget> children = new List<Widget>();   //越靠后优先级越高,越被渲染在上层
        bool clipChildren = false;

        /// <summary>
        /// 是否对子控件的渲染片段进行裁剪,使其只显示在容器大小的范围内
        /// </summary>
        public bool ClipChildren
        {
            get => clipChildren;
            set => clipChildren = value;
        }

        protected override void MakeRenderClip()
        {
            CurrentClip.Clear();
            CurrentClip.MergeWith(GetChildrenRenderClip(), null, ClipChildren);
        }

        /// <summary>
        /// 获取未裁剪的由所有子控件按优先级叠加的渲染片段
        /// </summary>
        /// <returns></returns>
        protected RenderClip GetChildrenRenderClip()
        {
            RenderClip res = new RenderClip();
            Widget[] temp = new Widget[children.Count];
            children.CopyTo(temp);
            RenderClip? clip;
            foreach (Widget widget in temp)
            {
                clip = widget.GetRenderClip();
                if(clip != null)
                {
                    res.MergeWith(clip, null, false);
                }
            }

            return res;
        }
        /// <summary>
        /// 添加控件到最上层
        /// </summary>
        /// <param name="widget"></param>
        public void AddWidget(Widget widget)
        {
            children.Add(widget);
            UpdateRenderClip();
        }
        public void RemoveWidget(Widget widget)
        {
            if (ContainWidget(widget))
            {
                children.Remove(widget);
                UpdateRenderClip();
            }
        }
        /// <summary>
        /// 根据索引移除控件
        /// </summary>
        /// <param name="index"></param>
        public void RemoveWidget(int index)
        {
            if (index > -1 && index < children.Count)
            {
                children.RemoveAt(children.Count - index - 1);
                UpdateRenderClip();
            }
        }
        public bool ContainWidget(Widget widget)
        {
            return children.Contains(widget);
        }
        public void TopUpWidget(Widget widget)
        {
            int i = IndexOf(widget);
            if (i != -1)
            {
                children.RemoveAt(i);
                children.Add(widget);
                UpdateRenderClip();
            }
        }
        public int IndexOf(Widget widget)
        {
            int count = children.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (children[i].Equals(widget))
                {
                    return i;
                }
            }
            return -1; 
        }

        protected override void OnCoordChanged(Vector2Int oldCoord, Vector2Int newCoord)
        {
            base.OnCoordChanged(oldCoord, newCoord);
            MoveChildren(newCoord - oldCoord);
        }

        /// <summary>
        /// 子控件相对移动
        /// </summary>
        /// <param name="offset"></param>
        protected void MoveChildren(Vector2Int offset)
        {
            foreach (Widget widget in children)
            {
                //子控件相对移动
                widget.Coord = widget.Coord + offset;
            }
        }
    }
}