﻿using System;
using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;

namespace CUIEngine.WidgetLib
{
    /// <summary>
    /// 色块
    /// </summary>
    [Obsolete("此控件将不再进行维护,若需实现色块绘制功能,应选择通用性更高的Panel控件.")]
    public class ColorBlock : Widget
    {
        CUIColor color = CUIColor.Black;
        public CUIColor Color
        {
            get => color;
            set
            {
                color = value;
                UpdateRenderClip();
            }
        }

        protected override void MakeRenderClip()
        {
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    CurrentClip?.SetUnit(i, j, new RenderUnit(new ColorPair(CUIColor.DarkGray, Color)));
                }
            }
        }

        public ColorBlock(Vector2Int size, Vector2Int coord, IWidgetOwner parent, string name, string tag = "") : base(size, coord, parent, name, tag)
        {
        }
    }
}