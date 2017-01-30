using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
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
    /// Special <see cref="HttpController"/> class which offers predefined REST functions for internal and external usage like functions for HTTP TRACE or HTTP OPTIONS with CORS.
    /// </summary>
    public class HttpInternalController : HttpController
    {

        public const string PATH_NOT_FOUND = "_INTERNAL/path_not_found";
        public const string TRACE = "_INTERNAL/trace";
        public const string OPTIONS = "_INTERNAL/options";
        public const string HTTP_VERSION = "_INTERNAL/server/version";
        public const string BAD_REQUEST = "_INTERNAL/client/bad_request";

        private HttpService _reference;

        internal HttpInternalController(HttpService reference)
        {
            _reference = reference;
        }

        /// <summary>
        /// Retuns a Method not allowed or a Not Found status code
        /// </summary>
        /// <param name="httpContext"></param>
        [Path("/path_not_found",HttpMethod._INTERNAL)]
        public void NotFound(HttpContext httpContext)
        {
            HttpMethod method = httpContext.Request.Method;

            if(method == HttpMethod.GET || method == HttpMethod.PUT || method == HttpMethod.POST || method == HttpMethod.DELETE || method == HttpMethod.PATCH)
            {
                //check whether other methods are allowed and which one:
                IList<string> allowed = new List<string>();
                if (_reference.RoutingTable.GetEntry(HttpMethod.GET + httpContext.Request.Path)!= null)
                {
                    allowed.Add("GET");
                    allowed.Add("HEAD");
                }
                if (_reference.RoutingTable.GetEntry(HttpMethod.PUT + httpContext.Request.Path) != null)
                {
                    allowed.Add("PUT");
                }
                if (_reference.RoutingTable.GetEntry(HttpMethod.POST + httpContext.Request.Path) != null)
                {
                    allowed.Add("POST");
                }
                if (_reference.RoutingTable.GetEntry(HttpMethod.DELETE + httpContext.Request.Path) != null)
                {
                    allowed.Add("DELETE");
                }
                if (_reference.RoutingTable.GetEntry(HttpMethod.PATCH + httpContext.Request.Path) != null)
                {
                    allowed.Add("PATCH");
                }
                if(allowed.Count > 0)
                {
                    httpContext.Response.Status = HttpStatus.MethodNotAllowed;
                    string allowHeader = allowed[0];
                    for(int i = 1; i < allowed.Count; i++)
                    {
                        allowHeader +=", "+allowed[i];
                    }
                    httpContext.Response.Headers.Set("Allow", allowHeader);
                    
                    
                }
                else
                {
                    httpContext.Response.Status = HttpStatus.NotFound;
                }
            }
            else
            {
                httpContext.Response.Status = HttpStatus.NotFound;
            }
        }

        /// <summary>
        /// Traces the HTTP request and returns the request fields as HTTP response
        /// </summary>
        /// <param name="httpContext"></param>
        [Path("/trace",HttpMethod._INTERNAL)]
        [ContentType(MimeType.MESSAGE_HTTP)]
        public void Trace(HttpContext httpContext)
        {
            switch (_reference.ServiceConfiguration.AllowTracing)
            {
                case TracingMode.DISABLED:
                    httpContext.Response.Status = HttpStatus.NotImplemented;
                    break;

                case TracingMode.AUTO:
                    httpContext.Response.Status = HttpStatus.OK;

                    string version = "";
                    switch (httpContext.Request.Version)
                    {
                        case HttpVersion.HTTP_1_0:
                            version = "HTTP/1.0";
                            break;
                        case HttpVersion.HTTP_1_1:
                            version = "HTTP/1.1";
                            break;
                        case HttpVersion.HTTP_2_0:
                            version = "HTTP/2.0";
                            break;
                    }

                    string responsePayload = "TRACE " + httpContext.Request.RawUrl + " " + version + "\n";
                    foreach(string header in httpContext.Request.Headers.GetHeaderNames())
                    {
                        responsePayload += header + ": " + httpContext.Request.Headers.Get(header)+"\n";
                    }
                    responsePayload += "\n";
                    responsePayload += httpContext.Request.Payload.ReadAll()+"\n";

                    httpContext.Response.Payload.Write(responsePayload);
                    break;
                //case TracingMode.MANUAL cannot be reached here due to routing
                
            }
        }

        /// <summary>
        /// Returns an unsupported version status
        /// </summary>
        /// <param name="context"></param>
        [Path("/server/version",HttpMethod._INTERNAL)]
        public void UnsupportedVersion(HttpContext context)
        {
            context.Response.Status = HttpStatus.HTTPVersionNotSupported;
        }

        /// <summary>
        /// Returns a bad request
        /// </summary>
        /// <param name="context"></param>
        [Path("/client/bad_request", HttpMethod._INTERNAL)]
        public void BadRequest(HttpContext context)
        {
            context.Response.Status = HttpStatus.BadRequest;
        }

        /// <summary>
        /// Answers an HTTP OPTION and enables CORS
        /// </summary>
        /// <param name="httpContext"></param>
        [Path("/options",HttpMethod._INTERNAL)]
        public void Options(HttpContext httpContext)
        {
            switch (_reference.ServiceConfiguration.AllowOptions)
            {
                case OptionsMode.DISABLED:
                    httpContext.Response.Status = HttpStatus.NotImplemented;
                    break;
                case OptionsMode.AUTO:

                    //prepare list with allowed methods
                    ISet<string> allowed = new HashSet<string>();

                    //for Asterisk (global)
                    if (httpContext.Request.Path.CompareTo("*") == 0)
                    {

                        foreach (RoutingEntry re in _reference.RoutingTable.RoutingEntries.Values)
                        {
                            if(re.HttpMethod != HttpMethod._INTERNAL)
                            {
                                allowed.Add(re.HttpMethod.ToString());
                            }
                            if(re.HttpMethod == HttpMethod.GET)
                            {
                                allowed.Add("HEAD");
                            }
                        }
                    }
                    else
                    {
                        //for individual paths, determine supportive methods
                        if (_reference.RoutingTable.GetEntry(HttpMethod.GET + httpContext.Request.Path) != null)
                        {
                            allowed.Add("GET");
                            allowed.Add("HEAD");
                        }
                        if (_reference.RoutingTable.GetEntry(HttpMethod.PUT + httpContext.Request.Path) != null)
                        {
                            allowed.Add("PUT");
                        }
                        if (_reference.RoutingTable.GetEntry(HttpMethod.POST + httpContext.Request.Path) != null)
                        {
                            allowed.Add("POST");
                        }
                        if (_reference.RoutingTable.GetEntry(HttpMethod.DELETE + httpContext.Request.Path) != null)
                        {
                            allowed.Add("DELETE");
                        }
                        if (_reference.RoutingTable.GetEntry(HttpMethod.PATCH + httpContext.Request.Path) != null)
                        {
                            allowed.Add("PATCH");
                        }
                        if (_reference.RoutingTable.GetEntry(HttpMethod.CONNECT + httpContext.Request.Path) != null)
                        {
                            allowed.Add("CONNECT");
                        }
                    }
                    //additionally add TRACE and OPTIONS if enabled (AUTO)
                    if (_reference.ServiceConfiguration.AllowTracing == TracingMode.AUTO)
                    {
                        allowed.Add(HttpMethod.TRACE.ToString());
                    }
                    if (_reference.ServiceConfiguration.AllowOptions == OptionsMode.AUTO)
                    {
                        allowed.Add(HttpMethod.OPTIONS.ToString());
                    }
                    //finally build header value:
                    string allowHeader = "";
                    int counter = 0;
                    foreach(string field in allowed)
                    {
                        if(counter == 0)
                        {
                            allowHeader += field;
                        }
                        else
                        {
                            allowHeader += ", " + field;
                        }
                        counter++;
                    }

                    httpContext.Response.Status = HttpStatus.OK;
                    if (!httpContext.Request.Headers.Has("Origin"))
                    {
                        httpContext.Response.Headers.Set("Allow", allowHeader);
                    }
                    else
                    {
                        //if CORS is requested...

                        //set allowed methods
                        httpContext.Response.Headers.Set("Access-Control-Allow-Methods", allowHeader);

                        if (httpContext.Request.Headers.Has("Access-Control-Request-Headers"))
                        {
                            if(_reference.ServiceConfiguration.AllowedHeaders == AllowedHeadersMode.ANY)
                            {
                                httpContext.Request.Headers.Set("Access-Control-Allow-Headers", httpContext.Request.Headers.Get("Access-Control-Request-Headers"));
                            }
                            else
                            {
                                httpContext.Request.Headers.Set("Access-Control-Allow-Headers", _reference.ServiceConfiguration.AllowedStaticHeaders);
                            }
                        }
                    }
                    
                    break;


                //case OptionsMode.MANUAL cannot be reached here due to routing
            }
        }
    }
}
