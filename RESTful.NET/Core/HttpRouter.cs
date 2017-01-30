using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Pipelining;
using SKotstein.Net.Http.Routing;
using SKotstein.Net.Http.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Core
{
    /// <summary>
    /// The <see cref="HttpRouter"/> forwards an incoming <see cref="HttpContext"/> object (i.e. an HTTP request) to the designated <see cref="HttpProcessor"/> hosting the target REST function
    /// mapping the URL and the HTTP method. Invalid HTTP requests and "special" requests like HTTP OPTIONS and HTTP TRACE are routed to the <see cref="HttpProcessor"/> hosting the <see cref="HttpInternalController"/>.
    /// </summary>
    public class HttpRouter : Router<HttpContext, RoutedContext>
    {
        private HttpService _reference;

        public HttpRouter(HttpService reference)
        {
            _reference = reference;
        }


        protected override void Final()
        {
            
        }

        protected override void Init()
        {
            
        }

        protected override void Route(HttpContext task)
        {
            //prepare route
            string route = "";
            
            if(task.Request.Version == HttpVersion.HTTP_2_0)
            {
                route = HttpInternalController.HTTP_VERSION;
            }
            else if (!task.Request.Headers.Has("Host"))
            {
                route = HttpInternalController.BAD_REQUEST;
            }
            else if (task.Request.Method == HttpMethod.TRACE && (_reference.ServiceConfiguration.AllowTracing == TracingMode.DISABLED || _reference.ServiceConfiguration.AllowTracing == TracingMode.AUTO))
            {
                route = HttpInternalController.TRACE;
            }
            else if (task.Request.Method == HttpMethod.OPTIONS && (_reference.ServiceConfiguration.AllowOptions == OptionsMode.DISABLED || _reference.ServiceConfiguration.AllowOptions == OptionsMode.AUTO))
            {
                route = HttpInternalController.OPTIONS;
            }
            else if (task.Request.Method == HttpMethod.HEAD)
            {
                route = @"GET" + task.Request.Path;
            }
            else
            {
                route = task.Request.Method.ToString() + task.Request.Path;
            }

            //determining routing target
            RoutingEntry entry = _reference.RoutingTable.GetEntry(route);

            //if no such entry --> EXCEPTION
            if (entry == null)
            {
                entry = _reference.RoutingTable.GetEntry(HttpInternalController.PATH_NOT_FOUND);
            }

            //pack routed context
            RoutedContext routedContext = new RoutedContext(task,entry);
           
            Forward(entry.ProcessingGroup, routedContext);
            


        }
    }
}
