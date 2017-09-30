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

    [IncludeCodedWebTest("WebAndLoadTestProject.TestCases.GenerateGAMToken", "webandloadtestproject.dll")]
    public class autoPagoTarjetaCoded : WebTest
    {
        PropertyLoader propertyLoader = new PropertyLoader();
        public autoPagoTarjetaCoded()
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

            #endregion

            string access_token = "";
            if (Base.AccessToken.Count > 0) { access_token = Base.AccessToken[0]; }

            string sBody = string.Format("{{\"token\":\"{0}\",\"sdtPagoIn\":{{\"DeviceId\":\"6c188af9 - 4e68 - 32a3 - 9816 - 9bfe21a23d33\"," +
                "\"devicetype\":1,\"UserGUID\":\"b6097a4d-acad-4035-96eb-dc4a2e9a7824\",\"cardID\":\"6520030238514058\",\"cardExpDate\":\"0419\"," +
                "\"cardCVV\":\"123\",\"cardEPHId\":\"6520030238514058\",\"amount\":1,\"cardHolder\":\"Eduardo Javier Epura\"}}}}", access_token);

            #region WebTest

            this.BeginTransaction("PH_NP_PagpTarjeta");
            WebTestRequest webRequest = new WebTestRequest(this.Context["NetEnvironment"].ToString() + "/phApp/rest/PagoTarjetaPalacio");
            webRequest.ExpectedHttpStatusCode = 200;
            webRequest.Method = "POST";
            webRequest.Cache = false;
            //webRequest.ThinkTime = Base.GetThinkTime();
            webRequest.Headers.Add(new WebTestRequestHeader("Accept-Encoding", "gzip,deflate"));
            webRequest.Headers.Add(new WebTestRequestHeader("Content-type", "application/json"));
            webRequest.Headers.Add(new WebTestRequestHeader("Authorization", propertyLoader.GetPropertyAUT("OAuth")));
            //Request Body JSON
            StringHttpBody webRequestLoginBody = new StringHttpBody { BodyString = sBody, ContentType = "application/json", InsertByteOrderMark = false };
            webRequest.Body = webRequestLoginBody;
            yield return webRequest;
            webRequest = null;
            this.EndTransaction("PH_NP_PagpTarjeta");

            #endregion

            #region Validation Response

            #endregion


        }
    }
}