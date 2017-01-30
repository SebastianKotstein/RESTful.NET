using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Adapters.HTTP.SYS
{
    /// <summary>
    /// <see cref="HttpSysOutboundAdapter"/> implementation basing on the Windows HTTP stack (HTTP.sys)
    /// </summary> 
   public class HttpSysOutboundAdapter : HttpOutboundAdapter
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
