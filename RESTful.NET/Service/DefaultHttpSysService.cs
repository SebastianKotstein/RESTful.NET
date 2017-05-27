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
        const string DEFAULT_PROCESSING_GROUP = "DEFAULT_PG";
        const string INTERNAL_PROCESSING_GROUP = "INTERNAL_PG";
        #endregion

        #region Pipes & Filters
        private BlockingCollection<HttpContext> _inputPipe;
        private BlockingCollection<HttpContext> _outputPipe;

        private HttpInboundAdapter _httpInboundAdapter;
        private HttpOutboundAdapter _httpOutboundAdapter;
        private HttpRouter _httpRouter;
        private IDictionary<string, HttpSimpleProcessor> _httpSimpleProcessors;
        private IDictionary<string, HttpLimitedMultiProcessor> _httpLimitedMultiProcessors;
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
            _httpSimpleProcessors = new Dictionary<string, HttpSimpleProcessor>();
            _httpLimitedMultiProcessors = new Dictionary<string, HttpLimitedMultiProcessor>();


            //HttpInboundAdapter ----<TaggedContext>----> HttpRouter
            _httpInboundAdapter.OutputPipe = _inputPipe;
            _httpRouter.InputPipe = _inputPipe;

            //---<HttpContext>---> HttpOutboundAdapter
            _httpOutboundAdapter.InputPipe = _outputPipe;

            _routingEngine = new RoutingEngine();
            AddController(new HttpInternalController(this), INTERNAL_PROCESSING_GROUP);

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
            AddController(httpController, DEFAULT_PROCESSING_GROUP, multiprocessing);
        }

        public override void AddController(HttpController httpController, string processingGroup, bool multiprocessing)
        {
            if (!multiprocessing)
            {
                if (!_httpSimpleProcessors.ContainsKey("SIMPLE_"+processingGroup))
                {
                    AddProcessor("SIMPLE_" + processingGroup, multiprocessing);
                }
                RegisterMethods("SIMPLE_" + processingGroup, httpController);
            }
            else
            {
                if (!_httpLimitedMultiProcessors.ContainsKey("MULTI_"+processingGroup))
                {
                    AddProcessor("MULTI_" + processingGroup, multiprocessing);
                }
                RegisterMethods("MULTI_" + processingGroup, httpController);
            }
            
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
            foreach(HttpSimpleProcessor p in _httpSimpleProcessors.Values)
            {
                p.Start();
            }
            foreach (HttpLimitedMultiProcessor p in _httpLimitedMultiProcessors.Values)
            {
                p.Start();
            }
            _httpRouter.Start();
            _httpInboundAdapter.Start();

        }

        public override void Stop()
        {
            _httpInboundAdapter.Stop();
            _httpRouter.Stop();
            foreach (HttpSimpleProcessor p in _httpSimpleProcessors.Values)
            {
                p.Stop();
            }
            foreach (HttpLimitedMultiProcessor p in _httpLimitedMultiProcessors.Values)
            {
                p.Stop();
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
            if (!multiprocessing)
            {
                HttpSimpleProcessor httpProcessor = new HttpSimpleProcessor(this);
                BlockingCollection<RoutedContext> inputPipe = new BlockingCollection<RoutedContext>();

                //HttpRouter ---<RoutedContext>---> HttpProcessor
                httpProcessor.InputPipe = inputPipe;
                _httpRouter.AddOutputPipe(processingGroup, inputPipe);

                //HttpProcessor ---<HttpContext>---> HttpOutboundAdapter
                httpProcessor.OutputPipe = _httpOutboundAdapter.InputPipe;

                _httpSimpleProcessors.Add(processingGroup, httpProcessor);
            }
            else
            {
                HttpLimitedMultiProcessor httpProcessor = new HttpLimitedMultiProcessor(this);
                BlockingCollection<RoutedContext> inputPipe = new BlockingCollection<RoutedContext>();

                //HttpRouter ---<RoutedContext>---> HttpProcessor
                httpProcessor.InputPipe = inputPipe;
                _httpRouter.AddOutputPipe(processingGroup, inputPipe);

                //HttpProcessor ---<HttpContext>---> HttpOutboundAdapter
                httpProcessor.OutputPipe = _httpOutboundAdapter.InputPipe;

                _httpLimitedMultiProcessors.Add(processingGroup, httpProcessor);
            }
            
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

        public override HttpManipulatorCollection<HttpContext> GetProcessorPreManipulation(bool multiProcessor)
        {
            if (multiProcessor)
            {
                return _httpLimitedMultiProcessors[DEFAULT_PROCESSING_GROUP].PreManipulators;
            }
            else
            {
                return _httpSimpleProcessors[DEFAULT_PROCESSING_GROUP].PreManipulators;
            }
        }

        public override HttpManipulatorCollection<HttpContext> GetProcessorPreManipulation(string processingGroup, bool multiProcessor)
        {
            if (multiProcessor)
            {
                return _httpLimitedMultiProcessors[processingGroup].PreManipulators;
            }
            else
            {
                return _httpSimpleProcessors[processingGroup].PreManipulators;
            }
        }

        public override HttpManipulatorCollection<HttpContext> GetProcessorPostManipulation(bool multiProcessor)
        {
            if (multiProcessor)
            {
                return _httpLimitedMultiProcessors[DEFAULT_PROCESSING_GROUP].PostManipulators;
            }
            else
            {
                return _httpSimpleProcessors[DEFAULT_PROCESSING_GROUP].PostManipulators;
            }
        }

        public override HttpManipulatorCollection<HttpContext> GetProcessorPostManipulation(string processingGroup, bool multiProcessor)
        {
            if (multiProcessor)
            {
                return _httpLimitedMultiProcessors[processingGroup].PostManipulators;
            }
            else
            {
                return _httpSimpleProcessors[processingGroup].PostManipulators;
            }
        }
    }
}
