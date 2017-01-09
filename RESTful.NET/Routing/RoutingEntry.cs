using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Routing
{
    /// <summary>
    /// Associates a REST function with its corresponding C# method by offering details like the unique <see cref="Path"/> of the REST function, its <see cref="ProcessingGroup"/>,
    /// the <see cref="HttpController"/> as the host of the method and its <see cref="MethodInfo"/>.
    /// </summary>
    public class RoutingEntry
    {
        private string _identifier;
        private string _path;
        private string _processingGroup;
        private HttpController _httpController;
        private MethodInfo _methodInfo;

        /// <summary>
        /// Gets the routing tree target identifier associated with this routing entry
        /// </summary>
        public string Identifier
        {
            get
            {
                return _identifier;
            }
        }

        /// <summary>
        /// Gets the unique path of the underlying REST function pointing on the associated method
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
        }

        /// <summary>
        /// Gets the processing group performing the associated method
        /// </summary>
        public string ProcessingGroup
        {
            get
            {
                return _processingGroup;
            }
        }

        /// <summary>
        /// Gets the HttpController hosting the associated method
        /// </summary>
        public HttpController HttpController
        {
            get
            {
                return _httpController;
            }
        }

        /// <summary>
        /// Returns the method info of the associated method
        /// </summary>
        public MethodInfo MethodInfo
        {
            get
            {
                return _methodInfo;
            }
        }

        /// <summary>
        /// Returns the HttpMethod
        /// </summary>
        public HttpMethod HttpMethod
        {
            get
            {
                switch (Path.Split('/')[0])
                {
                    case "GET":
                        return HttpMethod.GET;
                    case "POST":
                        return HttpMethod.POST;
                    case "PUT":
                        return HttpMethod.PUT;
                    case "DELETE":
                        return HttpMethod.DELETE;
                    case "HEAD":
                        return HttpMethod.HEAD;
                    case "PATCH":
                        return HttpMethod.PATCH;
                    case "TRACE":
                        return HttpMethod.TRACE;
                    case "OPTIONS":
                        return HttpMethod.OPTIONS;
                    case "CONNECT":
                        return HttpMethod.CONNECT;
                    case "_INTERNAL":
                        return HttpMethod._INTERNAL;
                    default:
                        return HttpMethod.UNKNOWN;
                }

            }
        }

        /// <summary>
        /// Indicates whether the underlying method is tagged with a <see cref="ContentType"/> attribute or not
        /// </summary>
        public bool MethodHasContentTypeAttribute
        {
            get
            {
                bool hasContentTypeAttribute = false;
                foreach(Attribute a in System.Attribute.GetCustomAttributes(MethodInfo))
                {
                    if(a is ContentType)
                    {
                        hasContentTypeAttribute = true;
                    }
                }
                return hasContentTypeAttribute;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processingGroup">name of the processing group</param>
        /// <param name="httpController">HttpController</param>
        /// <param name="methodInfo">method info</param>
        /// <param name="identifier"> target identifier (optional, only for internal usage)</param>
        /// <param name="path">unique path of the REST function</param>
        public RoutingEntry(string processingGroup, HttpController httpController, MethodInfo methodInfo, string identifier, string path)
        {
            _processingGroup = processingGroup;
            _httpController = httpController;
            _methodInfo = methodInfo;
            _identifier = identifier;
            _path = path;

        }


    }
}
