using System;
using System.Collections.Generic;
using System.Threading;

namespace CUIEngine.Inputs
{
    public delegate void InputHandler(ConsoleKeyInfo info);

    public static class Input
    {
        //针对性处理方法
        static Dictionary<int, InputHandler?> handlersDic = new Dictionary<int, InputHandler?>();
        //任意键处理方法
        static InputHandler? anyKeyHandlers = null;
        
        static Thread getInputThread;

        static bool isRunning = true;

        static Input()
        {
            getInputThread = new Thread(() =>
            {
                while (isRunning)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    CallHandler(keyInfo);
                }
            });
            getInputThread.IsBackground = true;
        }
        
        public static void Initialize()
        {
            isRunning = true;
            getInputThread.Start();
        }

        public static void Shutdown()
        {
            isRunning = false;
            getInputThread.Join();
        }
        
        /// <summary>
        /// 添加对指定按键组合输入的处理方法
        /// </summary>
        public static void AttachHandler(InputHandler handler, ConsoleKey key, bool shift = false, bool ctrl = false, bool alt = false)
        {
            int keyInfoCode = GetKeyInfoHashCode(key, shift, ctrl, alt);
            
            if (handlersDic.ContainsKey(keyInfoCode))
            {
                handlersDic[keyInfoCode] += handler;
            }
            else
            {
                handlersDic.Add(keyInfoCode, handler);
            }
        }
        
        /// <summary>
        /// 添加对输入的处理方法
        /// </summary>
        /// <param name="handler"></param>
        public static void AttachHandler(InputHandler handler)
        {
            anyKeyHandlers += handler;
        }

        /// <summary>
        /// 移除输入处理方法
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="key"></param>
        /// <param name="shift"></param>
        /// <param name="ctrl"></param>
        /// <param name="alt"></param>
        public static void DetachHandler(InputHandler handler, ConsoleKey key, bool shift = false, bool ctrl = false, bool alt = false)
        {
            int keyInfoCode = GetKeyInfoHashCode(key, shift, ctrl, alt);

            if (handlersDic.ContainsKey(keyInfoCode))
            {
                handlersDic[keyInfoCode] -= handler;
            }
        }

        /// <summary>
        /// 移除输入处理方法
        /// </summary>
        /// <param name="handler"></param>
        public static void DetachHandler(InputHandler handler)
        {
            anyKeyHandlers -= handler;
        }

        static void CallHandler(ConsoleKeyInfo info)
        {
            int keyInfoCode = GetKeyInfoHashCode(info.Key, info.Modifiers);
            
            anyKeyHandlers?.Invoke(info);
            
            if(handlersDic.ContainsKey(keyInfoCode))
            {
                handlersDic[keyInfoCode]?.Invoke(info);
            }
        }

        /// <summary>
        /// 获取按键信息的哈希码
        /// </summary>
        /// <param name="key"></param>
        /// <param name="shift"></param>
        /// <param name="ctrl"></param>
        /// <param name="alt"></param>
        /// <returns></returns>
        static int GetKeyInfoHashCode(ConsoleKey key, bool shift = false, bool ctrl = false, bool alt = false)
        {
            ConsoleModifiers modifiers = (shift ? ConsoleModifiers.Shift : 0) | (ctrl ? ConsoleModifiers.Control : 0) | (alt ? ConsoleModifiers.Alt : 0);
            return GetKeyInfoHashCode(key, modifiers);
        }

        static int GetKeyInfoHashCode(ConsoleKey key, ConsoleModifiers modifiers)
        {
            return HashCode.Combine(key, modifiers);
        }
    }
}