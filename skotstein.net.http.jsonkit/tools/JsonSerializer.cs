using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.jsonkit
{
    public class JsonSerializer
    {
        /// <summary>
        /// Converts a JSON structure into an object.
        /// Returns null, if a exception while converting occurs.
        /// </summary>
        /// <typeparam name="T">type of the class</typeparam>
        /// <param name="json">JSON structure</param>
        /// <returns>object</returns>
        public static T DeserializeJson<T>(string json) where T : class
        {
            try
            {
                //throws an exception if JSON is null!
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts an object into a JSON structure.
        /// Returns null, if a exception while converting occurs or the passed object is null.
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>JSON structure</returns>
        public static string SerializeJson(object o)
        {
            try
            {
                //returns null, if o is null
                return JsonConvert.SerializeObject(o);
            }
            catch (Exception e)
            {
                return null;
            }



        }

    }
}
