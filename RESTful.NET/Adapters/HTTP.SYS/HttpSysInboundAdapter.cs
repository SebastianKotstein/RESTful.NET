using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKotstein.Net.Http.Context;
using System.Net;
using System.Threading;

namespace SKotstein.Net.Http.Adapters.HTTP.SYS
{
    /// <summary>
    /// <see cref="HttpInboundAdapter"/> implementation basing on the Windows HTTP stack (HTTP.sys)
    /// </summary>
    public class HttpSysInboundAdapter : HttpInboundAdapter
    {
        private HttpListener _httpListener = new HttpListener();
        

        protected override void Execute()
        {
            
            while (_httpListener.IsListening)
            {
                //long form of ThreadPool.QueueUserWorkItem((O)=>{....},_httpListener.GetContext());
                //ThreadPool.QueueUserWorkItem(new WaitCallback(PrepareContext), _httpListener.GetContext());
                PrepareContext(_httpListener.GetContext());
            }
        }

        private void PrepareContext(object context)
        {
            HttpListenerContext ctxsys = context as HttpListenerContext;

            //prepare context
            HttpContext ctx = new HttpSysContext(ctxsys);

            Forward(ctx);
        }

        protected override void Final()
        {
            _httpListener.Stop();
            _httpListener.Close();
        }

        protected override void Init()
        {
            _httpListener.Prefixes.Clear();
            _httpListener.Prefixes.Add(Prefix);
            _httpListener.Start();
           
            
        }
    }
}
