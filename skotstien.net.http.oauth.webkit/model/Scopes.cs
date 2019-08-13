using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class Scopes
    {
        private IList<Scope> _scopes = new List<Scope>();

        [JsonProperty("scopes")]
        public IList<Scope> ScopeList
        {
            get
            {
                return _scopes;
            }

            set
            {
                _scopes = value;
            }
        }

        [JsonProperty("self")]
        public Link Self
        {
            get
            {
                return new Link() { Href = ApiBase.API_V1 + "/scopes" };
            }
        }
    }
}
