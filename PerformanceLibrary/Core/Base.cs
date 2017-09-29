using PerformanceLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLibrary.Tools;

namespace PerformanceLibrary.Core
{
    public class Base
    {
        #region Private Fields
        private static PropertyLoader propertyLoader = new PropertyLoader();

        private static List<string> accessToken = new List<string>();

        #endregion

        #region Properties
        public static List<string> AccessToken { get => accessToken; set => accessToken = value; }

        #endregion

        #region Public methods

        /// <summary>
        /// Decode a base64 string given. Useful to code values e.g. passwords
        /// </summary>
        /// <param name="codedString"></param>
        /// <returns>decoded string value</returns>
        public static string DecodeString(string codedString)
        {
            byte[] data = Convert.FromBase64String(codedString);
            string res = Encoding.UTF8.GetString(data);
            return res;
        }

        /// <summary>
        /// Obtain the execution Path
        /// </summary>
        /// <returns>Return a string that represent the build path </returns>
        public static string GetExecPath()
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

        /// <summary>
        /// Bind a test case with data using the test case context information to obtaing test case name and obtain the test case name from an excel file 
        /// given as data for the test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="testName"></param>
        /// <returns></returns>
        public static Dictionary<string, string> BindTestCaseData(Dictionary<string, string> testData, string testName)
        {
            FetchExcelData fetchExcelData = new FetchExcelData(propertyLoader.GetPropertyAUT("TestDataFile"));
            testData = fetchExcelData.mapTestCaseWithData(testName);
            return testData;
        }
        
        /// <summary>
        /// Returns a random number from 5 to the number found in the property
        /// </summary>
        /// <returns></returns>
        public static int GetThinkTime()
        {
            int value = 0;
            try
            {
                if (propertyLoader.GetPropertyAUT("ThinkTime").Length > 0)
                {
                    // Random generation
                    value = Convert.ToInt16(propertyLoader.GetPropertyAUT("ThinkTime"));
                    Random random = new Random();
                    value = random.Next(5, value);
                    if (value > 3)
                    {
                        return value;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0}: {1}", "Error during the conversion from string to integer", ex.Message));
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
            }
            return 3;
        }

        /// <summary>
        /// Return a random element selection of a given list
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Element of the list</returns>
        public static string GetRandomValue(List<string> list)
        {
            Random rnd = new Random();
            return (list[rnd.Next(list.Count - 1)]);
        }
        #endregion
    }
}
