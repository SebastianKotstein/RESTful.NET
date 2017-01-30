using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize(object o)
        {
            string json = "";

            Type type = o.GetType();
            foreach(PropertyInfo pi in type.GetProperties())
            {
                
            }
            return json;
        }
    }
}
