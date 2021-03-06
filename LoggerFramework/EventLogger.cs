﻿using LoggerFramework.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerFramework
{
    public class EventLogger : LogBase
    {
        public override void Log(string message)
        {
            lock (lockObj)
            {
                EventLog eventLog = new EventLog("");

                eventLog.Source = "IDGEventLog";

                eventLog.WriteEntry(message);

            }
        }
    }
}
