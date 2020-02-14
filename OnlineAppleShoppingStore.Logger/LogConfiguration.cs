using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Logger
{
    public class LogConfiguration
    {
        public static string LogDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["LogDirectory"];
            }
        }
    }
}
