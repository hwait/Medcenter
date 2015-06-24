using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Medcenter.Service.Model.Messaging
{
    public class LogMessage
    {
        public DateTime Time { get; set; }
        public string Caller { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public IDictionary Data { get; set; }
        public int Type { get; set; }
        public LogMessage(string caller, Exception e)
        {
            Caller = caller;
            Message = e.Message;
            Source = e.Source;
            StackTrace = e.StackTrace;
            Data = e.Data;
            Type = 2;
            Time = DateTime.Now;
        }
        public LogMessage(string caller, Exception e, string message)
        {
            Caller = caller;
            Message = e.Message;
            Source = e.Source;
            StackTrace = message;
            Data = e.Data;
            Type = 2;
            Time = DateTime.Now;
        }
        public LogMessage(string caller, string message)
        {
            Caller = caller;
            Message = message;
            Type = 1;
            Time = DateTime.Now;
        }
        public LogMessage(string caller)
        {
            Caller = caller;
            Type = 0;
            Time = DateTime.Now;
        }
    }

    public static class Logger
    {
        private static bool IsLogging = true;
        public static void Log(string caller, Exception e)
        {
            var log = new LogMessage(caller, e);
            SaveLog(log);
        }
        public static void Log(string caller, Exception e, string message)
        {
            var log = new LogMessage(caller, e);
            SaveLog(log);
        }

        public static void Log(string caller, string message)
        {
            var log = new LogMessage(caller, message);
            SaveLog(log);
        }

        public static void Log(string caller)
        {
            var log = new LogMessage(caller);
            SaveLog(log);
        }

        private static void SaveLog(LogMessage logMessage)
        {
            if (IsLogging)
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"c:\logs\log.txt", true))
                {
                    file.WriteLine(JsonConvert.SerializeObject(logMessage));
                }
        }
    }
}
