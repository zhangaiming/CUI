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

        ColorPair borderColorPair = ColorPair.DefaultColorPair;
        ColorPair fillColorPair = ColorPair.DefaultColorPair;
        
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
        public ColorPair BorderColorPair
        {
            get => borderColorPair;
            set
            {
                if (borderColorPair != value)
                {
                    borderColorPair = value;
                    UpdateRenderClip();
                }
            }
        }

        /// <summary>
        /// 填充的颜色
        /// </summary>
        public ColorPair FillColorPair
        {
            get => fillColorPair;
            set
            {
                if (fillColorPair != value)
                {
                    fillColorPair = value;
                    UpdateRenderClip();
                }
            }
        }

        protected override void MakeRenderClip()
        {
            if (CurrentClip != null)
            {
                CurrentClip.Clear();
                
                RenderUnit borderUnit = new RenderUnit(borderColorPair, borderChar);
                RenderUnit fillUnit = new RenderUnit(fillColorPair, fillChar);
                for (int y = 0; y < Size.Y; y++)
                {
                    for (int x = 0; x < Size.X; x++)
                    {
                        if (x == 0 || x == Size.X - 1 || y == 0 || y == Size.Y - 1)
                        {
                            //绘制边框
                            if (drawType == PanelDrawType.BorderOnly || drawType == PanelDrawType.FillAndBorder)
                                CurrentClip.SetUnit(x, y, borderUnit);
                            else
                                CurrentClip.SetUnit(x, y, fillUnit);
                        }
                        else
                        {
                            //绘制填充
                            if(drawType == PanelDrawType.FillOnly || drawType == PanelDrawType.FillAndBorder)
                                CurrentClip.SetUnit(x, y, fillUnit);
                        }
                    }
                }
            }
        }
    }
}