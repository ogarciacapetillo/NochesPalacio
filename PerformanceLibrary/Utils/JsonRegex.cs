// </copyright> 
// =============================================================================================================== 
// <summary> 
// JSON regex handler non-nested search and replacement
// </summary> 
// =============================================================================================================== 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UtilsLibrary.Tools
{
    public class JsonRegex
    {
        /// <summary>
        /// Pattern for direct fields
        /// </summary>
        private const string FieldPattern = @"\{.*""<fieldName>"":""?(?<val>[^\{\}""]+)""?.*\}";
        /// <summary>
        /// Pattern for objects with a key field
        /// </summary>
        private const string ObjectValuePattern = @"\{.*""<keyFieldName>"":""<keyFieldValue>""[^\{\}]+""<fieldName>"":""?(?<val>[^\{\}""]+)""?.*\}";

        /// <summary>
        /// Source JSON
        /// </summary>
        public string SourceJson { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="json">Source JSON</param>
        public JsonRegex(string json)
        {
            SourceJson = json;
        }

        /// <summary>
        /// Gets a direct field value
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>List of values with the corresponding field</returns>
        public List<string> GetFieldValue(string fieldName)
        {
            return GetValue(
                FieldPattern.Replace("<fieldName>", fieldName),
                "val");
        }

        /// <summary>
        /// Gest list of values of the given field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public List<string> GetMultipleValue(string fieldName)
        {
            return GetValues(fieldName);
        }

        /// <summary>
        /// Sets a direct field value
        /// </summary>
        /// <param name="fieldName">Target field name</param>
        /// <param name="value">Replacement value</param>
        /// <returns>JSON with the replaced value</returns>
        public string SetFieldValue(string fieldName, string value)
        {
            return SetValue(
                FieldPattern.Replace("<fieldName>", fieldName),
                "val",
                value);
        }

        /// <summary>
        /// Gets an field value, for an object with a valued key field
        /// </summary>
        /// <param name="keyFieldName">Key field name</param>
        /// <param name="keyFieldValue">Key field value</param>
        /// <param name="fieldName">Target field name</param>
        /// <returns>List of values with the corresponding field</returns>
        public List<string> GetFieldValueByKeyField(string keyFieldName, string keyFieldValue, string fieldName)
        {
            return GetValue(ObjectValuePattern
                    .Replace("<keyFieldName>", keyFieldName)
                    .Replace("<keyFieldValue>", keyFieldValue)
                    .Replace("<fieldName>", fieldName),
                    "val");
        }

        /// <summary>
        /// Sets an field value, for an object with a valued key field
        /// </summary>
        /// <param name="keyFieldName">Key field name</param>
        /// <param name="keyFieldValue">Key field value</param>
        /// <param name="fieldName">Target field name</param>
        /// <param name="value">Replacement value</param>
        /// <returns>JSON with the replaced value</returns>
        public string SetFieldValueByKeyField(string keyFieldName, string keyFieldValue, string fieldName, string value)
        {
            return SetValue(
                ObjectValuePattern
                    .Replace("<keyFieldName>", keyFieldName)
                    .Replace("<keyFieldValue>", keyFieldValue)
                    .Replace("<fieldName>", fieldName),
                "val",
                value);
        }

        /// <summary>
        /// Gets a value, using pattern search
        /// </summary>
        /// <param name="pattern">Pattern to evaluate</param>
        /// <param name="groupName">Value group name</param>
        /// <returns>List of values with the corresponding pattern</returns>
        private List<string> GetValue(string pattern, string groupName)
        {
            return Regex.Match(SourceJson, pattern, RegexOptions.Multiline)
                .Groups[groupName]
                .Captures
                .Cast<Capture>()
                .Select(c => c.Value)
                .ToList();
        }


        private List<string> GetValues(string pattern)
        {
            List<string> temp = new List<string>();
            try
            {
                // Splitting 
                string[] elements = SourceJson.Split(',');
                
                foreach (string record in elements)
                {
                    if (record.Contains(pattern))
                    {
                        string value = record.Split(':')[1];
                        temp.Add(CleanString(value));
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return temp;
           

        }

        private string CleanString(string sentence)
        {
            if (sentence != null && sentence.Length > 0)
            {
                StringBuilder sb = new StringBuilder(sentence.Length);
                foreach (char c in sentence)
                {
                    if (!((c.Equals('"')) || (c.Equals(' '))))
                    {
                        sb.Append(c);
                    }                    
                }
                sentence = sb.ToString().Trim();
            }
            return sentence;
        }

        /// <summary>
        /// Replaces a value, using pattern replacement
        /// </summary>
        /// <param name="pattern">Pattern to evaluate</param>
        /// <param name="groupName">Value group name</param>
        /// <param name="value">Value to replace</param>
        /// <returns>JSON with the replaced value</returns>
        private string SetValue(string pattern, string groupName, string value)
        {
            return Regex.Replace(
                SourceJson,
                pattern,
                m =>
                {
                    var capture = m.Value;
                    capture = capture.Remove(m.Groups[groupName].Index - m.Index, m.Groups[groupName].Length);
                    capture = capture.Insert(m.Groups[groupName].Index - m.Index, value);
                    return capture;
                },
                RegexOptions.Multiline
            );
        }
    }
}
