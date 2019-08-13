using JWT.Algorithms;
using JWT.Builder;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Manipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    
    public class OAuth2 : IAuthorization
    {
        private AccessTokenValidator _accessTokenValidator;

        private string _secret;

        private IAuthorization _refreshTokenAuthorization;
        private IAuthorization _authorizationCodeAuthorization;
        private IAuthorization _passwordAuthorization;
        private IAuthorization _clientCredentialsAuthorization;

        private IList<DynamicAuthorizationScope> _dynamicScope = new List<DynamicAuthorizationScope>();

        /// <summary>
        /// Gets or sets the flag indicating whether refresh tokens are issued
        /// </summary>
        public bool IssuesRefreshTokens
        {
            get
            {
                return _refreshTokenAuthorization != null;
            }

         
        }

        public IAuthorization RefreshTokenAuthorization
        {
            get
            {
                return _refreshTokenAuthorization;
            }

            set
            {
                _refreshTokenAuthorization = value;
            }
        }

        public IAuthorization AuthorizationCodeAuthorization
        {
            get
            {
                return _authorizationCodeAuthorization;
            }

            set
            {
                _authorizationCodeAuthorization = value;
            }
        }

        public IAuthorization PasswordAuthorization
        {
            get
            {
                return _passwordAuthorization;
            }

            set
            {
                _passwordAuthorization = value;
            }
        }

        public IAuthorization ClientCredentialsAuthorization
        {
            get
            {
                return _clientCredentialsAuthorization;
            }

            set
            {
                _clientCredentialsAuthorization = value;
            }
        }

        public AccessTokenValidator AccessTokenValidator
        {
            get
            {
                return _accessTokenValidator;
            }

            set
            {
                _accessTokenValidator = value;
            }
        }

        public OAuth2(string secret)
        {
            _secret = secret;
            _accessTokenValidator = new AccessTokenValidator(secret, this);
        }

        public void Authorize(HttpContext context, IDictionary<string, string> query)
        {
            if (query.ContainsKey("grant_type"))
            {
                string grantType = query["grant_type"];
                switch (grantType)
                {
                    case "refresh_token": //if an application wants to exchange an refresh token for an access token
                        if(_refreshTokenAuthorization != null)
                        {
                            _refreshTokenAuthorization.Authorize(context, query);
                        }
                        else
                        {
                            InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                            errorResponse.Error = InvalidAuthorizationResponse.UNSUPPORTED_GRANT_TYPE;
                            errorResponse.Error_description = "The grant type 'refresh_token' is not supported by this application";
                            //TODO: add URL

                            //write payload
                            context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                            context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                            context.Response.Status = HttpStatus.BadRequest;
                        }
                        break;
                    case "authorization_code": //if an application wants to exchange an authorization code for an access token
                        if(_authorizationCodeAuthorization != null)
                        {
                            _authorizationCodeAuthorization.Authorize(context, query);
                        }
                        else
                        {
                            InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                            errorResponse.Error = InvalidAuthorizationResponse.UNSUPPORTED_GRANT_TYPE;
                            errorResponse.Error_description = "The grant type 'authorization_code' is not supported by this application";
                            //TODO: add URL

                            //write payload
                            context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                            context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                            context.Response.Status = HttpStatus.BadRequest;
                        }
                        break;
                    case "password": //if an application wants to exchange the user's name and password for an access token
                        if(_passwordAuthorization != null)
                        {
                            _passwordAuthorization.Authorize(context, query);
                        }
                        else
                        {
                            InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                            errorResponse.Error = InvalidAuthorizationResponse.UNSUPPORTED_GRANT_TYPE;
                            errorResponse.Error_description = "The grant type 'password' is not supported by this application";
                            //TODO: add URL

                            //write payload
                            context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                            context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                            context.Response.Status = HttpStatus.BadRequest;
                        }
                        break;
                    case "client_credentials": //if an application wants to exchange the client's credentials for an access token, not on behalf of a user
                        if(_clientCredentialsAuthorization != null)
                        {
                            _clientCredentialsAuthorization.Authorize(context, query);
                        }
                        else
                        {
                            InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                            errorResponse.Error = InvalidAuthorizationResponse.UNSUPPORTED_GRANT_TYPE;
                            errorResponse.Error_description = "The grant type 'client_credentials' is not supported by this application";
                            //TODO: add URL

                            //write payload
                            context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                            context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                            context.Response.Status = HttpStatus.BadRequest;
                        }
                        break;
                    default:
                        InvalidAuthorizationResponse errResponse = new InvalidAuthorizationResponse();
                        errResponse.Error = InvalidAuthorizationResponse.UNSUPPORTED_GRANT_TYPE;
                        errResponse.Error_description = "The grant type '"+grantType+"' is not supported by this application";
                        //TODO: add URL

                        //write payload
                        context.Response.Payload.Write(JsonSerializer.SerializeJson(errResponse));
                        context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                        context.Response.Status = HttpStatus.BadRequest;
                        break;
                }
            }
            else if (query.ContainsKey("response_type"))
            {

            }
            else
            {
                //create invalid request
                InvalidAuthorizationResponse errorResponse = new InvalidAuthorizationResponse();
                errorResponse.Error = InvalidAuthorizationResponse.INVALID_REQUEST;
                errorResponse.Error_description = "The grant type or response type is missing. The request must include the query parameter 'grant_type' or 'response_type.";
                //TODO: add URL

                //write payload
                context.Response.Payload.Write(JsonSerializer.SerializeJson(errorResponse));
                context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
                context.Response.Status = HttpStatus.BadRequest;
            }
        }


        public AccessToken GenerateAccessToken(string userId, string clientId, string scope, double expiresInSeconds)
        {
            JwtBuilder encoder = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret(_secret).AddClaim("scope", scope);

            DateTimeOffset now = DateTimeOffset.UtcNow;
            encoder = encoder.AddClaim("iat",now.ToUnixTimeSeconds());
            encoder = encoder.AddClaim("exp", now.AddSeconds(expiresInSeconds).ToUnixTimeSeconds());

            if (!String.IsNullOrWhiteSpace(userId))
            {
                encoder = encoder.AddClaim("sub", userId);
            }
            if (!String.IsNullOrWhiteSpace(clientId))
            {
                encoder = encoder.AddClaim("cid", clientId);
            }
            AccessToken accessToken = new AccessToken();
            accessToken.ExpiresIn = (long)expiresInSeconds;
            accessToken.Scope = scope;
            accessToken.TokenType = "bearer";
            accessToken.Token = encoder.Build();
            return accessToken;
        }

        public string GenerateRefreshToken(string userId, string clientId, string scope, double expiresInSeconds)
        {
            if (_refreshTokenAuthorization != null)
            {
                IRefreshTokenStorage storage = ((RefreshTokenAuthorization)_refreshTokenAuthorization).RefreshStorage;
                string token = Guid.NewGuid().ToString();

                long expiresIn = 0;
                if(expiresInSeconds > 0)
                {
                    expiresInSeconds = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds).ToUnixTimeSeconds();
                }

                storage.CreateRefreshToken(token, userId, clientId, expiresIn, scope, false);
                return token;
            }
            else
            {
                return null;
            }
        }

        public IList<DynamicAuthorizationScope> DynamicScopes
        {
            get
            {
                return _dynamicScope;
            }
        }

         
    }
}
