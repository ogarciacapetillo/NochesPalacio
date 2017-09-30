﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAndLoadTestProject.TestCases.Coded
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.WebTesting;
    using PerformanceLibrary.Utils;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;
    using PerformanceLibrary.ValidationRules;
    using PerformanceLibrary.Core;
    using PerformanceLibrary.ExtractionRules;
    using UtilsLibrary.Tools;

    public class autoGenerateGAMTokenCoded : WebTest
    {

        PropertyLoader propertyLoader = new PropertyLoader();
        public autoGenerateGAMTokenCoded()
        {
            this.PreAuthenticate = true;
            this.Proxy = "default";
        }
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            //Environment definition
            this.Context["NetEnvironment"] = propertyLoader.GetPropertyAUT("EnvironmentURL");

            #region ValidationRules

            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidateResponseUrl validationRule1 = new ValidateResponseUrl();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidationRuleResponseTimeGoal validationRule2 = new ValidationRuleResponseTimeGoal();
                validationRule2.Tolerance = 0D;
                this.ValidateResponseOnPageComplete += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }

            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                // Call method to validate HTTP response conde
                HTTPResponseCode validateResponseCode = new HTTPResponseCode();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validateResponseCode.Validate);
                Assert assertResponse = new Assert();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(assertResponse.Validate);
            }
            #endregion

            #region WebTest

            this.BeginTransaction("PH_NP_GenerateGAM");

            WebTestRequest webRequest = new WebTestRequest(this.Context["NetEnvironment"].ToString() + "/phApp/rest/RestApi/GetGAMToken");
            webRequest.ExpectedHttpStatusCode = 200;
            webRequest.Method = "POST";
            webRequest.Cache = false;
            //webRequest.ThinkTime = Base.GetThinkTime();
            webRequest.Headers.Add(new WebTestRequestHeader("Accept-Encoding", "gzip,deflate"));
            webRequest.Headers.Add(new WebTestRequestHeader("Content-type", "application/json"));
            webRequest.Headers.Add(new WebTestRequestHeader("Authorization", propertyLoader.GetPropertyAUT("OAuth")));
            StringHttpBody webRequestBody = new StringHttpBody { BodyString = "", ContentType = "application/json", InsertByteOrderMark = false };
            webRequest.Body = webRequestBody;

            // Extraction Rule 
            JsonTextExtractor extractionRule = new JsonTextExtractor
            {
                ContextParameterName = "Param_accessToken",
                Name = ""
            };
            webRequest.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule.Extract);

            yield return webRequest;
            webRequest = null;
            this.EndTransaction("PH_NP_GenerateGAM");

            #endregion

            #region ContextActions

            if (this.Context.ContainsKey("Param_accessToken"))
            {
                // Saving the list of GlobalModelId
                var jsonExtract = new JsonRegex(this.Context["Param_accessToken"].ToString());
                List<string> r = jsonExtract.GetMultipleValue("access_token");
                // Save the list of Access token to a memory list in the Core.Base class
                if (!(Base.AccessToken.Count > 0))
                {
                    Base.AccessToken = r;
                }
                r = null;
            }
            #endregion
        }
    }
}