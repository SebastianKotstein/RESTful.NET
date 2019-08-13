using Newtonsoft.Json;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Admin.Model
{
    public class HttpControllerWrapper
    {
        private HttpController _nested;
        private IList<NestedRoutingEntry> _routing = new List<NestedRoutingEntry>();

        [JsonProperty("controllerUuid")]
        public string HttpControllerUuid
        {
            get
            {
                return _nested.Uuid;
            }
        }

        [JsonProperty("controllerName")]
        public string HttpControllerFriendlyName
        {
            get
            {
                return _nested.FriendlyName;
            }
        }

        [JsonIgnore]
        public HttpController Nested
        {
            get
            {
                return _nested;
            }

            set
            {
                _nested = value;
            }
        }

        [JsonProperty("count")]
        public int Count
        {
            get
            {
                return _routing.Count;
            }
        }

        [JsonProperty("routing")]
        public IList<NestedRoutingEntry> Routing
        {
            get
            {
                return _routing;
            }

            set
            {
                _routing = value;
            }
        }
    }

    public class NestedRoutingEntry
    {
        private string _id;
        private string _path;

        [JsonProperty("id")]
        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        [JsonProperty("path")]
        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
            }
        }
    }

    
}
