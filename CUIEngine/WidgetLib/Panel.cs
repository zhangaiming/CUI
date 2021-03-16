using CUIEngine.Mathf;
using CUIEngine.Render;
using CUIEngine.Widgets;

namespace CUIEngine.WidgetLib
{
    /// <summary>
    /// 用于描述面板绘制的类型
    /// </summary>
    public enum PanelDrawType
    {
        /// <summary>
        /// 同时绘制边框与填充
        /// </summary>
        FillAndBorder,
        
        /// <summary>
        /// 仅填充
        /// </summary>
        FillOnly,
        
        /// <summary>
        /// 仅边框
        /// </summary>
        BorderOnly
    }
    
    /// <summary>
    /// 可选择边框和填充绘制方式的面板
    /// </summary>
    public class Panel : Widget
    {
        char borderChar = '#';
        char fillChar = ' ';

        ColorPair borderColor = ColorPair.DefaultColorPair;
        ColorPair fillColor = ColorPair.DefaultColorPair;
        
        PanelDrawType drawType = PanelDrawType.FillAndBorder;

        /// <summary>
        /// 用于绘制边框的字符
        /// </summary>
        public char BorderChar
        {
            get => borderChar;
            set
            {
                if (borderChar != value)
                {
                    borderChar = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 用于填充面板的字符
        /// </summary>
        public char FillChar
        {
            get => fillChar;
            set
            {
                if (fillChar != value)
                {
                    fillChar = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 面板的绘制方式
        /// </summary>
        public PanelDrawType DrawType
        {
            get => drawType;
            set
            {
                if (drawType != value)
                {
                    drawType = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 边框的颜色
        /// </summary>
        public ColorPair BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 填充的颜色
        /// </summary>
        public ColorPair FillColor
        {
            get => fillColor;
            set
            {
                if (fillColor != value)
                {
                    fillColor = value;
                    UpdateRenderClip();
                }
            }
        }

        public Panel(Vector2Int size, Vector2Int coord, IWidgetOwner parent, string name, string tag = "") : base(size, coord, parent, name, tag)
        {
            
        }
        
        protected override void MakeRenderClip()
        {
            RenderUnit borderUnit = new RenderUnit(borderColor, borderChar);
            RenderUnit fillUnit = new RenderUnit(fillColor, fillChar);
            
            if(drawType == PanelDrawType.FillOnly || drawType == PanelDrawType.FillAndBorder)
            {
                CurrentClip.DrawRectangle(Vector2Int.Zero, Size.Y, Size.X, fillUnit);
            }

            if (drawType == PanelDrawType.BorderOnly || drawType == PanelDrawType.FillAndBorder)
            {
                CurrentClip.DrawBorder(Vector2Int.Zero, Size.Y, Size.X, borderUnit);
            }
            
        }
    }
}