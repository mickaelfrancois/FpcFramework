using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpcFramework.Log
{
    public class FileLog : ILogger
    {
        public string LogFile { get; set; }


        public FileLog() { }


        public FileLog(string logFile)
        {
            this.LogFile = LogFile;
        }

        public void Write(string sender, string message, LogType logType)
        {
            if (!string.IsNullOrEmpty(this.LogFile))
            {
                using (var stream = File.AppendText(this.LogFile))
                {
                    stream.WriteLine($"{DateTime.Now.ToString()};{logType.ToString()};{sender};{message}");
                }
            }
        }
    }
}
