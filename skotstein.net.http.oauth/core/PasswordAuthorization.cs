using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKotstein.Net.Http.Context;

namespace skotstein.net.http.oauth
{
    public class PasswordAuthorization : IAuthorization
    {
        private OAuth2 _reference;

        private IClientAccountStorage _clientStorage;
        private IUserAccountStorage _userStorage;

        public PasswordAuthorization(OAuth2 reference, IClientAccountStorage clientStorage, IUserAccountStorage userStorage)
        {
            _reference = reference;
            _clientStorage = clientStorage;
            _userStorage = userStorage;
        }

        public void Authorize(HttpContext context, IDictionary<string, string> query)
        {
            string username = "";
            string password = "";
            string clientId = "";
            string clientSecret = "";
            string requestedScope = "";

            //expected but already parsed input is: grant_type
            //expected input is: username, password
            //optional input is: scope

            //load username
            if (query.ContainsKey("username"))
            {
                username = query["username"];
            }
            else
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_REQUEST;
                errorResponse.Error_description = "The username is missing. The request must include the query parameter 'username'";
                //TODO: add URI
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.BadRequest;
                return;
            }

            //load password
            if (query.ContainsKey("password"))
            {
                password = query["password"];
            }
            else
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_REQUEST;
                errorResponse.Error_description = "The password is missing. The request must include the query parameter 'password'";
                //TODO: add URI
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.BadRequest;
                return;
            }

            //load client ID:
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

            //load scope
            if (query.ContainsKey("scope"))
            {
                requestedScope = query["scope"];
            }

            //step 1: check whether: 1) client exists, 2) client is not blocked, 3) client has user
            IClientAccount client = _clientStorage.GetClient(clientId);
            if(client == null || client.IsBlocked || !client.HasUser)
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                errorResponse.Error_description = "The client ID is invalid";
                //TPDP: add URI
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.Unauthorized;
                return;
            }

            //step 2 (if applicable): 1) check whether client's secret is set (if enabled) and 2) if it is correct
            if (client.IsClientSecretRequiredForPasswordGrant)
            {
                if (String.IsNullOrWhiteSpace(clientSecret))
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
                if (client.ClientSecret.CompareTo(clientSecret) != 0)
                {
                    InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                    errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                    errorResponse.Error_description = "The client ID or secret is invalid";
                    //TODO: add URI
                    context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                    context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                    context.Response.Status = HttpStatus.Unauthorized;
                    return;
                }
            }

            //step 3: check whether 1) user exists, 2) user is not blocked, 3) password is correct and 4) the user belongs to the passed client
            IUserAccount user = _userStorage.GetUserByName(username);
            if(user == null || user.IsBlocked || user.Password.CompareTo(password) != 0 || user.ClientId.CompareTo(clientId)!=0)
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                errorResponse.Error_description = "The username or password invalid";
                //TODO: add URI
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.Unauthorized;
                return;
            }

            //step 4: validate scope
            string finalScope = "";
            if (String.IsNullOrWhiteSpace(requestedScope))
            {
                finalScope = user.Scope;
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
                else if (requestedScope.Contains(" "))
                {
                    scopes = requestedScope.Split(' ');
                }
                else
                {
                    scopes = new string[] { requestedScope };
                }

                //check whether all requested scopes are permitted:
                IList<string> permittedScope = user.GetScopeAsList;
                foreach (string scope in scopes)
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
                    finalScope += scope + " ";
                }
                finalScope.Trim();
            }

            //step 6: create access token and (if applicable) refresh token
            AccessToken accessToken = _reference.GenerateAccessToken(username, clientId, finalScope, client.AccessTokenExpiryInSeconds);
            string refreshToken = _reference.GenerateRefreshToken(username, clientId, finalScope, client.RefreshTokenExpiryInSeconds);

            //step 7: return access token and (if applicable) refresh token
            accessToken.RefreshToken = refreshToken;
            context.Response.Payload.Write(JsonSerializer.SerializeJson(accessToken));
            context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
            context.Response.Headers.Set("Cache-Control", "no-store");
            context.Response.Headers.Set("Pragma", "no-cache");
            context.Response.Status = HttpStatus.OK;



        }
    }
}
