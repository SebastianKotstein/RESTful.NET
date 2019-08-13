using System.Collections.Generic;

namespace skotstein.net.http.oauth.webkit
{
    /// <summary>
    /// The <see cref="IClientHandler"/> defines methods for creating, retrieving, updating and deleting <see cref="Client"/> objects. Note that every method might
    /// throw a <see cref="HttpRequestException"/> as the behaviour of the underlying persistent layer is not specified any further.
    /// </summary>
    public interface IClientHandler
    {
        /// <summary>
        /// Creates a new <see cref="Client"/> having the specified attributes (encompassed in the passed <see cref="Client"/> object).
        /// Note that the <see cref="Client.ClientId"/> as well as <see cref="Client.ClientSecret"/> is generated automatically and cannot be set manually.
        /// The created <see cref="Client"/> object, including the generated ID and secret, are returned by this method.
        /// </summary>
        /// <param name="client"><see cref="Client"/> containing all attributes of the client which should be created</param>
        /// <returns><see cref="Client"/> as it has been created</returns>
        Client CreateClient(Client client);

        /// <summary>
        /// Deletes the <see cref="Client"/> having the specified ID. Nothing happens, if the is no such <see cref="Client"/>.
        /// </summary>
        /// <param name="id"><see cref="Client.ClientId"/></param>
        void DeleteClient(string id);

        /// <summary>
        /// Generates a new <see cref="Client.ClientSecret"/> for the <see cref="Client"/> having the specified ID. The method throws an <see cref="HttpRequestException"/> if there is no such <see cref="Client"/>.
        /// </summary>
        /// <param name="id"><see cref="Client.ClientId"/></param>
        /// <returns>the updated <see cref="Client"/> object, containing the new <see cref="Client.ClientSecret"/></returns>
        Client GenerateNewSecret(string id);

        /// <summary>
        /// Returns the <see cref="Client"/> having the specified ID. The method throws an <see cref="HttpRequestException"/> if there is no such <see cref="Client"/>.
        /// </summary>
        /// <param name="id"><see cref="Client.ClientId"/></param>
        /// <returns>the <see cref="Client"/> having the specified ID</returns>
        Client GetClient(string id);

        /// <summary>
        /// Returns a <see cref="Clients"/> object containing all <see cref="Client"/>s matching the passed query parameters. If the <see cref="IDictionary{TKey, TValue}"/> is null or empty,
        /// all <see cref="Client"/>s are returned.
        /// </summary>
        /// <param name="query"><see cref="IDictionary{TKey, TValue}"/> with query parameter, can be null or empty</param>
        /// <returns><see cref="Clients"/> object containing <see cref="Client"/>s</returns>
        Clients GetClients(IDictionary<string, string> query);

        /// <summary>
        /// Updates one or multiple attributes of an existing <see cref="Client"/> having the specified ID. The new values of the attributes which should be changed, can be specified in the passed
        /// <see cref="Client"/> object. Note that only these attributes which have been actively set in the passed <see cref="Client"/> will be replaced in the target <see cref="Client"/> object and
        /// note that the <see cref="Client.ClientId"/> as well as the <see cref="Client.ClientSecret"/> cannot be changed (use <see cref="GenerateNewSecret(string)"/> instead, to change the secret).
        /// The method throws an <see cref="HttpRequestException"/> if there is no such <see cref="Client"/>
        /// </summary>
        /// <param name="id"><see cref="Client.ClientId"/></param>
        /// <param name="client"><see cref="Client"/> object containing the new values which should replace the old values</param>
        /// <returns>the updated <see cref="Client"/> object</returns>
        Client UpdateClient(string id, Client client);
    }
}