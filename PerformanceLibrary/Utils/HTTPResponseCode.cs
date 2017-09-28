using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceLibrary.Utils
{
    public class HTTPResponseCode : ValidationRule
    {
        #region Private variables
        private string responseCode = "";
        private string[] invalidResonseStatus = { "BadRequest", "Unauthorized", "PaymentRequired", "Forbidden", "NotFound", "MethodNotAllowed", "NotAcceptable", "ProxyAuthenticationRequired",
            "RequestTimeout","Conflict","Gone","LengthRequired","PreconditionFailed","RequestEntityTooLarge","RequestUriTooLong","UnsupportedMediaType", "RequestedRangeNotSatisfiable",
            "ExpectationFailed", "UpgradeRequired", "InternalServerError","NotImplemented","BadGateway","ServiceUnavailable","GatewayTimeout","HttpVersionNotSupported" };

        #endregion
        public override void Validate(object sender, ValidationEventArgs e)
        {
            bool status = true;
            responseCode = e.Response.StatusCode.ToString();
            foreach(string keyword in invalidResonseStatus)
            {
                if (keyword.Equals(responseCode, StringComparison.OrdinalIgnoreCase))
                {
                    status = false;
                    break;
                }
            }
            if (!status)
            {
                e.IsValid = false;
                e.WebTest.Stop();
                e.WebTest.Outcome = Outcome.Fail;
            }
            
        }
    }
}
