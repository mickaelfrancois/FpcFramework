using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpcFramework.Log
{
    public enum LogType { Debug, Information, Warning, Error, CriticalError }


    public interface ILogger
    {
        void Write(string sender, string message, LogType logType);
    }

}

