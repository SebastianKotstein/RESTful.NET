using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using SKotstein.Net.Http.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class UserController : HttpController
    {
        private IUserHandler _handler;

        public UserController(IUserHandler handler)
        {
            _handler = handler;
        }

        [Path(ApiBase.API_V1+"/clients/{id}/users",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetAllUser(HttpContext context, string clientId)
        {
            string json = JsonSerializer.SerializeJson(_handler.GetUsers(context.Request.ParsedQuery,clientId));
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        [Path(ApiBase.API_V1 + "/clients/{id}/users/{id}", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetUser(HttpContext context, string clientId, string userId)
        {
            string json = JsonSerializer.SerializeJson(_handler.GetUser(clientId, userId));
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        [Path(ApiBase.API_V1 + "/clients/{id}/users", HttpMethod.POST)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void CreateUser(HttpContext context, string clientId)
        {
            User user;
            if(context.Request.Payload.Length > 0)
            {
                User input = JsonSerializer.DeserializeJson<User>(context.Request.Payload.ReadAll());
                if(input == null)
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The input data cannot be parsed", 400)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.BadRequest };
                }
                else
                {
                    user = _handler.CreateUser(clientId, input);
                }
            }
            else
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("Input data expected", 400)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.BadRequest };
            }
            string json = JsonSerializer.SerializeJson(user);
            context.Response.Payload.Write(json);
            context.Response.Headers.Set("Location", ApiBase.API_V1 + "/clients/" + clientId + "/users/" + user.UserId);
            context.Response.Status = HttpStatus.Created;
        }

        [Path(ApiBase.API_V1 + "/clients/{id}/users/{id}", HttpMethod.PUT)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void UpdateUser(HttpContext context, string clientId, string userId)
        {
            User user;
            if (context.Request.Payload.Length > 0)
            {
                User input = JsonSerializer.DeserializeJson<User>(context.Request.Payload.ReadAll());
                if (input == null)
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The input data cannot be parsed", 400)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.BadRequest };
                }
                else
                {
                    user = _handler.UpdateUser(clientId,userId, input);
                }
            }
            else
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("Input data expected", 400)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.BadRequest };
            }
            string json = JsonSerializer.SerializeJson(user);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        [Path(ApiBase.API_V1 + "/clients/{id}/users/{id}/password", HttpMethod.PUT)]
        public void ChangePassword(HttpContext context, string clientId, string userId)
        {
            if(context.Request.Payload.Length > 0)
            {
                Password input = JsonSerializer.DeserializeJson<Password>(context.Request.Payload.ReadAll());
                if(input == null)
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The input data cannot be parsed", 400)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.BadRequest };
                }
                else
                {
                    _handler.ChangePassword(clientId, userId, input.OldPassword, input.NewPassword);
                }
            }
            else
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("Input data expected", 400)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.BadRequest };
            }
            context.Response.Status = HttpStatus.OK;
        }

        [Path(ApiBase.API_V1 + "/clients/{id}/users/{id}", HttpMethod.DELETE)]
        public void DeleteUser(HttpContext context, string clientId, string userId)
        {
            _handler.DeleteUser(clientId, userId);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Assigns an OAuth 2.0 Scope to every API endpoint method implemented in <see cref="UserController"/> such that the access to these endpoints
        /// is limited and not granted without permission. The method assigns two types of scopes for reading and writing depending on the purpose of the endpoint method.
        /// The actual name of the reading and writing scope can be specified by the parameters 'scopeRead' and 'scopeWrite'.
        /// The two types of scopes are assigned as follows:
        /// <list type="bullet">
        ///     //TODO: continue list
        /// </list>
        /// </summary>
        /// <param name="scopeRead"></param>
        /// <param name="scopeWrite"></param>
        /// <param name="oauth"></param>
        public static void AssignScopesToEndpoints(string scopeRead, string scopeWrite, OAuth2 oauth)
        {
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("GET" + ApiBase.API_V1 + "/clients/{id}/users", scopeRead));
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("GET"+ ApiBase.API_V1 + "/clients/{id}/users/{id}", scopeRead));
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("POST" + ApiBase.API_V1 + "/clients/{id}/users", scopeWrite));
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("PUT" + ApiBase.API_V1 + "/clients/{id}/users/{id}", scopeWrite));
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("PUT" + ApiBase.API_V1 + "/clients/{id}/users/{id}/password", scopeWrite));
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("DELETE" + ApiBase.API_V1 + "/clients/{id}/users/{id}", scopeWrite));
        }
    }
}
