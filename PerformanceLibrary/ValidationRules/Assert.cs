﻿using LoggerFramework;
using LoggerFramework.Shared;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLibrary.Tools;

namespace PerformanceLibrary.ValidationRules
{
    
    public class Assert : ValidationRule
    {
        #region Annotations
        [Obsolete]
        public override string RuleName
        {
            get { return "Validate Response"; }
        }

        [Obsolete]
        public override string RuleDescription
        {
            get { return "Validates that response does not have a invalid success message"; }
        }
        #endregion

        public override void Validate(object sender, ValidationEventArgs e)
        {
            bool validated = false;

            // Make sure we have a response
            if (!System.String.IsNullOrWhiteSpace(e.Response.BodyString))
            {
                // Response could be HTML code for an Internal Server Error
                if (e.Response.BodyString.Contains("Error"))
                {
                    e.IsValid = validated;
                    e.WebTest.Outcome = Outcome.Fail;          
                    LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " ASSERT CLASS: Web Test Name - " + e.WebTest.Name + " - ERROR: Internal Server Error Page");
                    return;
                }
                
                // Get the response string, and parse into json
                string json = e.Response.BodyString;                
                var jsonExtract = new JsonRegex(json);
                List<string> r = jsonExtract.GetMultipleValue("success");
                if (r.Count == 0) {r= jsonExtract.GetMultipleValue("Success"); }
                foreach(string value in r)
                {                   
                    if (!value.StartsWith("true", StringComparison.InvariantCultureIgnoreCase)){
                        e.IsValid = false;
                        e.Message = "Message -> success: false found";
                        e.WebTest.Outcome = Outcome.Fail;
                        
                        LogHelper.Log(LogTarget.File, DateTime.Now.ToString()+" - ASSERT CLASS: Web Test Name - "+e.WebTest.Name
                            +" - Web Test Outcome - "+e.WebTest.Outcome +" - VALIDATION RULE:  JSON Message attribute 'Success': FALSE");
                        break;
                    }
                }
                e.IsValid = true;
                e.Message = "Message -> success: true found";
                e.WebTest.Context.Add("Validation Message:", e.Message);
            }            
        }
    }
}
