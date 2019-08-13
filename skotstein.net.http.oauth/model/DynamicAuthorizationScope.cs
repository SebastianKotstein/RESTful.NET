using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    public class DynamicAuthorizationScope
    {
        private string _path;
        private string _scope;

        public DynamicAuthorizationScope(string path, string scope)
        {
            _path = path;
            _scope = scope;
        }

        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
            }
        }

        public string Scope
        {
            get
            {
                return _scope;
            }

            set
            {
                _scope = value;
            }
        }
    }
}
