using Newtonsoft.Json;
using SKotstein.Net.Http.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Admin.Model
{
    public class RoutingEntryWrapper
    {
        private RoutingEntry _nested;

        [JsonProperty("metrics")]
        public int Metrics
        {
            get
            {
                return _nested.Metrics;
            }
        }

        [JsonProperty("paths")]
        public string Path
        {
            get
            {
                return _nested.Path;
            }
        }

        [JsonProperty("id")]
        public string Identifier
        {
            get
            {
                return _nested.Identifier;
            }
        }

        [JsonProperty("controllerUuid")]
        public string HttpControllerUuid
        {
            get
            {
                return _nested.HttpController.Uuid;
            }
        }

        [JsonProperty("controllerName")]
        public string HttpControllerFriendlyName
        {
            get
            {
                return _nested.HttpController.FriendlyName;
            }
        }

        [JsonProperty("processingGroup")]
        public string ProcessingGroup
        {
            get
            {
                return _nested.ProcessingGroup;
            }
        }

        [JsonIgnore]
        internal RoutingEntry Nested
        {
            set
            {
                _nested = value;
            }
        }

    }
}
