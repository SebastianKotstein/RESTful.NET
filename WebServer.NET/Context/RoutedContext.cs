using SKotstein.Net.Http.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Context
{
    /// <summary>
    /// The <see cref="RoutedContext"/> enriches the underlying <see cref="HttpContext"/> with details of the method which has been selected for processing the context.
    /// Commonly, the <see cref="RoutedContext"/> is exchanged between the <see cref="Http.Core.HttpRouter"/> and a <see cref="Http.Core.HttpProcessor"/> hosting the method for processing this context.
    /// By attaching the <see cref="RoutingEntry"/> the <see cref="Http.Core.HttpProcessor"/> can easily determine the method for processing the context instead of searching the routing table/tree again.
    /// </summary>
    public class RoutedContext
    {
        private HttpContext _context;
        private RoutingEntry _routingEntry;

        /// <summary>
        /// Gets the underlying HttpContext.
        /// </summary>
        public HttpContext Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// Gets the attached routing entry containing details about the target method such that <see cref="HttpContext"/> can be processed.
        /// </summary>
        public RoutingEntry RoutingEntry
        {
            get
            {
                return _routingEntry;
            }
        }

        /// <summary>
        /// Constructs a RoutedContext consiting of a <see cref="HttpContext"/> and the corresponding <see cref="RoutingEntry"/> providing details for processing the request.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routingEntry"></param>
        public RoutedContext(HttpContext context, RoutingEntry routingEntry)
        {
            _context = context;
            _routingEntry = routingEntry;
        }
    }
}
