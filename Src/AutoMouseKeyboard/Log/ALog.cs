using System;
using System.Diagnostics;
using System.Text;

namespace AMK
{
    public static class ALog
    {
        public static bool IsOutputConsole { get; set; } = true;
        public static bool IsAppendTime { get; set; } = true;

        public delegate void DebugCallback(string message);

        public static DebugCallback OnDebug;

        public static void Initialize()
        {
            OnDebug += (message) => { };
        }

        public static string Debug(string format, params object[] args)
        {
            var sb = new StringBuilder();
            if (IsAppendTime)
                sb.Append($"[{DateTime.Now.ToLongTimeString()}] ");

            sb.Append(string.Format(format, args));

            var log = sb.ToString();
            if (IsOutputConsole)
                Trace.WriteLine(log);

            OnDebug(log);
            return log;
        }

    }
}
