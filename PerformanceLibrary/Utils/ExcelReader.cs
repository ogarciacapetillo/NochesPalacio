using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceLibrary.Utils
{
    public class ExcelReader
    {
        private XSSFWorkbook xssfWorkbook = new XSSFWorkbook();
        private bool allSheetsProcessed = false;
        private List<string> lSheets = new List<string>();

        /// <summary>
        /// Dictionary to represent the Excel workbook
        /// 1st level Book , sheetNames
        /// 2nd level SheetName, Sheet
        /// 3rd level Key, Value
        /// </summary>
        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> sheetsDictionary = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> SheetsDictionary { get => sheetsDictionary; }
        public List<string> GetSheetNames { get => lSheets; set => lSheets = value; }

        public ExcelReader(String fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            try
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    xssfWorkbook = new XSSFWorkbook(fs);
                    int nos = xssfWorkbook.NumberOfSheets;
                    for (int x = 0; x < nos; x++)
                    {
                        lSheets.Add(xssfWorkbook.GetSheetName(x));
                    }
                }
                // Converts all sheets
                ConvertAllSheetsToDictionary();
                Dictionary<string, Dictionary<string, Dictionary<string, string>>> sheetsDictionary = SheetsDictionary;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> ConvertAllSheetsToDictionary()
        {
            if (allSheetsProcessed)
            {
                return sheetsDictionary;
            }
            else
            {
                foreach (var sheet in lSheets)
                {
                    if (sheetsDictionary.ContainsKey(sheet))
                    {
                        continue;
                    }
                    sheetsDictionary.Add(sheet, ConvertSheet((XSSFSheet)xssfWorkbook.GetSheet(sheet)));
                }
                allSheetsProcessed = true;
            }
            return sheetsDictionary;
        }

        public Dictionary<string, Dictionary<string, string>> GetSheetAsDictionary(string sheetName)
        {
            try
            {
                if (!lSheets.Contains(sheetName))
                {
                    throw new ExecutionEngineException(sheetName + " not found");
                }
            }
            catch (Exception)
            {
                return null;
            }
            Dictionary<string, Dictionary<string, string>> sheetDictionary = sheetsDictionary[sheetName];
            if (sheetDictionary == null)
            {
                sheetDictionary = ConvertSheet((XSSFSheet)xssfWorkbook.GetSheet(sheetName));
            }
            return sheetDictionary;
        }

        private Dictionary<string, Dictionary<string, string>> ConvertSheet(XSSFSheet xSheet)
        {
            Dictionary<string, Dictionary<string, string>> sheetDictionary = new Dictionary<string, Dictionary<string, string>>();

            int rows = xSheet.PhysicalNumberOfRows;
            int cols = xSheet.GetRow(0).LastCellNum;

            XSSFRow headerRow = (XSSFRow)xSheet.GetRow(0);
            List<string> headerRowName = new List<string>();

            for (int x = 0; x < cols; x++)
            {
                headerRowName.Add(headerRow.GetCell(x).StringCellValue);
            }

            for (int i = 1; i < rows; i++)
            {
                Dictionary<String, String> rowMap = new Dictionary<string, string>();
                try
                {
                    XSSFRow dataRow = (XSSFRow)xSheet.GetRow(i);
                    String key = dataRow.GetCell(0).StringCellValue;
                    if (key == null || key.Trim().Length == 0)
                    {
                        continue;
                    }
                    //System.out.println("TestCase = " + key);
                    for (int j = 0; j < cols; j++)
                    {
                        XSSFCell cell = (XSSFCell)dataRow.GetCell(j);
                        if (cell != null)
                        {
                            rowMap.Add(headerRowName[j], cell.ToString());
                        }
                        else
                        {
                            rowMap.Add(headerRowName[j], null);
                        }

                    }
                    // Verify that item key does not exist
                    if (!sheetDictionary.ContainsKey(key))
                    {
                        sheetDictionary.Add(key, rowMap);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace, ex.Message);
                }
            }
            return sheetDictionary;
        }
    }
}
