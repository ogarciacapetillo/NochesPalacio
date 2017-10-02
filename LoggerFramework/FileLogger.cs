using LoggerFramework.Shared;
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
        public string filePath = GetExecPath()+@"NochesPalacioLog.txt";

        public override void Log(string message)

        {
            lock (lockObj)
            {

                using (StreamWriter streamWriter = new StreamWriter(filePath,append:true))
                {
                    streamWriter.WriteLine(message);
                    //streamWriter.Close();
                }
            }

        }

        private static string GetExecPath()
        {
            string projectPath = "";
            try
            {
                string sPath = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
                string actualPath = sPath.Substring(0, sPath.LastIndexOf("TestResults"));
                projectPath = new Uri(actualPath).LocalPath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return projectPath;
        }
    }
}
