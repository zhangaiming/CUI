using System;
using System.Collections.Generic;
using System.Threading;

namespace CUIEngine.Inputs
{
    public delegate void InputHandler(ConsoleKeyInfo info);

    public static class Input
    {
        //针对性处理方法
        static Dictionary<int, List<InputHandler>> handlersDic = new Dictionary<int, List<InputHandler>>();
        //任意键处理方法
        static List<InputHandler> anyKeyHandlers = new List<InputHandler>();
        
        static Thread? getInputThread;

        static bool isRunning = true;

        public static void Initialize()
        {
            isRunning = true;
            getInputThread = new Thread(() =>
            {
                while (isRunning)
                {
                    if(Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        CallHandler(keyInfo);
                    }
                }
            });
            getInputThread.Start();
        }

        public static void Shutdown()
        {
            isRunning = false;
            getInputThread?.Join();
        }
        
        /// <summary>
        /// 添加对指定按键组合输入的处理方法
        /// </summary>
        public static void AttachHandler(InputHandler handler, ConsoleKey key, bool shift = false, bool ctrl = false, bool alt = false)
        {
            int keyInfoCode = GetKeyInfoHashCode(key, shift, ctrl, alt);
            
            List<InputHandler> list;
            if (handlersDic.ContainsKey(keyInfoCode))
            {
                list = handlersDic[keyInfoCode];
            }
            else
            {
                list = new List<InputHandler>();
                handlersDic.Add(keyInfoCode, list);
            }

            if (!list.Contains(handler))
            {
                list.Add(handler);
            }
        }
        
        /// <summary>
        /// 添加对输入的处理方法
        /// </summary>
        /// <param name="handler"></param>
        public static void AttachHandler(InputHandler handler)
        {
            if (!anyKeyHandlers.Contains(handler))
            {
                anyKeyHandlers.Add(handler);
            }
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
                List<InputHandler> list = handlersDic[keyInfoCode];
                if (list == null)
                {
                    handlersDic.Remove(keyInfoCode);
                    return;
                }

                list.Remove(handler);
                if (list.Count == 0)
                {
                    handlersDic.Remove(keyInfoCode);
                }
            }
        }

        /// <summary>
        /// 移除输入处理方法
        /// </summary>
        /// <param name="handler"></param>
        public static void DetachHandler(InputHandler handler)
        {
            if (anyKeyHandlers.Contains(handler))
            {
                anyKeyHandlers.Remove(handler);
            }
        }

        static void CallHandler(ConsoleKeyInfo info)
        {
            int keyInfoCode = GetKeyInfoHashCode(info.Key, info.Modifiers);
            foreach (InputHandler anyKeyHandler in anyKeyHandlers)
            {
                anyKeyHandler?.Invoke(info);
            }
            if (handlersDic.ContainsKey(keyInfoCode))
            {
                List<InputHandler> list = handlersDic[keyInfoCode];
                foreach (InputHandler inputHandler in list)
                {
                    inputHandler.Invoke(info);
                }
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