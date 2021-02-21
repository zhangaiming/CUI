﻿using CUIEngine.Mathf;

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
        /// 根控件不允许被移动
        /// </summary>
        /// <param name="newCoord"></param>
        public override void Move(Vector2Int newCoord) { }
    }
}