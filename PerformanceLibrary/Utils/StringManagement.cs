using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceLibrary.Utils
{
    public class StringManagement
    {
        public static string CleanUpChar(string cValue, string sentence)
        {
            sentence = sentence.Replace(cValue, "");
            return sentence;
        }
    }
}
