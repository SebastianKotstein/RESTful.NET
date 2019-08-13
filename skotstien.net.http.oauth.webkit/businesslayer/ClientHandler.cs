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
    public class ClientHandler : IClientHandler
    {
        private IClientAccountStorage _clientStorage;

        public ClientHandler(IClientAccountStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        public Clients GetClients(IDictionary<string,string> query)
        {
            try
            {
                Clients clients = new Clients();
                if (query == null || query.Count == 0)
                {
                    foreach (IClientAccount ca in _clientStorage.GetAllClients())
                    {
                        Client client = Client.CreateInstance(ca);
                        client.ClientSecret = "*********"; //hide secret
                        clients.ClientList.Add(client);
                    }
                }
                else
                {
                    foreach (IClientAccount ca in _clientStorage.GetAllClients())
                    {
                        if (query.ContainsKey("name"))
                        {
                            if (ca.FriendlyName.CompareTo(query["name"]) != 0)
                            {
                                continue;
                            }
                        }

                        if (query.ContainsKey("isBlocked"))
                        {
                            if ((query["isBlocked"].CompareTo("false") == 0 && ca.IsBlocked) || (query["isBlocked"].CompareTo("true") == 0 && !ca.IsBlocked))
                            {
                                continue;
                            }
                        }

                        if (query.ContainsKey("hasUser"))
                        {
                            if ((query["hasUser"].CompareTo("false") == 0 && ca.HasUser) || (query["hasUser"].CompareTo("true") == 0 && !ca.HasUser))
                            {
                                continue;
                            }
                        }

                        Client client = Client.CreateInstance(ca);
                        client.ClientSecret = "*********"; //hide secret
                        clients.ClientList.Add(client);
                    }
                }
                return clients;
            }
            catch (Exception e)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage(e.Message, 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
            }
        }

        public Client GetClient(string id)
        {
            IClientAccount ca;
            try
            {
                ca = _clientStorage.GetClient(id);
            }
            catch (Exception e)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage(e.Message, 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
            }

            if (ca != null)
            {
                Client client =  Client.CreateInstance(ca);
                client.ClientSecret = "*********"; //hide secret
                return client;
            }
            else
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("The client with the ID: "+id+" does not exist",404)),MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
            }
        }

        public Client CreateClient(Client client)
        {
            try
            {
                client.ClientSecret = Guid.NewGuid().ToString();
                client.ClientId = _clientStorage.CreateClient(client);
                return client;
            }
            catch(Exception e)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage(e.Message, 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
            }
        }
        
        public Client UpdateClient(string id, Client client)
        {
            try
            {
                IClientAccount ca = _clientStorage.GetClient(id);
                if (ca != null)
                {
                    if (client.HasPropertySet[Client.NAME_PROPERTY])
                    {
                        ca.FriendlyName = client.FriendlyName;
                    }
                    if (client.HasPropertySet[Client.DESCRIPTION_PROPERTY])
                    {
                        ca.Description = client.Description;
                    }
                    if (client.HasPropertySet[Client.SCOPE_PROPOERTY])
                    {
                        ca.Scope = client.Scope;
                    }
                    if (client.HasPropertySet[Client.IS_BLOCKED_PROPERTY])
                    {
                        ca.IsBlocked = client.IsBlocked;
                    }
                    if (client.HasPropertySet[Client.ACCESS_TOKEN_EXPIRY_PROPERTY])
                    {
                        ca.AccessTokenExpiryInSeconds = client.AccessTokenExpiryInSeconds;
                    }
                    if (client.HasPropertySet[Client.REFRESH_TOKEN_EXPIRY_PROPERTY])
                    {
                        ca.RefreshTokenExpiryInSeconds = client.RefreshTokenExpiryInSeconds;
                    }
                    if (client.HasPropertySet[Client.HAS_USER_PROPERTY])
                    {
                        ca.HasUser = client.HasUser;
                    }
                    if (client.HasPropertySet[Client.IS_CLIENT_ID_REQUIRED_FOR_REFRESH_TOKEN])
                    {
                        ca.IsClientIdRequiredForRefreshToken = client.IsClientIdRequiredForRefreshToken;
                    }
                    if (client.HasPropertySet[Client.IS_CLIENT_SECRET_REQUIRED_FOR_REFRESH_TOKEN])
                    {
                        ca.IsClientSecretRequiredForRefreshToken = client.IsClientSecretRequiredForRefreshToken;
                    }
                    if (client.HasPropertySet[Client.IS_USER_ID_REQUIRED_FOR_REFRESH_TOKEN])
                    {
                        ca.IsUserIdRequiredForRefreshToken = client.IsUserIdRequiredForRefreshToken;
                    }
                    if (client.HasPropertySet[Client.IS_USER_PASSWORD_REQUIRED_FOR_REFRESH_TOKEN])
                    {
                        ca.IsUserPasswordRequiredForRefreshToken = client.IsUserPasswordRequiredForRefreshToken;
                    }
                    if (client.HasPropertySet[Client.IS_CLIENT_SECRET_REQUIRED_FOR_PASSWORD_GRANT])
                    {
                        ca.IsClientSecretRequiredForPasswordGrant = client.IsClientSecretRequiredForPasswordGrant;
                    }

                    _clientStorage.UpdateClient(id, ca);

                    Client c = Client.CreateInstance(ca);
                    c.ClientSecret = "*********"; //hide secret
                    return c;
                }
                else
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("Client does not exist", 404)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.NotFound };
                }
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

        public Client GenerateNewSecret(string id)
        {
            //TODO: invalidate existing refresh tokens?
            try
            {
                IClientAccount ca = _clientStorage.GetClient(id);
                if(ca != null)
                {
                    ca.ClientSecret = Guid.NewGuid().ToString();
                    _clientStorage.UpdateClient(id,ca);
                    return Client.CreateInstance(ca);
                }
                else
                {
                    throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage("Client does not exist", 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
                }
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

        public void DeleteClient(string id)
        {
            try
            {
                _clientStorage.DeleteClient(id);
            }
            catch(Exception e)
            {
                throw new HttpRequestException(JsonSerializer.SerializeJson(new ErrorMessage(e.Message, 500)), MimeType.APPLICATION_JSON) { Status = SKotstein.Net.Http.Context.HttpStatus.InternalServerError };
            }
        }
    }
}
