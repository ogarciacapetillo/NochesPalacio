using LoggerFramework.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerFramework
{
    public class DBLogger : LogBase
    {
        string connectionString = string.Empty;

        public override void Log(string message)

        {
            lock (lockObj)
            {
                //Code to log data to the database
            }


        }
    }
}
