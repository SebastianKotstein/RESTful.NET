using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    public interface IRefreshToken
    {
        string RefreshToken { get; set; }
        string Subject { get; set; }
        string ClientId { get; set; }
        long ValidUntil { get; set; }
        string Scope { get; set; }
        bool IsInvalidated { get; set; }

        IList<string> GetScopeAsList { get;}
    }
}
