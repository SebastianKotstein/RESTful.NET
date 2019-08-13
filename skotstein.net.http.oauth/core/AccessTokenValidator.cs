using JWT;
using JWT.Builder;
using Newtonsoft.Json;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Manipulation;
using SKotstein.Net.Http.Model.Exceptions;
using SKotstein.Net.Http.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    /// <summary>
    /// The class <see cref="AccessTokenValidator"/> is an <see cref="HttpManipulator{RoutedContext}"/> which checks preliminarily whether a requested operation, identified by the combination of URL (i.e. the resource being subject of the operation) and <see cref="HttpMethod"/>, is protected by one or multiple scopes. If so, it checks whether the HTTP request contains a valid access token certifying
    /// that the client is authorized to perform the operation (see <see cref="Manipulate(RoutedContext)"/> for further details). Make sure that this <see cref="HttpManipulator{RoutedContext}"/> is added as a pre manipulator for every <see cref="HttpProcessor"/> hosting resources/providing operations (e.g. REST functions) being
    /// protected by a predefined set of scopes (use <see cref="HttpService.GetProcessorPreManipulation(...).Add(...)"/>. to add this <see cref="HttpManipulator{RoutedContext}"/> to an <see cref="HttpProcessor"/>). Otherwise, a resource/operation might be unprotected even if it is associated with a scope.
    /// </summary>
    public class AccessTokenValidator : HttpManipulator<RoutedContext>
    {
        private JwtBuilder _decoder;

        private OAuth2 _reference;

        public AccessTokenValidator(string secret, OAuth2 reference)
        {
            _decoder = new JwtBuilder().WithSecret(secret).MustVerifySignature();
            _reference = reference;
        }

        /// <summary>
        /// The method checks whether the requested operation (i.e. the combination of URL and <see cref="HttpMethod"/>) is protected by one or multiple predefined scopes. Scopes can be defined statically, by applying <see cref="AuthorizationScope"/> attribute
        /// to the C# method representing a resource or a RESTful operation or dynamically during runtime by using <see cref="DynamicAuthorizationScope"/>s (see <see cref="OAuth2.DynamicScopes"/>). If at least one <see cref="DynamicAuthorizationScope"/>
        /// is defined for the corresponding method, then all predefined <see cref="AuthorizationScope"/>s are ignored. If there are no scopes defined (dynamically as well as statically), this method grants access even without an access token.
        /// In case that the resource/operation is protected by scopes, the method checks whether the request contains a valid access token in the Authorization header. Since this framework uses Self-Encoded Access Tokens, the scopes the client is granted to can be decoded from the provided access token and
        /// compared with the predefined scopes of the method. At least one granted scope must match the list of predefined scopes, otherwise the request is rejected with an <see cref="HttpStatus.Unauthorized"/> immediately (same applies if no access token is included or if the access token is invalid).
        /// </summary>
        /// <param name="context">Routed Context</param>
        public override void Manipulate(RoutedContext context)
        {

            //step 1: check whether method has scopes (i.e. is a protected resource)
            IList<string> scopes = new List<string>();
            //check first, whether there are dynamic scopes specifiec
            foreach(DynamicAuthorizationScope das in _reference.DynamicScopes)
            {
                if (das.Path.CompareTo(context.RoutingEntry.Path) == 0)
                {
                    scopes.Add(das.Scope);
                }
            }

            

            //if not, check whether there are static scopes via attributes defined
            if(scopes.Count == 0)
            {
                scopes = GetScopesOfMethod(context.RoutingEntry.MethodInfo);
            }

            //if there are no scopes, ...
            if (scopes.Count == 0)
            {
                //...do nothing (i.e. the request can be processed as no permission is required for accessing the resource)
                return;
            }

            //step 1a: load access token
            string token = context.Context.Request.Headers.Get("Authorization");
            if (String.IsNullOrWhiteSpace(token))
            {
                throw new HttpRequestException("Missing access token", MimeType.TEXT_PLAN) { Status = HttpStatus.Unauthorized };
            }
            token = token.Replace("bearer ", "").Replace("Bearer ","");

            //step 2: decode token
            string scope = null;
            try
            {
                IDictionary<string, object> payload = _decoder.Decode<IDictionary<string, object>>(token);
                if (payload.ContainsKey("scope"))
                {
                    scope = (string)payload["scope"];
                }
                else
                {
                    throw new HttpRequestException("Invalid access token", MimeType.TEXT_PLAN) { Status = HttpStatus.Unauthorized };
                }
            }
            catch(TokenExpiredException)
            {
                throw new HttpRequestException("Token has expired", MimeType.TEXT_PLAN) { Status = HttpStatus.Unauthorized };
            }
            catch (SignatureVerificationException)
            {
                throw new HttpRequestException("Invalid access token", MimeType.TEXT_PLAN) { Status = HttpStatus.Unauthorized };
            }
            catch (Exception)
            {
                throw new HttpRequestException("Invalid access token", MimeType.TEXT_PLAN) { Status = HttpStatus.Unauthorized };
            }


            //step 3: check scope
            if (String.IsNullOrWhiteSpace(scope))
            {
                throw new HttpRequestException("Invalid scope: you are not allowed to access this resource", MimeType.TEXT_PLAN) { Status = HttpStatus.Forbidden };
            }

            bool match = false;
            string[] scopess = scope.Split(' ');
            foreach(string scp in scopess) //for all scopes supported by the access token
            {
                foreach(string scpp in scopes) //for all scopes supported by the method
                {
                    if (scp.CompareTo(scpp)==0)
                    {
                        match = true;
                    }
                }
            }
            if (!match)
            {
                throw new HttpRequestException("Invalid scope: you are not allowed to access this resource", MimeType.TEXT_PLAN) { Status = HttpStatus.Forbidden };
            }
        }

        /// <summary>
        /// Returns a list with the scopes defined for a specific method. The list is empty, if the method does not define any scope.
        /// </summary>
        /// <param name="method">method whose scopes should be returned</param>
        /// <returns>list with scopes</returns>
        private IList<string> GetScopesOfMethod(MethodInfo method)
        {
            IList<string> scopes = new List<string>();
            foreach(Attribute a in System.Attribute.GetCustomAttributes(method))
            {
                if(a is AuthorizationScope)
                {
                    scopes.Add(((AuthorizationScope)a).Scope);
                }
            }
            return scopes;
        }
    }
}
