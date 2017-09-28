using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using PerformanceLibrary.Core;
using System.Linq;
using System.IO;
using NPOI.Util;
using System.Collections.Concurrent;
namespace UtilsLibrary.Tools
{
    public class ExcelReader
    {
        private HSSFWorkbook hssfWorkbook = null;
        private Boolean allSheetsDone = false;
        private List<String> sheetNames = new List<string>();

        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> allSheetsMap = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        private OrderedDictionary orderedDictionary = new OrderedDictionary();

        public List<String> getSheetNames()
        {
            return sheetNames;
        }

        public ExcelReader(string fileName)
        {
            try
            {
                
                string sPath = Base.GetExecPath();               
                fileName = sPath + fileName;
                FileStream fs = new FileStream(fileName, FileMode.Open);
                hssfWorkbook = new HSSFWorkbook(fs);
                int numberOfSheets = hssfWorkbook.NumberOfSheets;
                for (int i = 0; i < numberOfSheets; i++)
                {
                    sheetNames.Add(hssfWorkbook.GetSheetName(i));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error when trying to open the Data source file: {0}",fileName);
                Console.WriteLine(ex.Message);
            }
        }

        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> convertAllSheetsToMap()
        {

            if (allSheetsDone)
            {
                return allSheetsMap;
            }
            else
            {
                foreach (string sheetName in sheetNames)
                {
                    if (allSheetsMap.ContainsKey(sheetName))
                    {
                        continue;
                    }
                    allSheetsMap.Add(sheetName, (convertSheetToMap((HSSFSheet)hssfWorkbook.GetSheet(sheetName))));
                }
                allSheetsDone = true;
            }
            return allSheetsMap;
        }

        private Dictionary<string, Dictionary<string, string>> convertSheetToMap(HSSFSheet sheet)
        {
            Dictionary<string, Dictionary<string, string>> sheetMap = new Dictionary<string, Dictionary<string, string>>();

            int rows = sheet.LastRowNum;
            int cols = sheet.GetRow(0).LastCellNum;

            HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
            List<string> headerRowNames = new List<string>();

            for (int i = 0; i < cols; i++)
            {
                headerRowNames.Add(headerRow.GetCell(i).StringCellValue);
            }

            for (int i = 1; i <= rows; i++)
            {
                Dictionary<string, string> rowMap = new Dictionary<string, string>();
                HSSFRow dataRow = (HSSFRow)sheet.GetRow(i);
                string key = dataRow.GetCell(0).StringCellValue;
                if (key == null || key.Trim().Length == 0)
                {
                    continue;
                }

                for (int j = 0; j < cols; j++)
                {
                    HSSFCell cell = (HSSFCell)dataRow.GetCell(j);
                    if (cell != null)
                    {
                        rowMap.Add(headerRowNames.ElementAt(j), cell.ToString());
                    }
                    else
                    {
                        rowMap.Add(headerRowNames.ElementAt(j), null);
                    }

                }
                sheetMap.Add(key, rowMap);
            }
            return sheetMap;
        }

        public Dictionary<string, Dictionary<string, string>> getSheetAsMap(string sheetName)
        {
            try
            {
                if (!sheetNames.Contains(sheetName))
                {

                    throw new RuntimeException(sheetName + " not found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            Dictionary<string, Dictionary<string, string>> sheetMap = null;
            {
                sheetMap = convertSheetToMap((HSSFSheet)hssfWorkbook.GetSheet(sheetName));
                allSheetsMap.Add(sheetName, sheetMap);
            }

            return sheetMap;
        }
    }
}
