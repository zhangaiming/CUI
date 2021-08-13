using System.Drawing;
using System.Text;
using CUIEngine.Mathf;

namespace CUIEngine.Render
{
    /// <summary>
    /// 用于相同颜色连续渲染的单位
    /// </summary>
    public struct RenderString
    {
        Vector2Int origin;  //原点
        StringBuilder content; //内容
        ColorPair color;//颜色

        /// <summary>
        /// 渲染串的起始坐标
        /// </summary>
        public Vector2Int Origin
        {
            get => origin;
            set => origin = value;
        }
        
        /// <summary>
        /// 渲染串的内容
        /// </summary>
        public string Content
        {
            get => content.ToString();
            set => content = new StringBuilder(value);
        }

        /// <summary>
        /// 渲染串的颜色
        /// </summary>
        public ColorPair Color
        {
            get => color;
            set => color = value;
        }

        /// <summary>
        /// 使用一个渲染单元来初始化串，将会继承单元的颜色
        /// </summary>
        /// <param name="unit">渲染单元</param>
        /// <param name="ori">渲染串起始点</param>
        public RenderString(RenderUnit unit, Vector2Int ori)
        {
            content = new StringBuilder(unit.Content.ToString());
            origin = ori;
            color = unit.ColorPair;
        }

        /// <summary>
        /// 使用颜色对和字符串对渲染串进行初始化
        /// </summary>
        /// <param name="pair">颜色对</param>
        /// <param name="ori">渲染串起始点</param>
        /// <param name="con">渲染串内容</param>
        public RenderString(ColorPair pair, Vector2Int ori, string con = "")
        {
            color = pair;
            origin = ori;
            content = new StringBuilder(con);
        }
        
        /// <summary>
        /// 附加渲染单元的内容到渲染串中，将不会使用单元的颜色
        /// </summary>
        /// <param name="unit">渲染单元</param>
        public void Append(RenderUnit unit)
        {
            content.Append(unit.Content);
        }

        public void Clear()
        {
            color = ColorPair.DefaultColorPair;
            content.Clear();
        }
    }
}