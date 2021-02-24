using CUIEngine.Mathf;

namespace CUIEngine.Render
{
    /// <summary>
    /// 渲染中的画布接口
    /// </summary>
    public interface ICanvas
    {
        Vector2Int Size { get; }
        /// <summary>
        /// 获取画布的渲染片段
        /// </summary>
        /// <returns></returns>
        RenderClip GetRenderClip();

        /// <summary>
        /// 更新渲染片段
        /// </summary>
        void UpdateRenderClip();
    }
}