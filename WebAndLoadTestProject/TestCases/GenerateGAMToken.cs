using Microsoft.VisualStudio.TestTools.WebTesting;
using Microsoft.VisualStudio.TestTools.WebTesting.Rules;
using PerformanceLibrary.Core;
using PerformanceLibrary.ExtractionRules;
using PerformanceLibrary.Utils;
using PerformanceLibrary.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UtilsLibrary.Tools;

namespace WebAndLoadTestProject.TestCases
{
    public class GenerateGAMToken : WebTest
    {
        PropertyLoader propertyLoader = new PropertyLoader();
        public GenerateGAMToken()
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
            webRequest.ThinkTime = Base.GetThinkTime();
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
