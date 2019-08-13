using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKotstein.Net.Http.Context;

namespace skotstein.net.http.oauth
{
    public class RefreshTokenAuthorization : IAuthorization
    {
        private IClientAccountStorage _clientStorage;
        private IRefreshTokenStorage _refreshStorage;
        private IUserAccountStorage _userStorage;
        private OAuth2 _reference;

        private bool _issuesNewRefreshToken;
        private long _newRefreshTokenExpiryThreshold;


        public bool IssuesNewRefreshToken
        {
            get
            {
                return _issuesNewRefreshToken;
            }

            set
            {
                _issuesNewRefreshToken = value;
            }
        }

        public long NewRefreshTokenExpiryThreshold
        {
            get
            {
                return _newRefreshTokenExpiryThreshold;
            }

            set
            {
                _newRefreshTokenExpiryThreshold = value;
            }
        }

        public IRefreshTokenStorage RefreshStorage
        {
            get
            {
                return _refreshStorage;
            }
        }

        public RefreshTokenAuthorization(OAuth2 reference, IClientAccountStorage clientStorage, IRefreshTokenStorage refreshStorage)
        {
            _clientStorage = clientStorage;
            _refreshStorage = refreshStorage;
            _reference = reference;
        }

        public void Authorize(HttpContext context, IDictionary<string, string> query)
        {
            string refreshToken;
            string clientId = "";
            string clientSecret = "";
            string username = "";
            string userPassword = "";
            string requestedScope = "";

            //expected but already parsed input is: grant_type
            //expected input is: refresh token
            //expected input is (if enabled): client ID
            //expected input is (if enabled): client secret
            //expected input is (if enabled): user ID
            //expected input is (if enabled): user password
            //optional input is: scope

            //load refresh token
            if (query.ContainsKey("refresh_token"))
            {
                refreshToken = query["refresh_token"];
            }
            else
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_REQUEST;
                errorResponse.Error_description = "The refresh token is missing. The request must include the query parameter 'refresh_token'";
                //TODO: add URI
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.BadRequest;
                return;
            }

            //load client ID
            if (query.ContainsKey("client_id"))
            {
                clientId = query["client_id"];
            }

            //load client secret
            if (query.ContainsKey("client_secret"))
            {
                clientSecret = query["client_secret"];
            }

            //load user ID
            if (query.ContainsKey("username"))
            {
                username = query["username"];
            }

            //load user password
            if (query.ContainsKey("password"))
            {
                userPassword = query["password"];
            }

            //load scope
            if (query.ContainsKey("scope"))
            {
                requestedScope = query["scope"];
            }

            //step 1: load refresh token and check whether the refresh token is listed and is valid
            IRefreshToken refreshTokenSet = _refreshStorage.GetRefreshToken(refreshToken);
            if(refreshTokenSet == null || refreshTokenSet.IsInvalidated)
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                errorResponse.Error_description = "The refresh token is invalid";
                //TODO: add uri
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.Unauthorized;
                return;
            }
            //step 2: check whether the refresh token has expired
            if (refreshTokenSet.ValidUntil > 0 && refreshTokenSet.ValidUntil < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                errorResponse.Error_description = "The refresh token has expired";
                //TODO: add uri
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.Unauthorized;
                return;
            }

            //step 3: load client and check whether the client is blocked
            IClientAccount client = _clientStorage.GetClient(refreshTokenSet.ClientId);
            if(client == null || client.IsBlocked)
            {
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                errorResponse.Error_description = "The refresh token has been issued to a client which is not existing or has been blocked";
                //TODO: add URI
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.Unauthorized;
                return;
            }

            //step 3a (if enabled): check whether client is linked to the refresh token
            if (client.IsClientIdRequiredForRefreshToken)
            {
                if (String.IsNullOrWhiteSpace(clientId))
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
                if (client.ClientId.CompareTo(clientId) != 0)
                {
                    InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                    errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                    errorResponse.Error_description = "The client ID is invalid";
                    //TODO: add URI
                    context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                    context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                    context.Response.Status = HttpStatus.Unauthorized;
                    return;
                }
            }

