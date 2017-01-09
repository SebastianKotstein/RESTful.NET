using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTestServer
{
    public class ContactManagementController : HttpController
    {
        [Path("/contact",HttpMethod.GET)]
        [Path("/contact",HttpMethod.POST)]
        [Path("/contact",HttpMethod.DELETE)]
        [Path("/contact", HttpMethod.PUT)]
        [ContentType(MimeType.TEXT_PLAN,Charset.UTF8)]
        public void SomeGetMethod(HttpContext ctx)
        {
            ctx.Response.Status = HttpStatus.OK;

            ctx.Response.Payload.Write("You have called /contact");

            
            
        }

        [Path("/echo",HttpMethod.POST)]
        [ContentType(MimeType.MESSAGE_HTTP)]
        public void Echo(HttpContext ctx)
        {
            ctx.Response.Status = HttpStatus.OK;
            ctx.Response.Payload.Write(ctx.Request.Payload.ReadAll());
        }
    }
}
