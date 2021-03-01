using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using DevToolSet;

namespace CUIEngine.Inputs
{
    public delegate void InputHandler(ConsoleKeyInfo info);

    public static class Input
    {
        struct KeyInfo
        {
            public ConsoleKey Key;
            public ConsoleModifiers Modifiers;

            public KeyInfo(ConsoleKey key, ConsoleModifiers modifiers)
            {
                Key = key;
                Modifiers = modifiers;
            }
        }
        //针对性处理方法
        static Dictionary<KeyInfo, List<InputHandler>> handlersDic = new Dictionary<KeyInfo, List<InputHandler>>();
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
            ConsoleModifiers modifiers = (shift ? ConsoleModifiers.Shift : 0) | (ctrl ? ConsoleModifiers.Control : 0) |
                                         (alt ? ConsoleModifiers.Alt : 0);
            KeyInfo keyInfo = new KeyInfo(key, modifiers);
            
            List<InputHandler> list;
            if (handlersDic.ContainsKey(keyInfo))
            {
                list = handlersDic[keyInfo];
            }
            else
            {
                list = new List<InputHandler>();
                handlersDic.Add(keyInfo, list);
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
            ConsoleModifiers modifiers = (shift ? ConsoleModifiers.Shift : 0) | (ctrl ? ConsoleModifiers.Control : 0) |
                                         (alt ? ConsoleModifiers.Alt : 0);
            KeyInfo keyInfo = new KeyInfo(key, modifiers);
            
            if (handlersDic.ContainsKey(keyInfo))
            {
                List<InputHandler> list = handlersDic[keyInfo];
                if (list == null)
                {
                    handlersDic.Remove(keyInfo);
                    return;
                }

                list.Remove(handler);
                if (list.Count == 0)
                {
                    handlersDic.Remove(keyInfo);
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
            Stopwatch totalSw = Stopwatch.StartNew();
            KeyInfo keyInfo = new KeyInfo(info.Key, info.Modifiers); 
            foreach (InputHandler anyKeyHandler in anyKeyHandlers)
            {
                anyKeyHandler?.Invoke(info);
            }
            if (handlersDic.ContainsKey(keyInfo))
            {
                Stopwatch localSw = new Stopwatch();
                List<InputHandler> list = handlersDic[keyInfo];
                foreach (InputHandler inputHandler in list)
                {
                    localSw.Start();
                    inputHandler.Invoke(info);
                    localSw.Stop();
                    Logger.Log(string.Format("调用输入处理方法{1},所用时间:{0}.", localSw.Elapsed, inputHandler));
                }
            }
            totalSw.Stop();
            Logger.Log(string.Format("调用所有输入处理方法所用的总时间为:{0}.", totalSw.Elapsed));
        }
    }
}