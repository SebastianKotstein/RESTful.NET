using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth
{
    public interface IRefreshTokenStorage
    {
        /// <summary>
        /// Returns a list containing all <see cref="IRefreshToken"/> entities.
        /// </summary>
        /// <returns>list with all <see cref="IRefreshToken"/>s</returns>
        IList<IRefreshToken> GetAllRefreshTokens();

        /// <summary>
        /// Returns the <see cref="IRefreshToken"/> entity having the passed refresh token or null, if there is no <see cref="IRefreshToken"/> having the passed refresh token.
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <returns>the <see cref="IRefreshToken"/> entity having the passed refresh token</returns>
        IRefreshToken GetRefreshToken(string refreshToken);

        /// <summary>
        /// Returns true if there is a <see cref="IRefreshToken"/> having the passed refresh token, else false.
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <returns>true or false</returns>
        bool HasRefreshToken(string refreshToken);

        /// <summary>
        /// Creates and stores a new <see cref="IRefreshToken"/> entity. Note that the passed refresh token must be unique. The method throws an <see cref="Exception"/>
        /// if there is already a <see cref="IRefreshToken"/> having the passed refresh token or if the token or the passed data is not set.
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <param name="data">new data of the <see cref="IRefreshToken"/></param>
        void CreateRefreshToken(string refreshToken, IRefreshToken data);

        /// <summary>
        /// Creates and stores a new <see cref="IRefreshToken"/> entity. Note that the passed refresh token must be unique. The method throws an <see cref="Exception"/>
        /// if there is already a <see cref="IRefreshToken"/> having the passed refresh token or if the token is not set.
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <param name="subject">the subject (e.g. the user ID) this token is issued to</param>
        /// <param name="clientId">the client ID associated with this refresh token</param>
        /// <param name="validUntil">the date of validation in seconds</param>
        /// <param name="scope">the permitted scope of the refresh token</param>
        /// <param name="isInvalidated">flag for a manual invalidation</param>
        void CreateRefreshToken(string refreshToken, string subject, string clientId, long validUntil, string scope, bool isInvalidated);

        /// <summary>
        /// Updates the data of the <see cref="IRefreshToken"/> having the passed refresh token. The method throws an <see cref="Exception"/> if there is no <see cref="IRefreshToken"/>
        /// having the passed refresh token or if the token or the passed data is not set.
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <param name="data">new of the <see cref="IRefreshToken"/></param>
        void UpdateRefreshToken(string refreshToken, IRefreshToken data);

        /// <summary>
        /// Deletes the <see cref="IRefreshToken"/> having the passed refresh token. Nothing will happen, if there is no <see cref="IRefreshToken"/> having the passed refresh token.
        /// The method throws an <see cref="Exception"/> if the refresh token is not set.
        /// </summary>
        /// <param name="refreshToken">refresh token of the <see cref="IRefreshToken"/> which should be deleted</param>
        void DeleteRefreshToken(string refreshToken);
    }
}
