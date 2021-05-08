using AMK.Global;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AMK
{
    public static class ALog
    {
        public static bool IsOutputConsole { get; set; } = true;
        public static bool IsAppendTime { get; set; } = true;
        public static bool IsWriteFile { get; set; } = true;

        public delegate void DebugCallback(string message);

        public static DebugCallback OnDebug;

        private static string LogFileName { get; set; }
        private static BackgroundQueue TaskQueue = new BackgroundQueue();

        public static void Initialize()
        {
            //Initialize File
            if(IsWriteFile)
            {
                if (!Directory.Exists(AUtil.ToOSAbsolutePath(AConst.LogPath)))
                    Directory.CreateDirectory(AUtil.ToOSAbsolutePath(AConst.LogPath));
                ALog.LogFileName = Path.Combine(AUtil.ToOSAbsolutePath(AConst.LogPath), string.Format("AMK_{0}.log", DateTime.Now.ToString("yyyyMMdd")));
            }

            //Write to File 
            OnDebug += (message) => { 
                if(IsWriteFile)
                {
                    ALog.TaskQueue.QueueTask(() =>
                    {
                        string logMessage = message + Environment.NewLine;
                        File.AppendAllText(ALog.LogFileName, logMessage);
                    });
                }
            };
        }

        public static string Debug(string format, params object[] args)
        {
            string callingMethodName = new StackFrame(1, true).GetMethod().Name;

            var sb = new StringBuilder();
            if (IsAppendTime)
                sb.Append(string.Format("[{0}]", DateTime.Now.ToString("HH:mm:ss.ff")));

            sb.Append(callingMethodName);
            if(!string.IsNullOrEmpty(format))
                sb.Append("::" + string.Format(format, args));

            var log = sb.ToString();
            if (IsOutputConsole)
                Trace.WriteLine(log);

            OnDebug(log);
            return log;
        }
    }
}
