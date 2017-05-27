using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Manipulation
{
    public class ContentTypePreSetter : HttpManipulator<RoutedContext>
    {
        public override void Manipulate(RoutedContext ctx)
        {
            //set ContentType (if applicable)
            MethodInfo methodInfo = ctx.RoutingEntry.MethodInfo;

            System.Attribute[] attr = System.Attribute.GetCustomAttributes(methodInfo);
            foreach (Attribute a in attr)
            {
                if (a is ContentType)
                {
                    string contentTypeValue = ((ContentType)a).ContentTypeValue;
                    string charsetValue = ((ContentType)a).CharsetValue;

                    if (charsetValue != null)
                    {
                        ctx.Context.Response.Headers.Set("Content-Type", contentTypeValue + "; " + charsetValue);
                        ctx.Context.Request.Headers.Get("Content-Type");
                        switch (charsetValue)
                        {
                            case Charset.ASCII:
                                ctx.Context.Response.Payload.DefaultEncoding = Encoding.ASCII;
                                break;
                            case Charset.UTF7:
                                ctx.Context.Response.Payload.DefaultEncoding = Encoding.UTF7;
                                break;
                            case Charset.UTF8:
                                ctx.Context.Response.Payload.DefaultEncoding = Encoding.UTF8;
                                break;
                            case Charset.UTF32:
                                ctx.Context.Response.Payload.DefaultEncoding = Encoding.UTF32;
                                break;
                            default:
                                //let default 
                                break;
                        }
                    }
                    else
                    {
                        ctx.Context.Response.Headers.Set("Content-Type", contentTypeValue);
                    }
                }
            }
        }
    }
}
