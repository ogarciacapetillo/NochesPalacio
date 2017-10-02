//using LoggerFramework;
//using LoggerFramework.Shared;
using LoggerFramework;
using LoggerFramework.Shared;
using Microsoft.VisualStudio.TestTools.WebTesting;
using PerformanceLibrary.Data;
using PerformanceLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAndLoadTestProject.TestCases.Shared
{
    /// <summary>
    /// Would load test data using an Excel Workbook as the source
    /// Read/Write operations
    /// </summary>
    [DeploymentItem(@".\Resources")]
    public class LoadTestData : WebTest
    {
        #region Fields
        private static ExcelReader loadDataFromExcel = null;
        private static PropertyLoader propertyLoader;
        private static int iTotalElements=0;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public LoadTestData()
        {
            this.PreAuthenticate = true;
            this.Proxy = "default";
            propertyLoader = new PropertyLoader();
        }

        #region Public Methods

        public static void LoadDataFromExcel()
        {
            try
            {                
                CleanUpStructures();
                loadDataFromExcel = new ExcelReader("TestData.xlsx");                
                int noRecords = FillingUpStructures();
                LogHelper.Log(LogTarget.File, String.Format("Total number of loaded records: {0}", noRecords));
            }catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, string.Format("Exception during loading data from Excel workbook: {0}",ex.Message));
            }
        }

        #endregion

        #region Private Methods
        private static void CleanUpStructures()
        {

        }

        private static int FillingUpStructures()
        {
            return Dataset.UpdateStructures(loadDataFromExcel);
        }
        #endregion

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            LoadDataFromExcel();
            WebTestRequest webTestRequest = new WebTestRequest("https://www.google.com/");
            webTestRequest.Cache = false;
            webTestRequest.ExpectedHttpStatusCode = 200;
            this.Context.Add("Number of records loaded", iTotalElements);            
            yield return webTestRequest;            
        }
     
    }
}
