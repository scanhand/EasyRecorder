using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMK
{
    public static class ALog
    {
        public static bool IsOutputConsole { get; set; } = true;
        public static bool IsAppendTime { get; set; } = true;

        public delegate void DebugCallback(string message);

        public static DebugCallback OnDebug;

        static void Initialize()
        {
            OnDebug += (message)=> { };
        }

        public static string Debug(string format, params object[] args)
        {
            var sb = new StringBuilder();
            sb.Append($"[{DateTime.Now.ToLongTimeString()}] ");
            if(IsAppendTime)
                sb.Append($"{string.Format(format, args)}");

            var log = sb.ToString();
            if(IsOutputConsole) 
                Trace.WriteLine(log);

            OnDebug(log);
            return log;
        }

    }
}
