using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKotstein.Net.Http.Context;

namespace SKotstein.Net.Http.Adapters.HTTP.Base
{
    public class HttpBaseOutboundAdapter : HttpOutboundAdapter
    {
        protected override void Execute(HttpContext task)
        {
            task.SendResponse();
        }

        protected override void Final()
        {
            
        }

        protected override void Init()
        {
            
        }
    }
}
