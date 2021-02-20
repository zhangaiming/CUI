using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace Loggers
{
    public enum LogType
    {
        Normal,
        Warning,
        Error
    }
    
    public static class Logger
    {
        public static string LogFileDir = "";
        public static int FlushTimeSpanByMs = 10;
        static FileStream fs;
        static bool fileCreated = false;
        static string curFilePath = "";
        static Thread messageSolveThread;
        static bool shouldSolveMessage = true;

        static ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();

        public static void Initialize(string logFileDir)
        {
            LogFileDir = logFileDir;
            messageSolveThread = new Thread(() =>
            {
                while (shouldSolveMessage)
                {
                    if (!messageQueue.IsEmpty)
                    {
                        string str;
                        if (messageQueue.TryDequeue(out str))
                        {
                            Print(str);
                        }
                        Thread.Sleep(FlushTimeSpanByMs);
                    }
                }
            });
        }

        public static void Shutdown()
        {
            shouldSolveMessage = false;
            messageSolveThread.Join();
        }
        
        static void CreateLogFile()
        {
            if (LogFileDir != "")
            {
                curFilePath = LogFileDir + GetAName() + ".txt";
                Directory.CreateDirectory(LogFileDir);
                if (!File.Exists(curFilePath))
                {
                    File.Create(curFilePath).Close();
                }

                fileCreated = true;
            }
        }

        public static void Log(bool message, LogType logType = LogType.Normal)
        {
            Log(message ? "true" : "false", logType);
        }
        
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message, LogType logType = LogType.Normal)
        {
            messageQueue.Enqueue(FormatLogMessage(message, logType));
        }

        static void Print(string message)
        {
            if (!fileCreated)
            {
                CreateLogFile();
            }
            if (fileCreated)
            {
                using (fs = File.OpenWrite(curFilePath))
                {
                    fs.Position = fs.Length;
                    Encoding encoder = Encoding.UTF8;
                    byte[] messageBytes = encoder.GetBytes(message);
                    fs.Write(messageBytes, 0, messageBytes.Length);
                }
            }
        }

        /// <summary>
        /// 格式化日志信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        static string FormatLogMessage(string message, LogType logType)
        {
            StringBuilder prefix = new StringBuilder();
            StringBuilder postfix = new StringBuilder();
            switch (logType)
            {
                case LogType.Warning:
                    prefix.Append("[Warning]");
                    break;
                case LogType.Error:
                    prefix.AppendLine("[**-----Error-----**]");
                    break;
            }
            DateTime curTime = DateTime.Now;
            prefix.Append("[" + curTime.Hour + "/" + curTime.Minute + "/" + curTime.Second + "] ");
            postfix.Append("\n");

            switch (logType)
            {
                case LogType.Error:
                    prefix.AppendLine("[**---------------**]");
                    break;
            }

            prefix.Append(message).Append(postfix);
            return prefix.ToString();
        }

        static string GetAName()
        {
            DateTime time = DateTime.Now;
            return "[" + time.Year + "," + time.Month + "," + time.Day + "]" + time.Hour + "_" + time.Minute +
                   "_" + time.Second;
        }
    }
}