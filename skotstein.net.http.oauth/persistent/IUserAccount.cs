using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    public interface IUserAccount
    {
        string UserId { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        bool IsBlocked { get; set; }

        string Password { get; set; }

        string Username { get; set; }

        string ClientId { get; set; }

        string Scope { get; set; }

        IList<string> GetScopeAsList { get; }
    }

}
