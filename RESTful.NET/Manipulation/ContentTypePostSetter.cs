using SKotstein.Net.Http.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Manipulation
{
    public class ContentTypePostSetter : HttpManipulator<RoutedContext>
    {
        public override void Manipulate(RoutedContext ctx)
        {
            if (ctx.Context.Request.Method == HttpMethod.HEAD)
            {
                ctx.Context.Response.Headers.Remove("Content-Type");
                ctx.Context.Response.Payload.ClearAll();
            }
            ctx.Context.Response.Headers.Set("Content-Length", ctx.Context.Response.Payload.Length + "");

            if (ctx.Context.Response.Payload.Length == 0 && ctx.RoutingEntry.MethodHasContentTypeAttribute)
            {
                ctx.Context.Response.Headers.Remove("Content-Type");
            }
        }
    }
}
