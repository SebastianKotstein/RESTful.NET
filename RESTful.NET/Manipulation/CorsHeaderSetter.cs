using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Manipulation
{
    public class CorsHeaderSetter : HttpManipulator<RoutedContext>
    {
        private HttpService _reference;

        public CorsHeaderSetter(HttpService reference)
        {
            _reference = reference;
        }

        public override void Manipulate(RoutedContext ctx)
        {
            if (_reference.ServiceConfiguration.Cors != CorsMode.NEVER)
            {
                if ((ctx.Context.Request.Headers.Has("Origin") && _reference.ServiceConfiguration.Cors == CorsMode.ONLY_IF_ORIGIN_IS_SET))
                {
                    if (_reference.ServiceConfiguration.AllowedOrigin == AllowedOriginMode.ANY)
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
                            ctx.Context.Response.Headers.Set("Vary", "Origin");
                        }
                    }
                }

            }
        }
    }
}
