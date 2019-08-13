using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{ 
    public class Link
    {
        private string _href;


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
    }
}
