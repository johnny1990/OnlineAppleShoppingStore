using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Logger
{
    public enum LogType
    {
        INFO = 1,
        WAR = 2,
        ERR = 3,
        DEBUG = 4
    }

    public class Log
    {
        public DateTime Date { get; set; }
        public LogType Type { get; set; }
        public string CustomMessage { get; set; }
        public string SourceMessage { get; set; }
        public string Path { get; set; }
    }
}
