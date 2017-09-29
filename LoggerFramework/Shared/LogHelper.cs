using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerFramework.Shared
{
    public static class LogHelper
    {
        
        public static void Log(LogTarget target, string message)

        {
            dynamic logger;

            switch (target)

            {

                case LogTarget.File:

                    logger = new FileLogger();

                    logger.Log(message);

                    break;

                case LogTarget.Database:

                    logger = new DBLogger();

                    logger.Log(message);

                    break;

                case LogTarget.Eventlog:

                    logger = new EventLogger();

                    logger.Log(message);

                    break;

                default:

                    return;

            }
        }
        
    }
}
