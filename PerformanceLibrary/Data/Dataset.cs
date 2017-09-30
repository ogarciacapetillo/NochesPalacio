using PerformanceLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceLibrary.Data
{
    public static class Dataset
    {
        static int iTotalElements;
        #region Fields
        private static Dictionary<string, dynamic> Consulta { get; set; }
        private static Dictionary<string, dynamic> Pago { get; set; }
        private static Dictionary<string, dynamic> Movimiento { get; set; }
        private static Dictionary<string, dynamic> Login { get; set; }
        
        private static List<KeyValuePair<string, dynamic>> Pago2 { get; set; }
        #endregion

        
        public static int UpdateStructures(ExcelReader excelBook)
        {
            CleanUpMemory();
            Dictionary<string, dynamic> dictionary = new Dictionary<string, dynamic>();
            if (excelBook.SheetsDictionary.Count > 0)
            {
                foreach (string name in excelBook.GetSheetNames)
                {                    
                    // Return the value as key
                    Dictionary<string, Dictionary<string, string>> sheetDictionary = excelBook.GetSheetAsDictionary(name);
                    // based on the name of the sheet it would be defined what base list should be updated
                    foreach (var kvp in sheetDictionary)
                    {
                        string key = kvp.Key;
                        Dictionary<string, string> innerDictionary = kvp.Value;
                        foreach(var element in innerDictionary)
                        {
                            switch (name)
                            {
                                case "pago":
                                    Pago.Add(element.Key, element.Value);
                                    break;
                                case "movimiento":
                                    Movimiento.Add(element.Key, element.Value);
                                    break;
                                case "login":
                                    Login.Add(element.Key, element.Value);
                                    break;
                                case "consulta":
                                    Consulta.Add(element.Key, element.Value);
                                    break;
                                case "pago2":
                                    Pago2.Add(new KeyValuePair<string, dynamic>(element.Key, element.Value));
                                    break;
                            }
                            iTotalElements++;
                        }                   
                        
                    }

                }
            }
            return iTotalElements;
        }

        

        public static void CleanUpMemory()
        {
            Consulta = new Dictionary<string, dynamic>();
            Pago = new Dictionary<string, dynamic>();
            Movimiento = new Dictionary<string, dynamic>();
            Login = new Dictionary<string, dynamic>();
            Pago2 = new List<KeyValuePair<string, dynamic>>();
        }

        #region helpers

        private static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>( this IEnumerable<KeyValuePair<TKey, TValue>> list)
        {
            return list.ToDictionary(x => x.Key, x => x.Value);
        }

        private static Dictionary<string,dynamic> ConvertToDictionary(KeyValuePair<string,string> kvp)
        {
            return (new Dictionary<string, dynamic> { { kvp.Key, kvp.Value } });
        }
        #endregion

    }
}
