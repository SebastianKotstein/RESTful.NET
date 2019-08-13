using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Manipulation;
using SKotstein.Net.Http.Model.Exceptions;
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
    /// <summary>
    /// Contains various operations (methods) which are required for pre and post processing an HTTP request/response.
    /// </summary>
    public class HttpProcessorOperations
    {
        private HttpService _reference;

        public HttpProcessorOperations(HttpService reference)
        {
            _reference = reference;
        }

        public void ProcessHttpRequest(RoutedContext context, HttpManipulatorCollection<RoutedContext> internalPreManipulators, HttpManipulatorCollection<RoutedContext> preManipulators, HttpManipulatorCollection<RoutedContext> internalPostManipulators, HttpManipulatorCollection<RoutedContext> postManipulators)
        {
            try
            {
                //execute pre manipulators first
                internalPreManipulators.Manipulate(context);
                preManipulators.Manipulate(context);

                //extract routing information
                RoutingEntry entry = context.RoutingEntry;
                MethodInfo method = entry.MethodInfo;
                HttpController controller = entry.HttpController;

                //prepare parameter list
                IList<string> variables = ExtractPathVariables(entry.Path, context.Context.Request.Path);

                object[] parameters = new object[1 + variables.Count]; //prepare parameter list for invocation
                parameters[0] = context.Context; //the first parameter is always the context object
                                                 //followed by the extracted URL variables:
                for (int i = 0; i < variables.Count; i++)
                {
                    parameters[i + 1] = variables[i];
                }

                //invoke method
                method.Invoke(controller, parameters);

                //post processing
                internalPostManipulators.Manipulate(context);
                postManipulators.Manipulate(context);
            }
            catch(HttpRequestException hre) //this might never be the case, as HttpRequestException is always an inner exeception
            {
                if (!String.IsNullOrWhiteSpace(hre.ErrorMessage))
                {
                    context.Context.Response.Payload.Write(hre.ErrorMessage);
                    if (!String.IsNullOrWhiteSpace(hre.ContentType))
                    {
                        context.Context.Response.Headers.Set("Content-Type", hre.ContentType);
                    }
                    else
                    {
                        context.Context.Response.Headers.Set("Content-Type", MimeType.TEXT_PLAN);
                    }
                }
                context.Context.Response.Status = hre.Status;
            }
            catch(Exception e)
            {
                if(e.InnerException is HttpRequestException)
                {
                    HttpRequestException hre = (HttpRequestException)e.InnerException;
                    if (!String.IsNullOrWhiteSpace(hre.ErrorMessage))
                    {
                        context.Context.Response.Payload.Write(hre.ErrorMessage);
                        if (!String.IsNullOrWhiteSpace(hre.ContentType))
                        {
                            context.Context.Response.Headers.Set("Content-Type", hre.ContentType);
                        }
                        else
                        {
                            context.Context.Response.Headers.Set("Content-Type", MimeType.TEXT_PLAN);
                        }
                    }
                    context.Context.Response.Status = hre.Status;

                }
                else
                {
                    if(e.InnerException!=null && !String.IsNullOrEmpty(e.InnerException.Message))
                    {
                        context.Context.Response.Payload.Write(e.InnerException.Message);
                    }
                    else
                    {
                        context.Context.Response.Payload.Write(e.Message);
                    }
                    
                    context.Context.Response.Headers.Set("Content-Type", MimeType.TEXT_PLAN);
                    context.Context.Response.Status = HttpStatus.InternalServerError;
                }
            }
           

            /*
            //invoke method:
            HttpContext httpContext = (HttpContext)

            /*
            if (httpContext == null)
            {
                httpContext = context.Context;
                httpContext.Response.Status = HttpStatus.InternalServerError;
                //TODO: set Content with error
            }
            
            return httpContext;
            */
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
            string[] patternElements = pattern.Split('/');

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

    }
}
