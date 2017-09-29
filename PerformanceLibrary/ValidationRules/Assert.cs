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
        public override string RuleName
        {
            get { return "Validate Response"; }
        }

        public override string RuleDescription
        {
            get { return "Validates that response does not have a invalid success message"; }
        }
        #endregion

        public string Name { get; set; }

        public string ExpectedMessage { get; set; }
      

        public override void Validate(object sender, ValidationEventArgs e)
        {
            bool validated = false;

            // Make sure we have a response
            if (!System.String.IsNullOrWhiteSpace(e.Response.BodyString))
            {
                // Response could be HTML code for an Internal Server Error
                foreach(HtmlTag tag in e.Response.HtmlDocument.GetFilteredHtmlTags("title"))
                {
                    if (tag.GetAttributeValue("Value").Equals("Error"))
                    {
                        e.IsValid = validated;
                        e.Message = (e.Response.HtmlDocument.GetFilteredHtmlTags("body")).ElementAt(0).ToString();
                        return;
                    }                   
                }
                // Get the response string, and parse into json
                string json = e.Response.BodyString;
                var jsonExtract = new JsonRegex(json);

                List<string> r = jsonExtract.GetMultipleValue("message");
            }            
        }
    }
}
