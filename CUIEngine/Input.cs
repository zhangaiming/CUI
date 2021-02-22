using System;

namespace CUIEngine
{
    public class Input
    {
        
        
        /// <summary>
        /// 返回按键key是否正被按住
        /// </summary>
        /// <param name="key"></param>
        public bool GetKey(KeyCode key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回按键key是否被按下
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetKeyDown(KeyCode key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回按键key是否弹起
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetKeyUp(KeyCode key)
        {
            throw new NotImplementedException();
        }

        internal void BeginReadInput()
        {
            throw new NotImplementedException();
        }

        internal void StopReadInput()
        {
            throw new NotImplementedException();
        }
    }
}