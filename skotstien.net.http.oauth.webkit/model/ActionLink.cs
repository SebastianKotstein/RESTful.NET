using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class ActionLink
    {
        private string _href;
        private string _action;

        [JsonProperty("href")]
        public string Href
        {
            get
            {
                return _href;
            }

            set
            {
                _href = value;
            }
        }

        [JsonProperty("action")]
        public string Action
        {
            get
            {
                return _action;
            }

            set
            {
                _action = value;
            }
        }
    }
}
