using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Pipelining.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Core
{
    /// <summary>
    /// An implementation of this abstract class is responsible for sending HTTP responses.
    /// </summary>
    public abstract class HttpOutboundAdapter : SingleOutboundAdapter<HttpContext>
    {
    }
}
