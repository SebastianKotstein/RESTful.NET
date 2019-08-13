using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKotstein.Net.Http.Core;
using System.Collections.Concurrent;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Routing;
using SKotstein.Net.Http.Adapters.HTTP.SYS;
using System.Reflection;
using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Manipulation;

namespace SKotstein.Net.Http.Service
{
    /// <summary>
    /// <see cref="HttpService"/> implementation basing on the Windows HTTP stack (HTTP.sys)
    /// </summary>
    public class DefaultHttpSysService : HttpService
    {
        #region Namespace
        public const string DEFAULT_PROCESSING_GROUP_SIMPLE = "DEFAULT_PG_SIMPLE";
        public const string DEFAULT_PROCESSING_GROUP_MULTI = "DEFAULT_PG_MULTI";
        public const string INTERNAL_PROCESSING_GROUP = "INTERNAL_PG";
        #endregion

        #region Pipes & Filters
        private BlockingCollection<HttpContext> _inputPipe;
        private BlockingCollection<HttpContext> _outputPipe;

        private HttpInboundAdapter _httpInboundAdapter;
        private HttpOutboundAdapter _httpOutboundAdapter;
        private HttpRouter _httpRouter;
        private IDictionary<string, IHttpProcessor> _httpProcessors;
        #endregion

        /// <summary>
        /// Creates a DefaultHttpSysService instance with the specified host and port.
        /// </summary>
        /// <param name="isSecured">NOT_SUPPORTED_YET</param>
        /// <param name="host">host</param>
        /// <param name="port">port</param>
        public DefaultHttpSysService(bool isSecured, string host, int port)
        {
            _inputPipe = new BlockingCollection<HttpContext>();
            _outputPipe = new BlockingCollection<HttpContext>();

            _httpInboundAdapter = new HttpSysInboundAdapter();
            _httpOutboundAdapter = new HttpSysOutboundAdapter();
            _httpRouter = new HttpRouter(this);
            _httpProcessors = new Dictionary<string, IHttpProcessor>();

            //set names
            _httpInboundAdapter.Name = "TxHTTP_Inbound_Adapter";
            _httpOutboundAdapter.Name = "TxHTTP_Outbound_Adapter";
            _httpRouter.Name = "TxHTTP_Router";

            //HttpInboundAdapter ----<TaggedContext>----> HttpRouter
            _httpInboundAdapter.OutputPipe = _inputPipe;
            _httpRouter.InputPipe = _inputPipe;

            //---<HttpContext>---> HttpOutboundAdapter
            _httpOutboundAdapter.InputPipe = _outputPipe;

            _routingEngine = new RoutingEngine();
            AddController(new HttpInternalController(), INTERNAL_PROCESSING_GROUP);

            //create ServiceConfiguration
            _serviceConfiguration = new ServiceConfiguration();
            _serviceConfiguration.IsSecured = isSecured;
            _serviceConfiguration.Host = host;
            _serviceConfiguration.Port = port;
        }

        /// <summary>
        /// Creates a DefaultHttpSysService instance with the specified service configuration.
        /// </summary>
        /// <param name="serviceConfiguration"></param>
        public DefaultHttpSysService(ServiceConfiguration serviceConfiguration) : this(false,"",0)
        {
            //overwrite service configuration
            _serviceConfiguration = serviceConfiguration;
        }

        public override void AddController(HttpController httpController, bool multiprocessing)
        {
            if (multiprocessing)
            {
                AddController(httpController, DEFAULT_PROCESSING_GROUP_MULTI, multiprocessing);
            }
            else
            {
                AddController(httpController, DEFAULT_PROCESSING_GROUP_SIMPLE, multiprocessing);
            }
            
        }

        public override void AddController(HttpController httpController, string processingGroup, bool multiprocessing)
        {
            if (!_httpProcessors.ContainsKey(processingGroup))
            {
                AddProcessor(processingGroup, multiprocessing);
            }
            else
            {
                //if the existing processing group has not the preferred type
                if(_httpProcessors[processingGroup].IsMultiProcessor != multiprocessing)
                {
                    throw new Exception("Cannot add controller since preferred processing group is multiprocessing="+!multiprocessing);
                }
            }
            //set service reference
            httpController.Service = this;
            //set provisional UUID if not set
            if(httpController.Uuid == null)
            {
                httpController.Uuid = httpController.GetType().GUID.ToString() + "_AUTO";
            }
            RegisterMethods(processingGroup, httpController);
            
        }

