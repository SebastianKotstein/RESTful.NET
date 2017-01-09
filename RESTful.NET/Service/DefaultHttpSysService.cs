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

namespace SKotstein.Net.Http.Service
{
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
        private IDictionary<string, HttpProcessor> _httpProcessors;
        #endregion   

        public DefaultHttpSysService(bool isSecured, string host, int port)
        {
            _inputPipe = new BlockingCollection<HttpContext>();
            _outputPipe = new BlockingCollection<HttpContext>();

            _httpInboundAdapter = new HttpSysInboundAdapter();
            _httpOutboundAdapter = new HttpSysOutboundAdapter();
            _httpRouter = new HttpRouter(this);
            _httpProcessors = new Dictionary<string, HttpProcessor>();

            //HttpInboundAdapter ----<TaggedContext>----> HttpRouter
            _httpInboundAdapter.OutputPipe = _inputPipe;
            _httpRouter.InputPipe = _inputPipe;

            //---<HttpContext>---> HttpOutboundAdapter
            _httpOutboundAdapter.InputPipe = _outputPipe;

            _routingTable = new RoutingTable();
            AddController(new HttpInternalController(this), INTERNAL_PROCESSING_GROUP);

            //create ServiceConfiguration
            _serviceConfiguration = new ServiceConfiguration();
            _serviceConfiguration.IsSecured = isSecured;
            _serviceConfiguration.Host = host;
            _serviceConfiguration.Port = port;
        }

        public DefaultHttpSysService(ServiceConfiguration serviceConfiguration) : this(false,"",0)
        {
            //overwrite service configuration
            _serviceConfiguration = serviceConfiguration;
        }

        public override void AddController(HttpController httpController)
        {
            AddController(httpController, DEFAULT_PROCESSING_GROUP);
        }

        public override void AddController(HttpController httpController, string processingGroup)
        {
            if (!_httpProcessors.ContainsKey(processingGroup))
            {
                AddProcessor(processingGroup);
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
            foreach(HttpProcessor p in _httpProcessors.Values)
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
            foreach (HttpProcessor p in _httpProcessors.Values)
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

        private void AddProcessor(string processingGroup)
        {
            HttpProcessor httpProcessor = new HttpProcessor(this);
            BlockingCollection<RoutedContext> inputPipe = new BlockingCollection<RoutedContext>();

            //HttpRouter ---<RoutedContext>---> HttpProcessor
            httpProcessor.InputPipe = inputPipe;
            _httpRouter.AddOutputPipe(processingGroup, inputPipe);

            //HttpProcessor ---<HttpContext>---> HttpOutboundAdapter
            httpProcessor.OutputPipe = _httpOutboundAdapter.InputPipe;

            _httpProcessors.Add(processingGroup, httpProcessor);
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
                        _routingTable.AddEntry(pathAttribute.Method.ToString() + pathAttribute.Url, processingGroup, httpController, methodInfo);
                    }

                }
            }
        }


    }
}
