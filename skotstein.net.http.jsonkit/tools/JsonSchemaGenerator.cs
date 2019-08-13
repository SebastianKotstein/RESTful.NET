using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.jsonkit
{
    public class JsonSchemaGenerator
    {
        public static JSchema Generate(Type type)
        {
            try
            {
                JSchemaGenerator jSchemaGenerator = new JSchemaGenerator();
                return jSchemaGenerator.Generate(type);
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}
