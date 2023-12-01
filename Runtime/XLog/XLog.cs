using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Log system. Used to output logs with date and log type, support output to file, support custom output log type.
    /// ||日志系统。用于输出带日期和日志类型的日志，支持输出到文件，支持自定义输出的日志类型。
    /// </summary>
    public class XLog : MonoBehaviour
    {
        public const int ALL = 0;
        public const int WARNING = 1;
        public const int DEBUG = 2;
        public const int INFO = 3;
        public const int PROTO = 4;
        public const int VITAL = 5;
        public const int ERROR = 6;
        public const int EXCEPTION = 7;

        private const int MAX_ERROR_LOG = 20;

        public static bool isReportBug = false;
        public static bool isOutputLog = false;
        public static bool isUploadLog = false;
        public static bool isCloseOutLog = false;

        public static int errorCount = 0;
        public static int exceptCount = 0;
        public static int uploadTick = 20;
        public static int reportTick = 10;

        private static bool initFileSuccess = false;
        private static bool[] levelList = new bool[] { true, true, true, true, true, true, true, true };
        private static List<string> writeList = new List<string>();
        private static float uploadTime = 0;
        private static float reportTime = 0;

        private string outpath;
        private StreamWriter writer;
        private string[] temp;

        public int logCount = 0;
        public static List<string> errorList = new List<string>();
        private static object m_Lock = new object();

        private static XLog m_Instance;
        public static XLog Instance
        {
            get
            {
                // if (m_Instance == null)
                // {
                //     GameObject go = new GameObject("XLog");
                //     m_Instance = go.AddComponent<XLog>();
                //     DontDestroyOnLoad(go);
                // }
                return m_Instance;
            }
        }

        void Awake()
        {
            if (m_Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            m_Instance = this;
            InitLogFile();
            // Application.logMessageReceived += HandleLog;
            Application.logMessageReceivedThreaded += HandleLog;
        }

        void OnDestroy()
        {
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }
            // Application.logMessageReceived -= HandleLog;
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        void Update()
        {
            uploadTime += Time.deltaTime;
            reportTime += Time.deltaTime;
            lock (m_Lock)
            {
                if (writeList.Count > 0)
                {
                    logCount = writeList.Count;
                    if (!initFileSuccess)
                    {
                        writeList.Clear();
                        return;
                    }
                    try
                    {
                        temp = writeList.ToArray();
                        int count = 0;
                        foreach (var str in temp)
                        {
                            count++;
                            writer.WriteLine(str);
                            writeList.Remove(str);
                            if (count > 10) break;
                        }
                        writer.Flush();
                    }
                    catch (Exception e)
                    {
                        initFileSuccess = false;
                        //Application.logMessageReceived -= HandleLog;
                        Application.logMessageReceivedThreaded -= HandleLog;
                        UnityEngine.Debug.LogError("write outlog.txt error:" + e.Message);
                    }
                }
            }
        }

        private void InitLogFile()
        {
            ClearAllLog();
            XLog.EnableLog(ALL);
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                XLog.ClearAllLog();
                XLog.EnableLog(VITAL);
                XLog.EnableLog(ERROR);
                XLog.isReportBug = true;
                XLog.isUploadLog = true;
            }
            else
            {
                XLog.isUploadLog = false;
                XLog.isReportBug = false;
            }
            outpath = GetLogOutputPath();
            try
            {
                if (File.Exists(outpath))
                {
                    File.Delete(outpath);
                }
                writer = new StreamWriter(outpath, false, Encoding.UTF8);
                writer.WriteLine(GetNowTime() + "init file success!!");
                UnityEngine.Debug.Log(GetNowTime() + "init file success:" + outpath);
                writer.Flush();
                initFileSuccess = true;
            }
            catch (Exception e)
            {
                initFileSuccess = false;
                Application.logMessageReceived -= HandleLog;
                UnityEngine.Debug.LogError("write outlog.txt error:" + e.Message);
            }
        }

        private static string GetLogOutputPath()
        {
#if UNITY_EDITOR
            string path = Application.dataPath + "/../outlog.txt";
#else
            string path = Application.persistentDataPath + "/outlog.txt";
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                path = Application.persistentDataPath + "/outlog.txt";
            }
            else
            {
                path = Application.dataPath + "/../outlog.txt";
            }
#endif
            return path;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            lock (m_Lock)
            {
                if (!initFileSuccess) return;
                int index = logString.IndexOf("stack traceback");
                if (index > 0)
                {
                    string log = logString.Substring(0, index);
                    string trace = logString.Substring(index, logString.Length - index);
                    logString = log;
                    stackTrace = trace;
                }

                if (type == LogType.Log)
                {
                }
                else if (type == LogType.Error)
                {
                    if (logString.IndexOf("LUA ERROR") > 0 || logString.IndexOf("stack traceback") > 0) exceptCount++;
                    else errorCount++;

                    writeList.Add(logString);
                    //writeList.Add(stackTrace + "\n");

                    if (errorList.Count >= MAX_ERROR_LOG)
                    {
                        errorList.RemoveAt(1);
                    }

                    if (errorList.Count < MAX_ERROR_LOG)
                    {
                        errorList.Add(logString);
                       // errorList.Add(stackTrace + "\n");
                    }
                }
                else if (type == LogType.Exception)
                {
                    exceptCount++;

                    writeList.Add(logString);
                    writeList.Add(stackTrace + "\n");

                    if (errorList.Count >= MAX_ERROR_LOG)
                    {
                        errorList.RemoveAt(1);
                    }

                    if (errorList.Count < MAX_ERROR_LOG)
                    {
                        errorList.Add(logString);
                        errorList.Add(stackTrace + "\n");
                    }
                }
            }
        }

        public static void FlushLog()
        {
            var instance = XLog.Instance;
            if (instance != null && instance.writer != null)
            {
                for (int i = 0; i < writeList.Count; i++)
                {
                    instance.writer.WriteLine(writeList[i]);
                }
                instance.writer.Flush();
                writeList.Clear();
            }
        }

        public static void EnableLog(int logType)
        {
            if (logType < 0 || logType >= levelList.Length) return;
            levelList[logType] = true;
        }

        public static void ClearAllLog()
        {
            for (int i = 0; i < levelList.Length; i++)
            {
                levelList[i] = false;
            }
        }

        public static bool CanLog(int level)
        {
            if (level < 0 || level >= levelList.Length) return false;
            return levelList[level] || levelList[0];
        }

        public static void Log(string log)
        {
            Debug(log);
        }

        public static void LogError(string log)
        {
            Error(log);
        }

        public static void LogWarning(string log)
        {
            Warning(log);
        }

        public static void Debug(string log)
        {
            if (!CanLog(DEBUG)) return;
            UnityEngine.Debug.Log(GetNowTime() + "[DEBUG]\t" + log);
        }

        public static void Vital(string log)
        {
            if (!CanLog(INFO)) return;
            UnityEngine.Debug.Log(GetNowTime() + "[VITAL]\t" + log);
        }

        public static void Info(string log)
        {
            if (!CanLog(INFO)) return;
            UnityEngine.Debug.Log(GetNowTime() + "[INFO]\t" + log);
        }

        public static void Proto(string log)
        {
            if (!CanLog(PROTO)) return;
            UnityEngine.Debug.Log(GetNowTime() + "[PROTO]\t" + log);
        }

        public static void Warning(string log)
        {
            if (!CanLog(WARNING)) return;
            UnityEngine.Debug.LogWarning(GetNowTime() + "[WARN]\t" + log);
        }

        public static void Error(string log)
        {
            if (!CanLog(ERROR)) return;
            UnityEngine.Debug.LogError(GetNowTime() + "[ERROR]\t" + log);
        }

        public static string GetNowTime(string formatter = null)
        {
            DateTime now = DateTime.Now;
            if (formatter == null)
                return now.ToString("[HH:mm:ss fff]", DateTimeFormatInfo.InvariantInfo);
            else
                return now.ToString(formatter, DateTimeFormatInfo.InvariantInfo);
        }

        public static ulong GetTimestamp()
        {
            return (ulong)(DateTime.Now - new DateTime(190, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }
    }
}
