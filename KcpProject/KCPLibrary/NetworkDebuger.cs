using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Network_Kcp
{
    public class NetworkDebuger
    {
        public static bool IsUnity;
        public static bool EnableLog;
        public static bool EnableTime = false;
        public static bool EnableSave = false;
        public static bool EnableStack = false;
        //public static string LogFileDir = Application.persistentDataPath + "/NetworkLog/";
        public static string LogFileDir = @"..\..\NetworkLog\";
        public static string LogFileName = "";
        public static string Prefix = "> ";
        public static StreamWriter LogFileWriter = null;

      


        public static void Log(object message)
        {
            if (!NetworkDebuger.EnableLog)
            {
                return;
            }

            string msg = GetLogTime() + message;

            string log = Prefix + msg;
            ShowLog(log, LogType.Log);
            LogToFile("[I]" + msg);
        }

        //public static void Log(object message, object context) {
        //    if (!NetworkDebuger.EnableLog) {
        //        return;
        //    }

        //    string msg = GetLogTime() + message;

        //    //Debug.Log(Prefix + msg, context);
        //    LogToFile("[I]" + msg);
        //}

        public static void LogError(object message)
        {
            string msg = GetLogTime() + message;

            string log = Prefix + msg;
            ShowLog(log, LogType.Error);
            LogToFile("[E]" + msg, true);
        }

        public static void LogException(object message)
        {
            string msg = GetLogTime() + message;

            string log = Prefix + msg;
            ShowLog(log, LogType.Exception);
            LogToFile("[Ex]" + msg, true);
        }

        public static void LogWarning(object message)
        {
            string msg = GetLogTime() + message;

            string log = Prefix + msg;
            ShowLog(log, LogType.Warning);
            LogToFile("[W]" + msg);
        }

        //public static void LogWarning(object message, Object context) {
        //    string msg = GetLogTime() + message;

        //    Debug.LogWarning(Prefix + msg, context);
        //    LogToFile("[W]" + msg);
        //}


        //----------------------------------------------------------------------

        public static void Log(string tag, string message)
        {
            if (!NetworkDebuger.EnableLog)
            {
                return;
            }

            message = GetLogText(tag, message);
            string log = Prefix + message;
            ShowLog(log, LogType.Log);
            LogToFile("[I]" + message);
        }

        public static void Log(string tag, string format, params object[] args)
        {
            if (!NetworkDebuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(tag, string.Format(format, args));
            string log = Prefix + message;
            ShowLog(log, LogType.Log);
            LogToFile("[I]" + message);
        }

        public static void LogError(string tag, string message)
        {
            message = GetLogText(tag, message);
            string log = Prefix + message;
            ShowLog(log, LogType.Error);
            LogToFile("[E]" + message, true);
        }

        public static void LogError(string tag, string format, params object[] args)
        {
            string message = GetLogText(tag, string.Format(format, args));
            string log = Prefix + message;
            ShowLog(log, LogType.Error);
            LogToFile("[E]" + message, true);
        }


        public static void LogWarning(string tag, string message)
        {
            message = GetLogText(tag, message);
            string log = Prefix + message;
            ShowLog(log, LogType.Warning);
            LogToFile("[W]" + message);
        }

        public static void LogWarning(string tag, string format, params object[] args)
        {
            string message = GetLogText(tag, string.Format(format, args));
            string log = Prefix + message;
            ShowLog(log, LogType.Warning);
            LogToFile("[W]" + message);
        }


        //----------------------------------------------------------------------
        private static string GetLogText(string tag, string message)
        {
            string str = "";
            if (NetworkDebuger.EnableTime)
            {
                DateTime now = DateTime.Now;
                str = now.ToString("HH:mm:ss.fff") + " ";
            }

            str = str + tag + "->" + message;
            return str;
        }

        private static string GetLogTime()
        {
            string str = "";
            if (NetworkDebuger.EnableTime)
            {
                DateTime now = DateTime.Now;
                str = now.ToString("HH:mm:ss.fff") + " ";
            }
            return str;
        }



        private static void LogToFile(string message, bool EnableStack = false)
        {
            if (!EnableSave)
            {
                if (LogFileWriter != null)
                {
                    LogFileWriter.Flush();
                    LogFileWriter.Close();
                    LogFileWriter = null;
                }
                return;
            }

            if (LogFileWriter == null)
            {
                DateTime now = DateTime.Now;
                LogFileName = now.GetDateTimeFormats('s')[0].ToString();//2005-11-05T14:06:25
                LogFileName = LogFileName.Replace("-", "_");
                LogFileName = LogFileName.Replace(":", "_");
                LogFileName = LogFileName.Replace(" ", "");
                LogFileName += ".log";

                string fullpath = LogFileDir + LogFileName;
                try
                {
                    if (!Directory.Exists(LogFileDir))
                    {
                        Directory.CreateDirectory(LogFileDir);
                    }

                    LogFileWriter = File.AppendText(fullpath);
                    LogFileWriter.AutoFlush = true;
                }
                catch (Exception e)
                {
                    LogFileWriter = null;
                    string log = "LogToCache() " + e.Message + e.StackTrace;
                    ShowLog(log, LogType.Exception);
                    return;
                }
            }

            if (LogFileWriter != null)
            {
                try
                {
                    LogFileWriter.WriteLine(message);
                    if (EnableStack || NetworkDebuger.EnableStack)
                    {
                        LogFileWriter.WriteLine(StackTraceUtility.ExtractStackTrace());
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
        private static void ShowLog(string info, LogType type)
        {
            if (IsUnity)
            {
                switch (type)
                {
                    case LogType.Log:
                        Debug.Log(info);
                        break;
                    case LogType.Warning:
                        Debug.LogWarning(info);
                        break;
                    case LogType.Error:
                        Debug.LogError(info);
                        break;
                    case LogType.Exception:
                        Debug.LogError("(Exception)" + info);
                        break;
                }
            }
            else
            {
                Console.WriteLine(info);
            }
        }
    }
}
