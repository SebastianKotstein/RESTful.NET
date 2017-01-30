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
    /// <summary>
    /// An implementation of the <see cref="HttpService"/> is the base class for hosting, handling and providing a RESTful service.
    /// </summary>
    public abstract class HttpService
    {

        protected RoutingTable _routingTable;
        protected ServiceConfiguration _serviceConfiguration;

        /// <summary>
        /// Adds an <see cref="HttpController"/> object hosting REST functions to the specified processing group.
        /// </summary>
        /// <param name="httpController">HttpController object</param>
        /// <param name="processingGroup">processing group</param>
        public abstract void AddController(HttpController httpController, string processingGroup);

        /// <summary>
        /// Adds an <see cref="HttpController"/> object hosting REST functions to the default processing group.
        /// </summary>
        /// <param name="httpController">HttpController object</param>
        public abstract void AddController(HttpController httpController);

        /// <summary>
        /// Starts the underlying RESTful service with the specific <see cref="SKotstein.Net.Http.Service.ServiceConfiguration"/>
        /// </summary>
        /// <param name="serviceConfiguration"></param>
        public abstract void Start(ServiceConfiguration serviceConfiguration);

        /// <summary>
        /// Starts the underlying RESTful service with the underlying <see cref="SKotstein.Net.Http.Service.ServiceConfiguration"/> (see <see cref="ServiceConfiguration"/>
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stops the underlying RESTful service
        /// </summary>
        public abstract void Stop();

        internal RoutingTable RoutingTable
        {
            get
            {
                return _routingTable;
            }
        }

        /// <summary>
        /// Returns the underlying routes as string
        /// </summary>
        public string Routes
        {
            get
            {
                return _routingTable.ToString();
            }
        }

        /// <summary>
        /// Returns the underlying service configruation
        /// </summary>
        public ServiceConfiguration ServiceConfiguration
        {
            get
            {
                return _serviceConfiguration;
            }
        }


    }
}


