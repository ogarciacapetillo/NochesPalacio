using LoggerFramework.Shared;
using PerformanceLibrary.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerFramework
{
    public class FileLogger : LogBase
    {
        public string filePath = Base.GetExecPath()+@"NochesPalacioLog.txt";

        public override void Log(string message)

        {
            lock (lockObj)
            {

                using (StreamWriter streamWriter = new StreamWriter(filePath,append:true))

                {

                    streamWriter.WriteLine(message);

                    streamWriter.Close();

                }
            }

        }
    }
}
