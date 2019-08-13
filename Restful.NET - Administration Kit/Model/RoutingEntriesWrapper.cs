using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Admin.Model
{
    public class RoutingEntriesWrapper
    {
        private IList<RoutingEntryWrapper> _routingEntries = new List<RoutingEntryWrapper>();

        [JsonProperty("count")]
        public int Count
        {
            get
            {
                return _routingEntries.Count;
            }
        }

        [JsonProperty("entries")]
        public IList<RoutingEntryWrapper> RoutingEntries
        {
            get
            {
                return _routingEntries;
            }

        }
    }
}
