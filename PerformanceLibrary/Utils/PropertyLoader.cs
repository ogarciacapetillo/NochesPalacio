using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceLibrary.Utils
{
    public class PropertyLoader
    {
        //Gets a reference to the assembly that containts the type 
        private Assembly myAssembly;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyLoader()
        {
            myAssembly = this.GetType().Assembly;
        }

        /// <summary>
        /// Return the value of the given property name
        /// </summary>
        /// <param name="key"></param>
        /// <returns>value of the property if exist</returns>
        public string GetPropertyAUT(String key)
        {

            ResourceManager resourceManagerAUT = new ResourceManager("PerformanceLibrary.Resources.Properties", myAssembly);

            String value = null;
            try
            {
                value = resourceManagerAUT.GetString(key);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Error while locating the property: " + key);
                Console.WriteLine(ex.Message);
                throw new Exception();
            }
            return value;
        }
    }
}
