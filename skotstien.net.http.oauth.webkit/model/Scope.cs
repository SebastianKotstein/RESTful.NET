using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class Scope
    {
        private string _scopeName;

        [JsonProperty("name")]
        public string ScopeName
        {
            get
            {
                return _scopeName;
            }

            set
            {
                _scopeName = value;
            }
        }
    }
}
