namespace CUIEngine
{
    public class CUIClock
    {
        bool enabled = false;

        public void Initialize()
        {
            
        }
        
        /// <summary>
        /// 设置时钟的开启与关闭
        /// </summary>
        /// <param name="state"></param>
        public void SetState(bool state)
        {
            enabled = state;
        }

        void Circle()
        {
            InputUpdate();
            ScriptUpdate();
            RenderFrame();
            ResetInput();
        }

        /// <summary>
        /// 更新输入状态
        /// </summary>
        void InputUpdate()
        {
            
        }
        /// <summary>
        /// 重置输入状态
        /// </summary>
        void ResetInput()
        {
            
        }
        
        /// <summary>
        /// 调用各脚本的Update方法
        /// </summary>
        void ScriptUpdate()
        {
            
        }

        /// <summary>
        /// 开始渲染,并等待渲染完毕
        /// </summary>
        void RenderFrame()
        {
            
        }
    }
}