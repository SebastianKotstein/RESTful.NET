using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class ScopeController : HttpController
    {
        private IScopeHandler _handler;

        public ScopeController(IScopeHandler handler)
        {
            _handler = handler;
        }

        [Path(ApiBase.API_V1+"/scopes", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetAllScopes(HttpContext context)
        {
            string json = JsonSerializer.SerializeJson(_handler.GetScopes(context.Request.ParsedQuery));
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Assigns an OAuth 2.0 Scope to the API endpoint method implemented in <see cref="ScopeController"/> such that the access to this endpoint
        /// is limited and not granted without permission.
        /// The actual name of the  scope can be specified by the parameter 'scopeRead'.
        /// <param name="scopeRead"></param>
        /// <param name="oauth"></param>
        public static void AssignScopeToEndpoint(string scopeRead, OAuth2 oauth)
        {
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("GET" + ApiBase.API_V1 + "/scopes", scopeRead));           
        }

    }
}
