using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class Clients
    {
        private IList<Client> _clients = new List<Client>();

        [JsonProperty("clients")]
        public IList<Client> ClientList
        {
            get
            {
                return _clients;
            }
            set
            {
                _clients = value;
            }
        }

        [JsonProperty("self")]
        public Link Self
        {
            get
            {
                return new Link() { Href = ApiBase.API_V1+"/clients" };
            }
        }

        [JsonProperty("createClient")]
        public ActionLink CreateClientLink
        {
            get
            {
                return new ActionLink() { Href = ApiBase.API_V1 + "/clients", Action = "POST" };
            }
        }
    }
}
