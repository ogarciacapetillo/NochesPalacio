using Microsoft.VisualStudio.TestTools.WebTesting;
using Microsoft.VisualStudio.TestTools.WebTesting.Rules;
using PerformanceLibrary.Core;
using PerformanceLibrary.Utils;
using PerformanceLibrary.ValidationRules;
using System;
using System.Collections.Generic;

namespace WebAndLoadTestProject.TestCases
{
    //[IncludeCodedWebTest("WebAndLoadTestProject.TestCases.GenerateGAMToken", "webandloadtestproject.dll")]
    public class Login : WebTest
    {
        PropertyLoader propertyLoader = new PropertyLoader();
        public Login()
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
                Assert assertResponse = new Assert();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(assertResponse.Validate);
            }
            #endregion

            #region Dependent WebTest

            if (Base.AccessToken.Count < 1)
            {
                foreach (WebTestRequest wtr in IncludeWebTest("GenerateGAMToken", false)) { yield return wtr; }
            }
            string access_token = "";
            if (Base.AccessToken.Count > 0) { access_token = Base.AccessToken[0]; }

            #endregion
            string tempo = String.Format("{{\"token\":\"{0}\"}}", access_token);

            string sBody = "{\"token\":\"" + access_token + "\",\"sdtLoginIn\":{\"email\":\"eromerog @ph.com.mx\",\"phone\":\"5540530743\"," +
                "\"pass\":\"xeHb / g / NOGBDCRAE1unMxg == \",\"userguid\":\"\",\"deviceid\":\"c188af9 - 4e68 - 32a3 - 9816 - 9bfe21a23d33\"," +
                "\"devicetype\":1,\"deviceosname\":\"Android\",\"deviceosversion\":\"5.0.2\"}}";

            #region WebTest

            this.BeginTransaction("PH_NP_Login");
            WebTestRequest webRequest = new WebTestRequest(this.Context["NetEnvironment"].ToString() + "/phApp/rest/Login");
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

            this.EndTransaction("PH_NP_Login");

            #endregion

            #region Validation Response

            #endregion


        }
    }
}
