using SKotstein.Net.Http.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    public interface IAuthorization
    {
        void Authorize(HttpContext context, IDictionary<string,string> query);
    }
}
