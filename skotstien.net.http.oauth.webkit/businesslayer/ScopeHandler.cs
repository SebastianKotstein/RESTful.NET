using SKotstein.Net.Http.Routing;
using SKotstein.Net.Http.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class ScopeHandler : IScopeHandler
    {
        private HttpService _service;
        private OAuth2 _oauth2;

        public ScopeHandler(HttpService service, OAuth2 oauth2)
        {
            _service = service;
            _oauth2 = oauth2;
        }

        public Scopes GetScopes(IDictionary<string,string> query)
        {
            Scopes scopes = new Scopes();
            if(query == null || query.Count == 0)
            {
                ISet<string> sscopes = new HashSet<string>();
                foreach(string scope in GetAllScopes())
                {
                    if (!sscopes.Contains(scope))
                    {
                        sscopes.Add(scope);
                        scopes.ScopeList.Add(new Scope() { ScopeName = scope });
                    }
                }
            }
            else
            {
                if (query.ContainsKey("path"))
                {
                    ISet<string> sscopes = new HashSet<string>();
                    foreach (string scope in GetScopesOfPath(query["path"]))
                    {
                        if (!sscopes.Contains(scope))
                        {
                            sscopes.Add(scope);
                            scopes.ScopeList.Add(new Scope() { ScopeName = scope });
                        }
                    }
                }
            }
            return scopes;
        }

        private IList<string> GetAllScopes()
        {
            IList<string> scopes = new List<string>();
            foreach (RoutingEntry re in _service.RoutingEngine.RoutingEntries.Values)
            {
                bool hasDynamicScopes = false;
                foreach (DynamicAuthorizationScope das in _oauth2.DynamicScopes)
                {
                    if (das.Path.CompareTo(re.Path) == 0)
                    {
                        scopes.Add(das.Scope);
                        hasDynamicScopes = true;
                    }
                }
                if (!hasDynamicScopes)
                {
                    foreach (Attribute a in System.Attribute.GetCustomAttributes(re.MethodInfo))
                    {
                        if (a is AuthorizationScope)
                        {
                            scopes.Add(((AuthorizationScope)a).Scope);
                        }
                    }
                }
            }
            return scopes;
        }

        private IList<string> GetScopesOfPath(string path)
        {
            IList<string> scopes = new List<string>();
            foreach(DynamicAuthorizationScope das in _oauth2.DynamicScopes)
            {
                if (das.Path.CompareTo(path) == 0)
                {
                    scopes.Add(das.Scope);
                }
            }

            if(scopes.Count == 0)
            {
                RoutingEntry re = _service.RoutingEngine.GetEntry(path);
                if(re != null)
                {
                    foreach (Attribute a in System.Attribute.GetCustomAttributes(re.MethodInfo))
                    {
                        if (a is AuthorizationScope)
                        {
                            scopes.Add(((AuthorizationScope)a).Scope);
                        }
                    }
                }
            }
            return scopes;
        }
    }
}
