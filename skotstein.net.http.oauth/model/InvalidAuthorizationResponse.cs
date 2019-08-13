using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    /// <summary>
    /// This class represents the unsuccessful response if the request for an access token is invalid. The response contains an <see cref="Error"/> code, typically an <see cref="Error_description"/> and an <see cref="Error_uri"/> pointing to the
    /// documentation providing information how to fix this issue. An instance of this class can be de-/serialized into/from JSON.
    /// <seealso cref=""/> cref="https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/"/>
    /// </summary>
    public class InvalidAuthorizationResponse
    {
        /// <summary>
        /// The request is missing a parameter. The server cannot proceed with the request.
        /// </summary>
        public const string INVALID_REQUEST = "invalid_request";

        /// <summary>
        /// The client authentication failed due to an invalid client ID or client secret.
        /// </summary>
        public const string INVALID_CLIENT = "invalid_client";

        /// <summary>
        /// The authorization code (or user's password for the password grant type) is invalid or expired or if
        /// there is a mismatch between the redirect URL given in the authorization grant and the URL of the access token request.
        /// </summary>
        public const string INVALID_GRANT = "invalid_grant";

        /// <summary>
        /// At least one of the requested scopes is not permitted (only for the password or client credentials grant type).
        /// </summary>
        public const string INVALID_SCOPE = "invalid_scope";

        /// <summary>
        /// The client is not authorized to use the requested grant type.
        /// </summary>
        public const string UNAUTHORIZED_GRANT_TYPE = "unauthorized_grant_type";

        /// <summary>
        /// The request grant type is not supported by the server or unknown.
        /// </summary>
        public const string UNSUPPORTED_GRANT_TYPE = "unsupported_grant_type";



        private string _error;
        private string _error_description;
        private string _error_uri;

        /// <summary>
        /// Gets or sets the error parameter having one of the following values:
        /// invalid_request (see <see cref="INVALID_REQUEST"/>),
        /// invalid_client (see <see cref="INVALID_CLIENT"/>),
        /// invalid_grant (see <see cref="INVALID_GRANT"/>),
        /// invalid_scope (see <see cref="INVALID_SCOPE"/>),
        /// unauthorized_grant_type (see <see cref="UNAUTHORIZED_GRANT_TYPE"/>)
        /// unsupported_grant_type (see <see cref="UNSUPPORTED_GRANT_TYPE"/>)
        /// </summary>
        public string Error
        {
            get
            {
                return _error;
            }

            set
            {
                _error = value;
            }
        }

        /// <summary>
        /// Gets or sets an error description in addition to the <see cref="Error"/>. According to the OAuth 2.0 framework, this error description is not intended to
        /// be shown to the end user.
        /// </summary>
        public string Error_description
        {
            get
            {
                return _error_description;
            }

            set
            {
                _error_description = value;
            }
        }

        /// <summary>
        /// Gets or sets the error URI pointing to an resource for further information. This parameter is typically used to link to a documentation describing how to fix the encountered error. 
        /// </summary>
        public string Error_uri
        {
            get
            {
                return _error_uri;
            }

            set
            {
                _error_uri = value;
            }
        }
    }
}
