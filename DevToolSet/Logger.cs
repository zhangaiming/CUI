using System;
using System.IO;
using System.Text;

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
        static FileStream fs;
        static bool fileCreated = false;
        static string curFilePath = "";

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
            if (message)
            {
                Log("true", logType);
            }
            else
            {
                Log("false", logType);
            }
        }
        
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message, LogType logType = LogType.Normal)
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
                    byte[] messageBytes = encoder.GetBytes(FormatLogMessage(message, logType));
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