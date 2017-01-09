using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using SKotstein.Net.Http.Routing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Service
{
    public abstract class HttpService
    {

        protected RoutingTable _routingTable;
        protected ServiceConfiguration _serviceConfiguration;


        public abstract void AddController(HttpController httpController, string processingGroup);

        public abstract void AddController(HttpController httpController);

        public abstract void Start(ServiceConfiguration serviceConfiguration);

        public abstract void Start();

        public abstract void Stop();

        internal RoutingTable RoutingTable
        {
            get
            {
                return _routingTable;
            }
        }

        public string Routes
        {
            get
            {
                return _routingTable.ToString();
            }
        }

        public ServiceConfiguration ServiceConfiguration
        {
            get
            {
                return _serviceConfiguration;
            }
        }


    }
}


