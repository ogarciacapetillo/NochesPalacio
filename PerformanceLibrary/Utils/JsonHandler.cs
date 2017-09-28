using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace UtilsLibrary.Tools
{
    public static class JsonHandler
    {
        private static Dictionary<string, object> dCTX = new Dictionary<string, object>();
        private static Dictionary<string, object> dictionary = new Dictionary<string, object>();

        
        public static void UpdateProperty(string json, string property, string subProperty, string newValue)
        {
            JObject code = JObject.Parse(json);
            var val = code[property];
            val[subProperty] = newValue;
            code.ToString();
        }
        //public static Dictionary<string, object> JsonToDictionary(string identifier,string jsonString)
        //{   
        //    try
        //    {               
        //        var jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();                
        //        dictionary = (Dictionary<string, object>)jsSerializer.DeserializeObject(jsonString);
        //        if (dictionary.TryGetValue(identifier, out object value))
        //        {
        //            dCTX = InnerLevelDicitionary(value);                   
        //        }                                   
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("The following exception has occurr....");
        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    return dCTX;            
        //}

        public static void UpdateKeyValue(string key, string newValue)
        {
            Dictionary<string, object> aux = new Dictionary<string, object>();
            aux = GetDictionary();
            if (aux.ContainsKey(key))
            {
                aux[key] = newValue;
            }
        }

        public static string JsonFromDictionary(Dictionary<string, object> content)
        {
            try
            {
                string sJson = JsonConvert.SerializeObject(content, Formatting.Indented);
                return sJson;
            }
            catch (Exception ex)
            {
                Console.WriteLine("The following exception has occurr....");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public static string JsonFromDictionary()
        {
            try
            {
                dictionary["ctx"] = GetDictionary();
                string sJson = JsonConvert.SerializeObject(GetParentDictionary());
                // Convert to Big decimal the ver field             
                sJson = String.Concat(@"", sJson);                                   
                return sJson;
            }
            catch (Exception ex)
            {
                Console.WriteLine("The following exception has occurr....");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return null;
        }

        private static Dictionary<string, object> GetDictionary()
        {
            return dCTX;
        }

        private static Dictionary<string, object> GetParentDictionary()
        {
            return dictionary;
        }


        private static Dictionary<string, object> InnerLevelDicitionary(object obj)
        {
            Dictionary<string, object> newDict = new Dictionary<string, object>();
            if (typeof(IDictionary).IsAssignableFrom(obj.GetType()))
            {
                IDictionary idict = (IDictionary)obj;

                foreach (object key in idict.Keys)
                {
                    newDict.Add(key.ToString(), idict[key]);
                }
                return newDict;
            }
            else
            {
                // My object is not a dictionary
                return newDict;
            }
        }

        #region Unused code section

        // Funciton to manipulate dictionary
        // $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        private static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        private static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }

        // Key Value Pair
        // $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        private static KeyValuePair<object, object> Cast<K, V>(this KeyValuePair<K, V> kvp)
        {
            return new KeyValuePair<object, object>(kvp.Key, kvp.Value);
        }

        private static KeyValuePair<T, V> CastFrom<T, V>(Object obj)
        {
            return (KeyValuePair<T, V>)obj;
        }

        private static KeyValuePair<object, object> CastFrom(Object obj)
        {
            var type = obj.GetType();
            if (type.IsGenericType)
            {
                if (type == typeof(KeyValuePair<,>))
                {
                    var key = type.GetProperty("Key");
                    var value = type.GetProperty("Value");
                    var keyObj = key.GetValue(obj, null);
                    var valueObj = value.GetValue(obj, null);
                    return new KeyValuePair<object, object>(keyObj, valueObj);
                }
            }
            throw new ArgumentException(" ### -> public static KeyValuePair<object , object > CastFrom(Object obj) : Error : obj argument must be KeyValuePair<,>");
        }

        #endregion


    }
}
