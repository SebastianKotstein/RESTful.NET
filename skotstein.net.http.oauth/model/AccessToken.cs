using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    /// <summary>
    /// This class represents the successful response if the request for an access token is valid. The response contains at least an <see cref="AccessToken"/> and the <see cref="TokenType"/>.
    /// All other parameters are optional and depend on the used grant type. An instance of this class can be de-/serialized into/from JSON.
    /// <seealso cref=""/> cref="https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/"/>
    /// </summary>
    public class AccessToken
    {
        private string _accessToken;
        private string _tokenType;
        private long _expiresIn;
        private string _refreshToken;
        private string _scope;


        /// <summary>
        /// Gets or sets the access token string as issues by the authorization server.
        /// </summary>
        [JsonProperty("access_token")]
        public string Token
        {
            get
            {
                return _accessToken;
            }

            set
            {
                _accessToken = value;
            }
        }

        /// <summary>
        /// Gets or sets the token type.
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType
        {
            get
            {
                return _tokenType;
            }

            set
            {
                _tokenType = value;
            }
        }

        /// <summary>
        /// Gets or sets the duration in seconds until the access token expires.
        /// This parameter might be not set (optional).
        /// </summary>
        [JsonProperty("expires_in")]
        public long ExpiresIn
        {
            get
            {
                return _expiresIn;
            }

            set
            {
                _expiresIn = value;
            }
        }

        /// <summary>
        /// Gets or sets the refresh token. Note that this token is optional and depends on the used grant type.
        /// If set, this token can be used to obtain a new access token if the last access token has been expired.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken
        {
            get
            {
                return _refreshToken;
            }

            set
            {
                _refreshToken = value;
            }
        }

        /// <summary>
        /// Gets or sets the scope the user is granted. If the scope is identical to the requested, this parameter might be not set (optional). If the granted scope is different from
        /// the requested scope, then this parameter is required.
        /// </summary>
        [JsonProperty("scope")]
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
