using System.Collections.Generic;

namespace CUIEngine_Framework.Widgets
{
    public class WidgetContainer : Widget, IWidgetOwner
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
            CurrentClip?.Clear();
            Widget[] temp = new Widget[children.Count];
            children.CopyTo(temp);
            foreach (Widget widget in temp)
            {
                CurrentClip?.MergeWith(widget.GetRenderClip(), null, false);
            }
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
                widget.SetParent(null);
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
        /// <summary>
        /// 将某个已存在与该容器中的控件置顶
        /// </summary>
        /// <param name="widget"></param>
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
        /// <summary>
        /// 获取某个控件的在该容器中的索引,索引越小说明优先级越高,索引为-1代表控件不在该容器中
        /// </summary>
        /// <param name="widget"></param>
        /// <returns>控件的索引,返回-1代表控件不在该容器中</returns>
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
    }
}