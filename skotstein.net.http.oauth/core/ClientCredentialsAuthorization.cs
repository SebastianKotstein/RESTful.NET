using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKotstein.Net.Http.Context;

namespace skotstein.net.http.oauth
{
    public class ClientCredentialsAuthorization : IAuthorization
    {
        private IClientAccountStorage _storage;
        private OAuth2 _reference;

        public ClientCredentialsAuthorization(OAuth2 reference, IClientAccountStorage storage)
        {
            _reference = reference;
            _storage = storage;
        }

        public void Authorize(HttpContext context, IDictionary<string, string> query)
        {
            string clientId = "";
            string clientSecret = "";
            string requestedScope = "";
            
            //expected but already parsed input is: grant_type
            //expected input is: client_id, client_secret
            //optional input is: scope
            

            //load client ID
            if (query.ContainsKey("client_id"))
            {
                clientId = query["client_id"];
            }
            else
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_REQUEST;
                errorResponse.Error_description = "The client ID is missing. The request must include the query parameter 'client_id'";
                //TODO: add URI
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.BadRequest;
                return;
            }

            //load client secret
            if (query.ContainsKey("client_secret"))
            {
                clientSecret = query["client_secret"];
            }
            else
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_REQUEST;
                errorResponse.Error_description = "The client secret is missing. The request must include the query parameter 'client_secret'";
                //TODO: add URI
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.BadRequest;
                return;
            }

            //load scope
            if (query.ContainsKey("scope"))
            {
                requestedScope = query["scope"];
            }

            //step 1: check whether: 1) client exists, 2) client is not blocked, 3) credentials are correct
            IClientAccount client = _storage.GetClient(clientId);
            if(client == null || client.IsBlocked || client.ClientSecret.CompareTo(clientSecret)!=0)
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                errorResponse.Error_description = "The client ID or secret is invalid";
                //TPDP: add URI
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.Unauthorized;
                return;
            }

            //step 2: validate scope
            string finalScope = "";
            if (String.IsNullOrWhiteSpace(requestedScope))
            {
                finalScope = client.Scope;
            }
            else
            {
                //extract single scopes from requested scope
                string[] scopes;
                if (requestedScope.Contains(","))
                {
                    scopes = requestedScope.Split(',');
                }
                else if (requestedScope.Contains(";"))
                {
                    scopes = requestedScope.Split(';');
                }
                else if(requestedScope.Contains(" "))
                {
                    scopes = requestedScope.Split(' ');
                }
                else
                {
                    scopes = new string[] { requestedScope };
                }

                //check whether all requested scopes are permitted:
                IList<string> permittedScope = client.GetScopeAsList;
                foreach(string scope in scopes)
                {
                    if (!permittedScope.Contains(scope))
                    {
                        InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                        errorResponse.Error = InvalidAuthorizationResponse.INVALID_SCOPE;
                        errorResponse.Error_description = "The scope '" + scope + "' is not permitted or unknown";
                        //TODO: add URL
                        context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                        context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                        context.Response.Status = HttpStatus.BadRequest;
                        return;
                    }
                    finalScope += scope+" ";
                }
                finalScope.Trim();
            }

            //step 3: create access token and (if applicable) refresh token
            AccessToken accessToken = _reference.GenerateAccessToken(null, clientId, finalScope, client.AccessTokenExpiryInSeconds);
            string refreshToken = _reference.GenerateRefreshToken(null, clientId, finalScope, client.RefreshTokenExpiryInSeconds);

            //step 4: return access token and (if applicable) refresh token
            accessToken.RefreshToken = refreshToken;
            context.Response.Payload.Write(JsonSerializer.SerializeJson(accessToken));
            context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
            context.Response.Headers.Set("Cache-Control", "no-store");
            context.Response.Headers.Set("Pragma", "no-cache");
            context.Response.Status = HttpStatus.OK;
        }
    }
}