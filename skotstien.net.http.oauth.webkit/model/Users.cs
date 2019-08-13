using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class Users
    {
        private IList<User> _users = new List<User>();
        private string _clientId;

        [JsonProperty("users")]
        public IList<User> UserList
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
            }
        }

        [JsonProperty("self")]
        public Link Self
        {
            get
            {
                return new Link() { Href = ApiBase.API_V1 + "/clients/"+ClientId+"/users" };
            }
        }

        [JsonProperty("createUser")]
        public ActionLink CreateClientLink
        {
            get
            {
                return new ActionLink() { Href = ApiBase.API_V1 + "/clients/" + ClientId + "/users", Action = "POST" };
            }
        }

        [JsonIgnore]
        public string ClientId
        {
            get
            {
                return _clientId;
            }

            set
            {
                _clientId = value;
            }
        }
    }
}
