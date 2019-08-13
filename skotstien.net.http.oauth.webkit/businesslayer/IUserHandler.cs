using System.Collections.Generic;

namespace skotstein.net.http.oauth.webkit
{
    public interface IUserHandler
    {
        void ChangePassword(string clientId, string userId, string oldPassword, string newPassword);
        User CreateUser(string clientId, User user);
        void DeleteUser(string clientId, string userId);
        User GetUser(string clientId, string userId);
        Users GetUsers(IDictionary<string, string> query, string clientId);
        User UpdateUser(string clientId, string userId, User user);
    }
}