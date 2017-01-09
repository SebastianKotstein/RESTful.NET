using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Context
{
    /// <summary>
    /// Encapsulates the HTTP-specific information of an HTTP request.
    /// </summary>
    public class HttpRequest
    {
        private HttpPayload _payload;
        private HttpRequestHeaders _headers;
        private HttpMethod _method;
        private HttpVersion _version;

        private string _rawUrl;
        private string _fragment;
        private string _query;
        private string _path;

        /// <summary>
        /// Constructor
        /// </summary>
        internal HttpRequest()
        {
            _headers = new HttpRequestHeaders();
            _payload = new HttpPayload();
        }

        /// <summary>
        /// Gets the payload instance associated with this request.
        /// </summary>
        public HttpPayload Payload
        {
            get
            {
                return _payload;
            }
            internal set
            {
                _payload = value;
            }
        }

        /// <summary>
        /// Gets the header instance containing all headers associated with this request.
        /// </summary>
        public HttpRequestHeaders Headers
        {
            get
            {
                return _headers;
            }
        }

        /// <summary>
        /// Gets the HTTP method associated with this request.
        /// </summary>
        public HttpMethod Method
        {
            get
            {
                return _method;
            }
            internal set
            {
                _method = value;
            }
        }

        /// <summary>
        /// Gets the protocol version of this request.
        /// </summary>
        public HttpVersion Version
        {
            get
            {
                return _version;
            }
            internal set
            {
                _version = value;
            }
        }

        /// <summary>
        /// Returns the raw URL as is has been sent by the requester.
        /// </summary>
        public string RawUrl
        {
            get
            {
                return _rawUrl;
            }
            internal set
            {
                _rawUrl = value;
            }
        }

        /// <summary>
        /// Returns the decoded fragment segment of the URL.
        /// </summary>
        public string Fragment
        {
            get
            {
                return _fragment;
            }
            internal set
            {
                _fragment = value;
            }
        }

        /// <summary>
        /// Returns the decoded query segment of the URL.
        /// </summary>
        public string Query
        {
            get
            {
                return _query;
            }
            internal set
            {
                _query = value;
            }
        }

        /// <summary>
        /// Returns the decoded path segment of the URL.
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
            internal set
            {
                _path = value;
            }
        }

        /// <summary>
        /// Extracts the fragment segment of the passed raw URL.
        /// </summary>
        /// <param name="rawUrl">raw URL</param>
        /// <returns>decoded fragment</returns>
        internal string ExtractFragment(string rawUrl)
        {
            if (rawUrl.Contains("#"))
            {
                string[] splitted = rawUrl.Split('#');
                if(splitted.Length == 2)
                {
                    return splitted[1];
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Extracts the path segment of the passed raw URL.
        /// </summary>
        /// <param name="rawUrl">raw URL</param>
        /// <returns>decoded path</returns>
        internal string ExtractPath(string rawUrl)
        {
            string[] splitted = rawUrl.Split('?');
            return splitted[0].Split('#')[0];
            
        }

        /// <summary>
        /// Extracts the query segment of the passed raw URL.
        /// </summary>
        /// <param name="rawUrl">raw URL</param>
        /// <returns>decoded query</returns>
        internal string ExtractQuery(string rawUrl)
        {
            if (rawUrl.Contains("?"))
            {
                string[] splitted = rawUrl.Split('?');
                if(splitted.Length == 2)
                {
                    return splitted[1].Split('#')[0];
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Decodes a URL part by ASCII Endcoding ONLY!
        /// </summary>
        /// <param name="urlPart">URL part</param>
        /// <param name="encoding">Only ASCII is supported so far...</param>
        /// <returns>Decoded URL part</returns>
        public static string DecodeUrl(string urlPart, string encoding)
        {
            //still ASCII is supported yet
            string output = urlPart;
            output = output.Replace("%20", " ");
            output = output.Replace("%21", "!");
            output = output.Replace("%22", "\"");
            output = output.Replace("%23", "#");
            output = output.Replace("%24", "$");
            output = output.Replace("%26", "&");
            output = output.Replace("%27", "'");
            output = output.Replace("%28", "(");
            output = output.Replace("%29", ")");
            output = output.Replace("%2A", "*");
            output = output.Replace("%2B", "+");
            output = output.Replace("%2C", ",");
            output = output.Replace("%2F", "/");
            output = output.Replace("%3A", ":");
            output = output.Replace("%3B", ";");
            output = output.Replace("%3D", "=");
            output = output.Replace("%3F", "?");
            output = output.Replace("%40", "@");
            output = output.Replace("%5B", "[");
            output = output.Replace("%5C", "\\");
            output = output.Replace("%5D", "]");
            output = output.Replace("%7B", "{");
            output = output.Replace("%7C", "|");
            output = output.Replace("%2D", "}");
            output = output.Replace("%25", "%");
            return output;
        }
      

    }

    /// <summary>
    /// Enumeration of HTTP methods. <see cref="HttpMethod._INTERNAL"/> and <see cref="HttpMethod.UNKNOWN"/> are for internal usage only.
    /// </summary>
    public enum HttpMethod
    {
        GET,
        POST,
        PUT,
        DELETE,
        HEAD,
        PATCH,
        TRACE,
        OPTIONS,
        CONNECT,
        _INTERNAL,
        UNKNOWN
    }


    public enum HttpVersion
    {
        HTTP_1_0,
        HTTP_1_1,
        HTTP_2_0
    }
}
