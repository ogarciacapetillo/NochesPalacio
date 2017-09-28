using PerformanceLibrary.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsLibrary.Tools
{
    public class FetchExcelData
    {
        private static PropertyLoader propertyLoader = new PropertyLoader();
        private ExcelReader excelReader = null;

        public FetchExcelData(string filePath)
        {
            excelReader = new ExcelReader(filePath);
        }

        public Dictionary<string,string> mapTestCaseWithData(string testName)
        {
            Dictionary<String, String> dataFeed = null;
            if (excelReader != null)
            {
                // Find the TestCase name match and return the map with data
                for (int i = 0; i < excelReader.getSheetNames().Count; i++)
                {
                    if (excelReader.getSheetNames().ElementAt(i).Equals(propertyLoader.GetPropertyAUT("TestDataSheet")))
                    {
                        Dictionary<string, Dictionary<string, string>> sheetMap = excelReader.getSheetAsMap(excelReader.getSheetNames().ElementAt(i));
                        if (sheetMap.ContainsKey(testName))
                        {
                            dataFeed = sheetMap[testName];
                            break;
                        }
                    }
                }
            }
            return dataFeed;
        }
    }
}
