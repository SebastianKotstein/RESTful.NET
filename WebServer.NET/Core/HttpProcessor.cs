using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Pipelining;
using SKotstein.Net.Http.Routing;
using SKotstein.Net.Http.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Core
{
    public class HttpProcessor : SimpleProcessor<RoutedContext, HttpContext>
    {
        private HttpService _reference;

        internal HttpProcessor(HttpService reference)
        {
            _reference = reference;
        }

        protected override HttpContext Execute(RoutedContext task)
        {
            //set default Header
            SetHeader(task);
            //invoke method
            InvokeMethod(task);
            //post processing
            Postprocessing(task.Context);

            return task.Context;
        }

        protected override void Final()
        {
            
        }

        protected override void Init()
        {
            
        }

        private HttpContext InvokeMethod(RoutedContext context)
        {
            //extract routing information
            RoutingEntry entry = context.RoutingEntry;
            MethodInfo method = entry.MethodInfo;
            HttpController controller = entry.HttpController;

            //prepare parameter list
            IList<string> variables = ExtractPathVariables(entry.Path, context.Context.Request.Path);

            object[] parameters = new object[1 + variables.Count]; //prepare parameter list for invocation
            parameters[0] = context.Context; //the first parameter is always the context object
            //followed by the extracted URL variables:
            for(int i = 0; i < variables.Count; i++)
            {
                parameters[i + 1] = variables[i];
            }

            //invoke method:
            HttpContext httpContext = (HttpContext)method.Invoke(controller, parameters);

            /*
            if (httpContext == null)
            {
                httpContext = context.Context;
                httpContext.Response.Status = HttpStatus.InternalServerError;
                //TODO: set Content with error
            }
            */
            return httpContext;
        }

        /// <summary>
        /// Extracts the variable part (so-called variables) of the request url. Those parts are declared as variables segments by a URL pattern. Variable parts (segments) are marked with "{....}".
        /// Example: 
        /// Request URL: /test/1/Max/application/2
        /// Pattern: /test/{id}/{Name}/application/{application_id}
        /// Extracted variables for id, Name and application_id: 1, Max, 2
        /// </summary>
        /// <param name="pattern">URL pattern</param>
        /// <param name="path">request URL</param>
        /// <returns>list with extracted variables (values only) ordered by their appearance in the URL</returns>
        private IList<string> ExtractPathVariables(string pattern, string path)
        {
            IList<string> variables = new List<string>();

            //1. split pattern
            string[] patternElements = path.Split('/');

            //2. split path (of the HTTP Request)
            string[] pathElements = path.Split('/');

            //3. search for generic fields
            for (int i = 0; i < patternElements.Length; i++)
            {
                if (patternElements[i].Contains("{"))
                {
                    variables.Add(pathElements[i]);
                }
            }
            return variables;
        }

        private void SetHeader(RoutedContext ctx)
        {
            //set ContentType (if applicable)
            MethodInfo methodInfo = ctx.RoutingEntry.MethodInfo;

            System.Attribute[] attr = System.Attribute.GetCustomAttributes(methodInfo);
            foreach(Attribute a in attr)
            {
                if(a is ContentType)
                {
                    string contentTypeValue = ((ContentType)a).ContentTypeValue;
                    string charsetValue = ((ContentType)a).CharsetValue;

                    if(charsetValue!= null)
                    {
                        ctx.Context.Response.Headers.Set("Content-Type", contentTypeValue + "; " + charsetValue);
                        switch (charsetValue)
                        {
                            case Charset.ASCII:
                                ctx.Context.Response.Payload.DefaultEncoding = Encoding.ASCII;
                                break;
                            case Charset.UTF7:
                                ctx.Context.Response.Payload.DefaultEncoding = Encoding.UTF7;
                                break;
                            case Charset.UTF8:
                                ctx.Context.Response.Payload.DefaultEncoding = Encoding.UTF8;
                                break;
                            case Charset.UTF32:
                                ctx.Context.Response.Payload.DefaultEncoding = Encoding.UTF32;
                                break;
                            default:
                                //let default 
                                break;
                        }
                    }
                    else
                    {
                        ctx.Context.Response.Headers.Set("Content-Type", contentTypeValue);
                    }

                    
                }
            }
            //set CORS Header
            if (_reference.ServiceConfiguration.Cors != CorsMode.NEVER)
            {
                if ((ctx.Context.Request.Headers.Has("Origin") && _reference.ServiceConfiguration.Cors == CorsMode.ONLY_IF_ORIGIN_IS_SET))
                {
                    if(_reference.ServiceConfiguration.AllowedOrigin == AllowedOriginMode.ANY)
                    {
                        ctx.Context.Response.Headers.Set("Access-Control-Allow-Origin", "*");
                    }
                    else
                    {
                        ctx.Context.Response.Headers.Set("Access-Control-Allow-Origin", ctx.Context.Request.Headers.Get("Origin"));
                        if (ctx.Context.Response.Headers.Has("Vary"))
                        {
                            ctx.Context.Response.Headers.Set("Vary", ctx.Context.Response.Headers.Get("Vary") + ", Origin");
                        }
                        else
                        {
                            ctx.Context.Response.Headers.Set("Vary","Origin");
                        }
                    }
                }
                
            }
        }
        private void Postprocessing(RoutedContext ctx)
        {
            if(ctx.Context.Request.Method == HttpMethod.HEAD)
            {
                ctx.Context.Response.Headers.Remove("Content-Type");
                ctx.Context.Response.Payload.ClearAll();
            }
            ctx.Context.Response.Headers.Set("Content-Length", ctx.Context.Response.Payload.Length+"");

            if(ctx.Context.Response.Payload.Length == 0 && ctx.RoutingEntry.MethodHasContentTypeAttribute)
            {
                ctx.Context.Response.Headers.Remove("Content-Type");
            }
        }
    }
}
