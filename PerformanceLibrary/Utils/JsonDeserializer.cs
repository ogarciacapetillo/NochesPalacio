using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace UtilsLibrary.Tools
{
    internal static class JsonDeserializer
    {

        /// <summary>
        /// Given a type and some json data, this method deserializes the json data into an object matching the type input.
        /// </summary>
        /// <param name="type">The type to deserialize the data into.</param>
        /// <param name="data">The json data to deserialize.</param>
        /// <returns>An object of "type".</returns>
        public static object Deserialize(System.Type type, System.String data)
        {
            // Find the proper deserialize method
            MethodInfo deserializeMethod = typeof(JsonConvert).GetMethods()
                .Where(m => m.Name == "DeserializeObject" && m.IsGenericMethodDefinition)
                .Where(m =>
                {
                    ParameterInfo[] parameters = m.GetParameters();

                    return (parameters.Length == 1 && parameters[0].ParameterType == typeof(System.String));

                }).First();

            // Create a version of the method with our specific output type
            MethodInfo deserializeGeneric = deserializeMethod.MakeGenericMethod(type);

            // Invoke the deserialize method
            return deserializeGeneric.Invoke(null, new object[] { data });

        }
    }
}
