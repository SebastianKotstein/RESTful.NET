using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Context
{
    /// <summary>
    /// The <see cref="HttpContext"/> class represents an atomic HTTP session (i.e. a single HTTP request between a client and this server) and contains all underlying HTTP-specific information.
    /// <see cref="HttpContext"/> encapsulates the data of the <see cref="HttpRequest"/> and the associated <see cref="HttpResponse"/>. While the <see cref="HttpRequest"/> object contains all request information provided by the client
    /// after the request has been accepted, the <see cref="HttpResponse"/> object is enriched with content (e.g. its payload) during the processing of the individual HTTP request.
    /// </summary>
    public abstract class HttpContext : IDisposable
    {
        private HttpRequest _request = new HttpRequest();
        private HttpResponse _response = new HttpResponse();

        /// <summary>
        /// Gets the underlying request data object
        /// </summary>
        public HttpRequest Request
        {
            get
            {
                return _request;
            }
            protected set
            {
                _request = value;
            }
        }

        /// <summary>
        /// Gets the underlying response data object
        /// </summary>
        public HttpResponse Response
        {
            get
            {
                return _response;
            }
            protected set
            {
                _response = value;
            }
        }

        public abstract void Dispose();

        /// <summary>
        /// Sends the respone data back to the requester
        /// </summary>
        internal abstract void SendResponse();
    }

    
}
