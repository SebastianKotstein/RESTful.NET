using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    public interface IClientAccount
    {
        string ClientId { get; set; }

        string FriendlyName { get; set; }

        string Description { get; set; }

        string ClientSecret { get; set; }

        string Scope { get; set; }
        bool IsBlocked { get; set; }

        long AccessTokenExpiryInSeconds { get; set; }

        long RefreshTokenExpiryInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether this client can have users (true) of whether this is a standalone, non-personalized client application (false)
        /// </summary>
        bool HasUser { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the client ID is required if a refresh token is exchanged for an access token
        /// </summary>
        bool IsClientIdRequiredForRefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the client secret is required if a refresh token is exchanged for an access token
        /// </summary>
        bool IsClientSecretRequiredForRefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the user ID is required if a refresh token is exchanged for an access token. 
        /// Note that the value of this flag has no effect, if <see cref="HasUser"/> is false.  
        /// </summary>
        bool IsUserIdRequiredForRefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the user password is required if a refresh token is exchanged for an access token.
        /// Note that the value of this flag has not effect, if <see cref="HasUser"/> is false.
        /// </summary>
        bool IsUserPasswordRequiredForRefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the client secret is required if username and password is exchanged for an access token / refresh token.
        /// </summary>
        bool IsClientSecretRequiredForPasswordGrant { get; set; }


        IList<string> GetScopeAsList { get; }



    }
}