        public override void Start()
        {
            if(_serviceConfiguration == null)
            {
                _serviceConfiguration = new DefaultServiceConfiguration();
            }
            _httpInboundAdapter.Host = _serviceConfiguration.Host;
            _httpInboundAdapter.Port = _serviceConfiguration.Port;
            _httpInboundAdapter.Schema = _serviceConfiguration.IsSecured ? HttpInboundAdapter.SCHEMA_HTTPS : HttpInboundAdapter.SCHEMA_HTTP;

            _httpOutboundAdapter.Start();
            foreach(IHttpProcessor p in _httpProcessors.Values)
            {
                p.StartProcessor();
            }
            _httpRouter.Start();
            _httpInboundAdapter.Start();

        }

        public override void Stop()
        {
            _httpInboundAdapter.Stop();
            _httpRouter.Stop();
            foreach (IHttpProcessor p in _httpProcessors.Values)
            {
                p.StopProcessor();
            }
          
            _httpOutboundAdapter.Stop();
        }

        public override void Start(ServiceConfiguration serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
            Start();
        }

        private void AddProcessor(string processingGroup, bool multiprocessing)
        {
            BlockingCollection<RoutedContext> inputPipe = new BlockingCollection<RoutedContext>();

            if (!multiprocessing)
            {
                HttpSimpleProcessor processor = new HttpSimpleProcessor(this);
                processor.ProcessingGroupName = processingGroup;
                processor.Name = "Tx_"+processingGroup;

                //HttpRouter ---<RoutedContext>---> HttpProcessor
                processor.InputPipe = inputPipe;

                //HttpProcessor ---<HttpContext>---> HttpOutboundAdapter
                processor.OutputPipe = _httpOutboundAdapter.InputPipe;

                _httpProcessors.Add(processingGroup, processor);
            }
            else
            {
                HttpLimitedMultiProcessor processor = new HttpLimitedMultiProcessor(this);
                processor.ProcessingGroupName = processingGroup;

                //HttpRouter ---<RoutedContext>---> HttpProcessor
                processor.InputPipe = inputPipe;

                //HttpProcessor ---<HttpContext>---> HttpOutboundAdapter
                processor.OutputPipe = _httpOutboundAdapter.InputPipe;

                _httpProcessors.Add(processingGroup, processor);
            }

            _httpRouter.AddOutputPipe(processingGroup, inputPipe);

        }

        private void RegisterMethods(string processingGroup, HttpController httpController)
        {
            Type type = httpController.GetType();

            //check all methods of the HttpController class
            foreach (MethodInfo methodInfo in type.GetMethods())
            {
                string path;

                //load all attributes
                System.Attribute[] attr = System.Attribute.GetCustomAttributes(methodInfo);
                foreach (Attribute a in attr)
                {
                    if (a is Path)
                    {
                        Path pathAttribute = (Path)a;
                        path = pathAttribute.Method.ToString() + pathAttribute.Url;

                        if (!pathAttribute.Url.StartsWith("/"))
                        {
                            throw new Exception("Cannot include method since first parameter does not match target type. Make sure that the first parameter is type of HttpContext.");
                        }

                        if (methodInfo.GetParameters()[0].ParameterType.Name.CompareTo("HttpContext") != 0)
                        {
                            throw new Exception("Cannot include method since first parameter does not match target type. Make sure that the first parameter is type of HttpContext.");
                        }
                        _routingEngine.AddEntry(pathAttribute.Method.ToString() + pathAttribute.Url, processingGroup, httpController, methodInfo);
                    }

                }
            }
        }
        public override HttpManipulatorCollection<HttpContext> RoutingPreManipulation
        {
            get
            {
                return _httpRouter.Manipulators;
            }
        }

        public override IDictionary<string, IHttpProcessor> HttpProcessors
        {
            get
            {
                return _httpProcessors;
            }
        }

        public override string Prefix
        {
            get
            {
                return _httpInboundAdapter.Prefix;
            }
        }

        public override HttpManipulatorCollection<RoutedContext> GetProcessorPreManipulation(bool multiProcessor)
        {
            if (multiProcessor)
            {
                return _httpProcessors[DEFAULT_PROCESSING_GROUP_MULTI].PreManipulators;
            }
            else
            {
                return _httpProcessors[DEFAULT_PROCESSING_GROUP_SIMPLE].PreManipulators;
            }
        }

        public override HttpManipulatorCollection<RoutedContext> GetProcessorPreManipulation(string processingGroup)
        {
            return _httpProcessors[processingGroup].PreManipulators;
        }

        public override HttpManipulatorCollection<RoutedContext> GetProcessorPostManipulation(bool multiProcessor)
        {
            if (multiProcessor)
            {
                return _httpProcessors[DEFAULT_PROCESSING_GROUP_MULTI].PostManipulators;
            }
            else
            {
                return _httpProcessors[DEFAULT_PROCESSING_GROUP_SIMPLE].PostManipulators;
            }
        }

        public override HttpManipulatorCollection<RoutedContext> GetProcessorPostManipulation(string processingGroup)
        {
            return _httpProcessors[processingGroup].PostManipulators;
        }
    }
}
