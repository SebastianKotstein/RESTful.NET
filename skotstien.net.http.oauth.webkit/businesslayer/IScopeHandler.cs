using System.Collections.Generic;

namespace skotstein.net.http.oauth.webkit
{
    public interface IScopeHandler
    {
        /// <summary>
        /// Returns a <see cref="Scopes"/> object containing all <see cref="Scope"/>s of the underlying service implementation.
        /// </summary>
        /// <param name="query"><see cref="IDictionary{TKey, TValue}"/> with query parameter, can be null or empty</param>
        /// <returns><see cref="Scopes"/> object containing all <see cref="Scope"/>s</returns>
        Scopes GetScopes(IDictionary<string, string> query);
    }
}