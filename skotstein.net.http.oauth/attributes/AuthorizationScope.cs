using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple=true)]
    public class AuthorizationScope : System.Attribute
    {
        private string _scope;

        public string Scope
        {
            get
            {
                return _scope;
            }
        }

        public AuthorizationScope(string scope)
        {
            _scope = scope;
        }
    }
}
