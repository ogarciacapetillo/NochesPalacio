using Microsoft.VisualStudio.TestTools.WebTesting;
using Microsoft.VisualStudio.TestTools.WebTesting.Rules;
using PerformanceLibrary.Core;
using PerformanceLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAndLoadTestProject.TestCases
{
    [IncludeCodedWebTest("WebAndLoadTestProject.TestCases.GenerateGAMToken","webandloadtestproject.dll")]
    public class ConsultarBalance : WebTest
    {
        PropertyLoader propertyLoader = new PropertyLoader();
        public ConsultarBalance()
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
                HTTPResponseCode validateResponseCode = new HTTPResponseCode();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validateResponseCode.Validate);
            }
            #endregion

            #region Dependent WebTest

            if (Base.AccessToken.Count < 1)
            {
                foreach (WebTestRequest wtr in IncludeWebTest("GenerateGAMToken", false)) { yield return wtr; }
            }

            #endregion
            
            string sBody = string.Format("{\"TarjetaNumero\":\"6520030238514058\",\"access_token\":\"{0}\"}", Base.AccessToken);

            #region WebTest

            this.BeginTransaction("PH_NP_ConsultarBalance");
            WebTestRequest webRequest = new WebTestRequest(this.Context["EnvironmentURL"].ToString() + "/phApp/rest/ConsultarBalance");
            webRequest.ExpectedHttpStatusCode = 200;            
            webRequest.Method = "POST";
            webRequest.Cache = false;
            webRequest.ThinkTime = Base.GetThinkTime();
            webRequest.Headers.Add(new WebTestRequestHeader("Accept-Encoding", "gzip,deflate"));
            webRequest.Headers.Add(new WebTestRequestHeader("Content-type", "application/json"));
            webRequest.Headers.Add(new WebTestRequestHeader("Authorization", propertyLoader.GetPropertyAUT("OAuth")));
            //Request Body JSON
            StringHttpBody webRequestLoginBody = new StringHttpBody { BodyString = sBody, ContentType = "application/json", InsertByteOrderMark = false };
            webRequest.Body = webRequestLoginBody;
            yield return webRequest;
            webRequest = null;
            this.EndTransaction("PH_NP_ConsultarBalance");

            #endregion

            #region Validation Response

            #endregion


        }
    }
}
