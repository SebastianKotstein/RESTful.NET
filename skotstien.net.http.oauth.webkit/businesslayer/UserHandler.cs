using skotstein.net.http.oauth;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class UserHandler : IUserHandler
    {
        private IClientAccountStorage _clientStorage;
        private IUserAccountStorage _userStorage;

        public UserHandler(IClientAccountStorage clientStorage, IUserAccountStorage userStorage)
        {
            _clientStorage = clientStorage;
            _userStorage = userStorage;
        }

        public Users GetUsers(IDictionary<string, string> query, string clientId)
        {
            try
            {
                Users users = new Users();
                CheckClient(clientId);

                if(query == null || query.Count == 0)
                {
                    foreach(IUserAccount ua in _userStorage.GetAllUsers())
                    {
                        if (ua.ClientId.CompareTo(clientId) == 0)
                        {
                            User user = User.CreateInstance(ua);
                            user.Password = "*********"; //hide password
                            users.UserList.Add(user);
                        }
                    }
                }
                else
                {
                    foreach (IUserAccount ua in _userStorage.GetAllUsers())
                    {
                        if (ua.ClientId.CompareTo(clientId) != 0)
                        {
                            continue;
                        }
                        if (query.ContainsKey("username"))
                        {
                            if (ua.Username.CompareTo(query["username"]) != 0)
                            {
                                continue;
                            }
                        }
                        if (query.ContainsKey("isBlocked"))
                        {
                            if ((query["isBlocked"].CompareTo("false") == 0 && ua.IsBlocked) || (query["isBlocked"].CompareTo("true") == 0 && !ua.IsBlocked))
                            {
                                continue;
                            }
                        }

                        User user = User.CreateInstance(ua);
                        user.Password = "*********"; //hide password
                        users.UserList.Add(user);
                    }
                }
                return users;
            }
            catch(Exception e)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage(e.Message, 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
            }
        }

        public User GetUser(string clientId, string userId)
        { 
            try
            {
                CheckClient(clientId);
                IUserAccount ua = _userStorage.GetUserById(userId);
                if(ua == null)
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The user with the ID: " + userId + " does not exist", 404)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
                }
                if (ua.ClientId.CompareTo(clientId) != 0) //check whether the user belongs to the specified client
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The user with the ID: " + userId + " does not belong to the specified client", 404)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
                }
                User user = User.CreateInstance(ua);
                user.Password = "*********"; //hide password
                return user;
            }
            catch(HttpRequestException hre)
            {
                throw hre;
            }
            catch (Exception e)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage(e.Message, 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
            }
        }

        public User CreateUser(string clientId, User user)
        {
            try
            {
                CheckClient(clientId);
                CheckUsername(user.Username);
                CheckPasswordPolicy(user.Password);
                user.ClientId = clientId;
                user.UserId = _userStorage.CreateUser(user);
                return user;
            }
            catch(HttpRequestException hre)
            {
                throw hre;
            }
            catch(Exception e)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage(e.Message, 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
            }
        }

        public User UpdateUser(string clientId, string userId, User user)
        {
            try
            {
                CheckClient(clientId);
                IUserAccount ua = _userStorage.GetUserById(userId);
                if (ua == null)
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The user with the ID: " + userId + " does not exist", 404)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
                }
                if (ua.ClientId.CompareTo(clientId) != 0) //check whether the user belongs to the specified client
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The user with the ID: " + userId + " does not belong to the specified client", 404)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
                }

                //updates values:
                //NOTE: UserId, Username, Password cannot be changed by applying UpdateUser
                if (user.HasPropertySet[User.DESCRIPTION_PROPERTY])
                {
                    ua.Description = user.Description;
                }
                if (user.HasPropertySet[User.IS_BLOCKED_PROPERTY])
                {
                    ua.IsBlocked = user.IsBlocked;
                }
                if (user.HasPropertySet[User.NAME_PROPERTY])
                {
                    ua.Name = user.Name;
                }
                if (user.HasPropertySet[User.SCOPE_PROPERTY])
                {
                    ua.Scope = user.Scope;
                }

                _userStorage.UpdateUser(userId, ua);
                User u = User.CreateInstance(ua);
                u.Password = "*********"; //hide password
                return u;
            }
            catch (HttpRequestException hre)
            {
                throw hre;
            }
            catch (Exception e)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage(e.Message, 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
            }
        }

        public void ChangePassword(string clientId, string userId, string oldPassword, string newPassword)
        {
            try
            {
                CheckClient(clientId);
                CheckPasswordPolicy(newPassword);
                IUserAccount ua = _userStorage.GetUserById(userId);
                if (ua == null)
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The user with the ID: " + userId + " does not exist", 404)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
                }
                if (ua.ClientId.CompareTo(clientId) != 0) //check whether the user belongs to the specified client
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The user with the ID: " + userId + " does not belong to the specified client", 404)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
                }
                if (ua.Password.CompareTo(oldPassword) != 0)
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("Old password is invalid", 400)), MimeType.APPLICATION_JSON) { Status = HttpStatus.BadRequest };
                }
                ua.Password = newPassword;
                _userStorage.UpdateUser(userId, ua);
            }
            catch (HttpRequestException hre)
            {
                throw hre;
            }
            catch (Exception e)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage(e.Message, 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
            }
        }

        public void DeleteUser(string clientId, string userId)
        {
            try
            {
                CheckClient(clientId);
                IUserAccount ua = _userStorage.GetUserById(userId);
                if (ua == null)
                {
                    return;
                }
                if (ua.ClientId.CompareTo(clientId) != 0) //check whether the user belongs to the specified client
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The user with the ID: " + userId + " does not belong to the specified client", 404)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
                }
                _userStorage.DeleteUser(userId);
            }
            catch(Exception e)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage(e.Message, 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
            }
        }


        /// <summary>
        /// Checks whether the <see cref="Client"/> having the passed ID exists and whether it has users (see <see cref="Client.HasUser"/>). 
        /// If not, a <see cref="HttpRequestException"/> with <see cref="HttpStatus.NotFound"/> will be thrown.
        /// </summary>
        /// <param name="clientId">ID of the <see cref="Client"/></param>
        private void CheckClient(string clientId)
        {
            IClientAccount client = _clientStorage.GetClient(clientId);
            if(client == null)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The client with the ID: " + clientId + " does not exist", 404)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
            }
            if (!client.HasUser)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The client with the ID: " + clientId + " has no user", 404)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
            }
        }

        /// <summary>
        /// Checks whether the username is already in use. If so, a <see cref="HttpRequestException"/> with <see cref="HttpStatus.BadRequest"/> will be thrown.
        /// </summary>
        /// <param name="username"><see cref="User.Username"/></param>
        private void CheckUsername(string username)
        {
            if(String.IsNullOrWhiteSpace(username) || _userStorage.GetUserByName(username) != null)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The username is already in use", 400)), MimeType.APPLICATION_JSON) { Status = HttpStatus.BadRequest };
            }
        }

        /// <summary>
        /// Checks whether the password meets the complexity criteria of the password policy.
        /// If not, a <see cref="HttpRequestException"/> with <see cref="HttpStatus.BadRequest"/> will be thrown.
        /// </summary>
        /// <param name="password"><see cref="User.Password"/></param>
        private void CheckPasswordPolicy(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The password must be set", 400)), MimeType.APPLICATION_JSON) { Status = HttpStatus.BadRequest };
            }
        }

        
    }
}