            //step 3b (if enabled): check whether client secret is valid
            if (client.IsClientSecretRequiredForRefreshToken)
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

            IUserAccount user = null;
            if (client.HasUser)
            {
                //step 4a: check whether the refresh token has been issued to a user
                if (String.IsNullOrWhiteSpace(refreshTokenSet.Subject))
                {
                    InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                    errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                    errorResponse.Error_description = "The refresh token is invalid";
                    //TODO: add uri
                    context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                    context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                    context.Response.Status = HttpStatus.Unauthorized;
                    return;
                }

                user = _userStorage.GetUserByName(refreshTokenSet.Subject);
                //step 4b: check whether the user is existing and not blocked
                if(user == null || user.IsBlocked)
                {
                    InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                    errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                    errorResponse.Error_description = "The refresh token has been issued to a user which is not existing or has been blocked";
                    //TODO: add URI
                    context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                    context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                    context.Response.Status = HttpStatus.Unauthorized;
                    return;
                }

                //step 4c: check whether the linked subject (user ID) matches the passed user ID (if enabled)
                if (client.IsUserIdRequiredForRefreshToken)
                {
                    if (String.IsNullOrWhiteSpace(username))
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

                    if (refreshTokenSet.Subject.CompareTo(username) != 0)
                    {
                        InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                        errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                        errorResponse.Error_description = "The username is invalid";
                        //TODO: add URI
                        context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                        context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                        context.Response.Status = HttpStatus.Unauthorized;
                        return;
                    }
                }

                //step 4d: check whether the passed user password is correct (if enabled)
                if (client.IsUserPasswordRequiredForRefreshToken)
                {
                    if (String.IsNullOrWhiteSpace(userPassword))
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

                    if (user.Password.CompareTo(userPassword) != 0)
                    {
                        InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                        errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                        errorResponse.Error_description = "The username or password is invalid";
                        //TODO: add URI
                        context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                        context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                        context.Response.Status = HttpStatus.Unauthorized;
                        return;
                    }
                }
            }
            else
            {
                //check whether client has NO user, but the refresh token has been issued to a user:
                if (!String.IsNullOrWhiteSpace(refreshTokenSet.Subject))
                {

                    InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                    errorResponse.Error = InvalidAuthorizationResponse.INVALID_CLIENT;
                    errorResponse.Error_description = "The refresh token is invalid";
                    //TODO: add uri
                    context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                    context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                    context.Response.Status = HttpStatus.Unauthorized;
                    return;
                }
            }

            //step 4: check scope
            string finalScope = "";
            if (String.IsNullOrWhiteSpace(requestedScope))
            {
                finalScope = refreshTokenSet.Scope;
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
                IList<string> permittedScope = refreshTokenSet.GetScopeAsList;
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

            //step 5: create new access token and issue new refresh token (if enabled and applicable)
            string subject;
            if(user == null)
            {
                subject = null;
            }
            else
            {
                subject = user.Username;
            }

            AccessToken accessToken = _reference.GenerateAccessToken(subject, clientId, finalScope, client.AccessTokenExpiryInSeconds);

            if(_issuesNewRefreshToken && refreshTokenSet.ValidUntil - DateTimeOffset.UtcNow.ToUnixTimeSeconds() < _newRefreshTokenExpiryThreshold)
            {
                accessToken.RefreshToken = _reference.GenerateRefreshToken(subject, client.ClientId, finalScope, client.RefreshTokenExpiryInSeconds);
            }
            else
            {
                accessToken.RefreshToken = null;
            }
            context.Response.Payload.Write(JsonSerializer.SerializeJson(accessToken));
            context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
            context.Response.Headers.Set("Cache-Control", "no-store");
            context.Response.Headers.Set("Pragma", "no-cache");
            context.Response.Status = HttpStatus.OK;

        }
    }
}
