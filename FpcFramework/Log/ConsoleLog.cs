using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpcFramework.Log
{
    public class ConsoleLog : ILogger
    {
        public ConsoleLog() { }


        public void Write(string sender, string message, LogType logType)
        {
            Trace.WriteLine(DateTime.Now.ToString() + " > " + logType.ToString() + " > " + sender + " > " + message);
        }
    }
}
