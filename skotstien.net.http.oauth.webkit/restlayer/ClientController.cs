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
    public class ClientController : HttpController
    {


        private IClientHandler _handler;

        public ClientController(IClientHandler handler)
        {
            _handler = handler;
        }

        [Path(ApiBase.API_V1+"/clients",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetAllClients(HttpContext context)
        {
            string json = JsonSerializer.SerializeJson(_handler.GetClients(context.Request.ParsedQuery));
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        [Path(ApiBase.API_V1+"/clients/{Id}",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetClient(HttpContext context, string id)
        {
            string json = JsonSerializer.SerializeJson(_handler.GetClient(id));
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        [Path(ApiBase.API_V1+"/clients",HttpMethod.POST)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void CreateClient(HttpContext context)
        {
            Client client;
            if(context.Request.Payload.Length > 0)
            {
                Client input = JsonSerializer.DeserializeJson<Client>(context.Request.Payload.ReadAll());
                if(input == null)
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The input data cannot be parsed", 400)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.BadRequest };
                }
                else
                {
                    client =_handler.CreateClient(input);
                }
            }
            else
            {
                client = _handler.CreateClient(new Client());
            }
            string json = JsonSerializer.SerializeJson(client);
            context.Response.Payload.Write(json);
            context.Response.Headers.Set("Location", ApiBase.API_V1 + "/clients/" + client.ClientId);
            context.Response.Status = HttpStatus.Created;
        }

        [Path(ApiBase.API_V1+"/clients/{Id}",HttpMethod.PUT)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void UpdateClient(HttpContext context, string id)
        {
            Client client;
            if (context.Request.Payload.Length > 0)
            {
                Client input = JsonSerializer.DeserializeJson<Client>(context.Request.Payload.ReadAll());
                if (input == null)
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The input data cannot be parsed", 400)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.BadRequest };
                }
                else
                {
                    client = _handler.UpdateClient(id,input);
                }
            }
            else
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("Input data expected", 400)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.BadRequest };
            }
            string json = JsonSerializer.SerializeJson(client);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        [Path(ApiBase.API_V1+"/clients/{Id}/secret",HttpMethod.PUT)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GenerateNewSecret(HttpContext context, string id)
        {
            string json = JsonSerializer.SerializeJson(_handler.GenerateNewSecret(id));
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        [Path(ApiBase.API_V1+"/clients/{Id}",HttpMethod.DELETE)]
        public void DeleteClient(HttpContext context, string id)
        {
            _handler.DeleteClient(id);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Assigns an OAuth 2.0 Scope to every API endpoint method implemented in <see cref="ClientController"/> such that the access to these endpoints
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
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("GET" + ApiBase.API_V1 + "/clients", scopeRead));
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("GET" + ApiBase.API_V1 + "/clients/{Id}", scopeRead));
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("POST" + ApiBase.API_V1 + "/clients", scopeWrite));
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("PUT" + ApiBase.API_V1 + "/clients/{Id}", scopeWrite));
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("PUT" + ApiBase.API_V1 + "/clients/{Id}/secret", scopeWrite));
            oauth.DynamicScopes.Add(new DynamicAuthorizationScope("DELETE" + ApiBase.API_V1 + "/clients/{Id}", scopeWrite));
        }

    }
}
