using SKotstein.Net.Http.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Adapters.HTTP.SYS
{
    /// <summary>
    /// <see cref="HttpContext"/> implementation wrapping an <see cref="HttpListenerContext"/> object of the Windows HTTP stack (HTTP.sys)
    /// </summary>
    public class HttpSysContext : HttpContext
    {
        private HttpListenerContext _originalContext;

        internal HttpListenerContext OriginalContext
        {
            get
            {
                return _originalContext;
            }
            set
            {
                _originalContext = value;
            }
        }

        internal HttpSysContext(HttpListenerContext originalContext)
        {
            _originalContext = originalContext;

            //copy request values into HttpContext:
            //1. HTTP method:
            switch (_originalContext.Request.HttpMethod)
            {
                case "GET":
                    Request.Method = HttpMethod.GET;
                    break;
                case "POST":
                    Request.Method = HttpMethod.POST;
                    break;
                case "PUT":
                    Request.Method = HttpMethod.PUT;
                    break;
                case "DELETE":
                    Request.Method = HttpMethod.DELETE;
                    break;
                case "HEAD":
                    Request.Method = HttpMethod.HEAD;
                    break;
                case "PATCH":
                    Request.Method = HttpMethod.PATCH;
                    break;
                case "TRACE":
                    Request.Method = HttpMethod.TRACE;
                    break;
                case "OPTIONS":
                    Request.Method = HttpMethod.OPTIONS;
                    break;
                case "CONNECT":
                    Request.Method = HttpMethod.CONNECT;
                    break;
                default:
                    Request.Method = HttpMethod.TRACE;
                    break;
            }

            //2. HTTP version:
            if(_originalContext.Request.ProtocolVersion.Major == 1 && _originalContext.Request.ProtocolVersion.Minor == 0)
            {
                //HTTP/1.0
                Request.Version = Context.HttpVersion.HTTP_1_0;
            }
            if (_originalContext.Request.ProtocolVersion.Major == 1 && _originalContext.Request.ProtocolVersion.Minor == 1)
            {
                //HTTP/1.1
                Request.Version = Context.HttpVersion.HTTP_1_1;
            }
            if (_originalContext.Request.ProtocolVersion.Major == 2 && _originalContext.Request.ProtocolVersion.Minor == 0)
            {
                //HTTP/2.0
                Request.Version = Context.HttpVersion.HTTP_2_0;
            }

            //3. Url:
            Request.RawUrl = _originalContext.Request.RawUrl;
            Request.Path = HttpRequest.DecodeUrl(Request.ExtractPath(Request.RawUrl),"ASCII");
            Request.Query = HttpRequest.DecodeUrl(Request.ExtractQuery(Request.RawUrl), "ASCII");
            Request.Fragment = HttpRequest.DecodeUrl(Request.ExtractFragment(Request.RawUrl), "ASCII");

            //4. Content:
            if (_originalContext.Request.HasEntityBody)
            {
                Request.Payload.ReadFrom(_originalContext.Request.InputStream, (int)_originalContext.Request.ContentLength64);
            }

            //5. Headers:
            foreach (string header in _originalContext.Request.Headers.AllKeys)
            {
                Request.Headers.Set(header, _originalContext.Request.Headers.Get(header));
            }
        }

        public override void Dispose()
        {
            _originalContext.Response.Close();
        }

        internal override void SendResponse()
        {
            //1. set Status
            _originalContext.Response.StatusDescription = Response.StatusDescription;
            _originalContext.Response.StatusCode = Response.StatusCode;

            //2. Protocol Version
            //is already set by generating the HTTP.SYS Response

            //3. Add Headers
            foreach(string header in Response.Headers.GetHeaderNames())
            {
                _originalContext.Response.AddHeader(header, Response.Headers.Get(header));
            }

            //4. Add Content and Send:
            _originalContext.Response.Close(Response.Payload.ReadAllBytes(), false);
            string test = Response.Payload.ReadAll();
            //Response.Payload.WriteTo(_originalContext.Response.OutputStream);

            /*
            //5. Send Response
            if (_originalContext.Request.KeepAlive)
            {
                _originalContext.Response.OutputStream.WriteByte(0);
            }
            else
            {
                _originalContext.Response.OutputStream.Close();
            }
            */

            //_originalContext.Response.Close();

        }
    }
}
