using CUIEngine.Mathf;

namespace CUIEngine.Render
{
    public static class Drawer
    {
        /// <summary>
        /// 在渲染片段中画一个矩形框
        /// </summary>
        /// <param name="renderClip">目标渲染片段</param>
        /// <param name="origin">矩形左上方的顶点</param>
        /// <param name="height">高</param>
        /// <param name="width">宽</param>
        /// <param name="style">矩形的样式</param>
        public static void DrawBorder(RenderClip renderClip, Vector2Int origin, int height, int width, RenderUnit style)
        {
            for (int x = 0; x <= width; x++)
            {
                renderClip.PutUnit(origin.X + x, origin.Y, style);
                renderClip.PutUnit(origin.X + x, origin.Y + height - 1, style);
            }

            for (int y = 1; y <= height - 1; y++)
            {
                renderClip.PutUnit(origin.X, origin.Y + y, style);
                renderClip.PutUnit(origin.X + width - 1, origin.Y + y, style);
            }
        }

        /// <summary>
        /// 在渲染片段中画一个矩形
        /// </summary>
        /// <param name="renderClip">目标渲染片段</param>
        /// <param name="origin">矩形左上方的顶点</param>
        /// <param name="height">高</param>
        /// <param name="width">宽</param>
        /// <param name="style">矩形的样式</param>
        public static void DrawRectangle(RenderClip renderClip, Vector2Int origin, int height, int width, RenderUnit style)
        {
            for (int i = 0; i <= width; i++)
            {
                for (int j = 0; j <= height; j++)
                {
                    renderClip.PutUnit(origin.X + i, origin.Y + j, style);
                }
            }
        }

        /// <summary>
        /// 在渲染片段中画一条直线
        /// </summary>
        /// <param name="renderClip">目标渲染片段</param>
        /// <param name="origin">起点</param>
        /// <param name="length">线的长度</param>
        /// <param name="isHorizontal">若为true，则画水平线；若为false，则画竖直线</param>
        /// <param name="style">线的样式</param>
        public static void DrawLine(RenderClip renderClip, Vector2Int origin, int length, bool isHorizontal, RenderUnit style)
        {
            if (isHorizontal)
            {
                for (int i = 0; i < length; i++)
                {
                    renderClip.PutUnit(origin.X + i, origin.Y, style);
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    renderClip.PutUnit(origin.X, origin.Y + i, style);
                }
            }
        }

        /// <summary>
        /// 用指定的样式填充渲染片段
        /// </summary>
        /// <param name="renderClip">目标渲染片段</param>
        /// <param name="style">填充样式</param>
        public static void Fill(RenderClip renderClip, RenderUnit style)
        {
            DrawRectangle(renderClip, Vector2Int.Zero, renderClip.Size.Y, renderClip.Size.X, style);
        }
    }
}