using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.jsonkit
{
    public interface ISchemaModifier
    {
        void Modify(JSchema schema);

    }
}
