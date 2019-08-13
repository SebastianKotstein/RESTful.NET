using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using SKotstein.Net.Http.Manipulation;
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

        protected RoutingEngine _routingEngine;
        protected ServiceConfiguration _serviceConfiguration;

        /// <summary>
        /// Gets the Prefix of the underlying <see cref="HttpService"/> including the HTTP Schema, the Host and the Port.
        /// </summary>
        public abstract string Prefix { get; }


        /// <summary>
        /// Adds an <see cref="HttpController"/> object hosting REST functions to the specified processing group.
        /// </summary>
        /// <param name="httpController">HttpController object</param>
        /// <param name="processingGroup">processing group</param>
        public void AddController(HttpController httpController, string processingGroup)
        {
            AddController(httpController, processingGroup, false);
        }

        /// <summary>
        /// Adds an <see cref="HttpController"/> object hosting REST functions to the default processing group.
        /// </summary>
        /// <param name="httpController">HttpController object</param>
        public void AddController(HttpController httpController)
        {
            AddController(httpController, false);
        }

        /// <summary>
        /// Adds an <see cref="HttpController"/> object hosting REST functions to the specified processing group.
        /// </summary>
        /// <param name="httpController">HttpController object</param>
        /// <param name="processingGroup">processing group</param>
        /// <param name="multiprocessing">whether the HttpRequests should be executed similtanously (true) or in sequence</param>
        public abstract void AddController(HttpController httpController, string processingGroup, bool multiprocessing);

        /// <summary>
        /// Adds an <see cref="HttpController"/> object hosting REST functions to the default processing group.
        /// </summary>
        /// <param name="httpController">HttpController object</param>
        /// <param name="multiprocessing">whether the HttpRequests should be executed similtanously (true) or in sequence</param>
        public abstract void AddController(HttpController httpController, bool multiprocessing);


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

        public RoutingEngine RoutingEngine
        {
            get
            {
                return _routingEngine;
            }
        }

        /// <summary>
        /// Returns the underlying routes as string
        /// </summary>
        public string Routes
        {
            get
            {
                return _routingEngine.ToString();
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

        public abstract IDictionary<string,IHttpProcessor> HttpProcessors
        {
            get;
        }

        public abstract HttpManipulatorCollection<HttpContext> RoutingPreManipulation
        {
            get;
        }

        public abstract HttpManipulatorCollection<RoutedContext> GetProcessorPreManipulation(bool multiProcessor);

        public abstract HttpManipulatorCollection<RoutedContext> GetProcessorPreManipulation(string processingGroup);

        public abstract HttpManipulatorCollection<RoutedContext> GetProcessorPostManipulation(bool multiProcessor);

        public abstract HttpManipulatorCollection<RoutedContext> GetProcessorPostManipulation(string processingGroup);
    }
}


