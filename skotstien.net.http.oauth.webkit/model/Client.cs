using Newtonsoft.Json;
using skotstein.net.http.oauth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class Client : IClientAccount
    {
        public const string CLIENT_ID_PROPERTY = "clientId";
        public const string NAME_PROPERTY = "name";
        public const string DESCRIPTION_PROPERTY = "description";
        public const string CLIENT_SECRET_PROPERTY = "clientSecret";
        public const string SCOPE_PROPOERTY = "scope";
        public const string IS_BLOCKED_PROPERTY = "isBlocked";
        public const string ACCESS_TOKEN_EXPIRY_PROPERTY = "accessTokenExpiry";
        public const string REFRESH_TOKEN_EXPIRY_PROPERTY = "refreshTokenExpiry";
        public const string HAS_USER_PROPERTY = "hasUser";
        public const string IS_CLIENT_ID_REQUIRED_FOR_REFRESH_TOKEN = "isClientIdRequiredForRefreshToken";
        public const string IS_CLIENT_SECRET_REQUIRED_FOR_REFRESH_TOKEN = "isClientSecretRequiredForRefreshToken";
        public const string IS_USER_ID_REQUIRED_FOR_REFRESH_TOKEN = "isUserIdRequiredForRefreshToken";
        public const string IS_USER_PASSWORD_REQUIRED_FOR_REFRESH_TOKEN = "isUserPasswordRequiredForRefreshToken";
        public const string IS_CLIENT_SECRET_REQUIRED_FOR_PASSWORD_GRANT = "isClientSecretRequiredForPasswordGrant";


        private string _clientId;
        private string _clientSecret;
        private string _friendlyName;
        private string _description;
        private string _scope;
        private bool _isBlocked;
        private long _accessTokenExpiryInSeconds;
        private long _refreshTokenExpiryInSeconds;

        private bool _hasUser;
        private bool _isClientIdRequiredForRefreshToken;
        private bool _isClientSecretRequiredForRefreshToken;
        private bool _isUserIdRequiredForRefreshToken;
        private bool _isUserPasswordRequiredForRefreshToken;
        private bool _isClientSecretRequiredForPasswordGrant;
        

        private IDictionary<string, bool> _hasPropertySet = new Dictionary<string, bool>()
        {
            {CLIENT_ID_PROPERTY,false },
            {NAME_PROPERTY,false },
            {DESCRIPTION_PROPERTY,false },
            {CLIENT_SECRET_PROPERTY,false },
            {SCOPE_PROPOERTY,false },
            {IS_BLOCKED_PROPERTY,false },
            {ACCESS_TOKEN_EXPIRY_PROPERTY,false },
            {REFRESH_TOKEN_EXPIRY_PROPERTY,false},
            {HAS_USER_PROPERTY,false},
            {IS_CLIENT_ID_REQUIRED_FOR_REFRESH_TOKEN,false},
            {IS_CLIENT_SECRET_REQUIRED_FOR_REFRESH_TOKEN,false },
            {IS_USER_ID_REQUIRED_FOR_REFRESH_TOKEN,false },
            {IS_USER_PASSWORD_REQUIRED_FOR_REFRESH_TOKEN,false },
            {IS_CLIENT_SECRET_REQUIRED_FOR_PASSWORD_GRANT,false }
        };

        [JsonProperty(CLIENT_ID_PROPERTY)]
        public string ClientId
        {
            get
            {
                return _clientId;
            }

            set
            {
                _clientId = value;
                _hasPropertySet[CLIENT_ID_PROPERTY] = true;
            }
        }


        [JsonProperty(NAME_PROPERTY)]
        public string FriendlyName
        {
            get
            {
                return _friendlyName;
            }

            set
            {
                _friendlyName = value;
                _hasPropertySet[NAME_PROPERTY] = true;
            }
        }

        [JsonProperty(DESCRIPTION_PROPERTY)]
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
                _hasPropertySet[DESCRIPTION_PROPERTY] = true;
            }
        }

        [JsonProperty(CLIENT_SECRET_PROPERTY)]
        public string ClientSecret
        {
            get
            {
                return _clientSecret;
            }

            set
            {
                _clientSecret = value;
                _hasPropertySet[CLIENT_SECRET_PROPERTY] = true;
            }
        }

        [JsonProperty(SCOPE_PROPOERTY)]
        public string Scope
        {
            get
            {
                return _scope;
            }

            set
            {
                _scope = value;
                _hasPropertySet[SCOPE_PROPOERTY] = true;
            }
        }

        [JsonProperty(IS_BLOCKED_PROPERTY)]
        public bool IsBlocked
        {
            get
            {
                return _isBlocked;
            }

            set
            {
                _isBlocked = value;
                _hasPropertySet[IS_BLOCKED_PROPERTY] = true;
            }
        }

        [JsonProperty(ACCESS_TOKEN_EXPIRY_PROPERTY)]
        public long AccessTokenExpiryInSeconds
        {
            get
            {
                return _accessTokenExpiryInSeconds;
            }

            set
            {
                _accessTokenExpiryInSeconds = value;
                _hasPropertySet[ACCESS_TOKEN_EXPIRY_PROPERTY] = true;
            }
        }

        [JsonProperty(REFRESH_TOKEN_EXPIRY_PROPERTY)]
        public long RefreshTokenExpiryInSeconds
        {
            get
            {
                return _refreshTokenExpiryInSeconds;
            }

            set
            {
                _refreshTokenExpiryInSeconds = value;
                _hasPropertySet[REFRESH_TOKEN_EXPIRY_PROPERTY] = true;
            }
        }

        [JsonProperty(HAS_USER_PROPERTY)]
        public bool HasUser
        {
            get
            {
                return _hasUser;
            }

            set
            {
                _hasUser = value;
                _hasPropertySet[HAS_USER_PROPERTY] = true;
            }
        }

        [JsonProperty(IS_CLIENT_ID_REQUIRED_FOR_REFRESH_TOKEN)]
        public bool IsClientIdRequiredForRefreshToken
        {
            get
            {
                return _isClientIdRequiredForRefreshToken;
            }

            set
            {
                _isClientIdRequiredForRefreshToken = value;
                _hasPropertySet[IS_CLIENT_ID_REQUIRED_FOR_REFRESH_TOKEN] = true;
            }
        }

        [JsonProperty(IS_CLIENT_SECRET_REQUIRED_FOR_REFRESH_TOKEN)]
        public bool IsClientSecretRequiredForRefreshToken
        {
            get
            {
                return _isClientSecretRequiredForRefreshToken;
            }

            set
            {
                _isClientSecretRequiredForRefreshToken = value;
                _hasPropertySet[IS_CLIENT_SECRET_REQUIRED_FOR_REFRESH_TOKEN] = true;
            }
        }

        [JsonProperty(IS_USER_ID_REQUIRED_FOR_REFRESH_TOKEN)]
        public bool IsUserIdRequiredForRefreshToken
        {
            get
            {
                return _isUserIdRequiredForRefreshToken;
            }

            set
            {
                _isUserIdRequiredForRefreshToken = value;
                _hasPropertySet["isUserIdRequiredForRefreshToken"] = true;
            }
        }

        [JsonProperty(IS_USER_PASSWORD_REQUIRED_FOR_REFRESH_TOKEN)]
        public bool IsUserPasswordRequiredForRefreshToken
        {
            get
            {
                return _isUserPasswordRequiredForRefreshToken;
            }

            set
            {
                _isUserPasswordRequiredForRefreshToken = value;
                _hasPropertySet[IS_USER_PASSWORD_REQUIRED_FOR_REFRESH_TOKEN] = true;
            }
        }

        [JsonProperty(IS_CLIENT_SECRET_REQUIRED_FOR_PASSWORD_GRANT)]
        public bool IsClientSecretRequiredForPasswordGrant
        {
            get
            {
                return _isClientSecretRequiredForPasswordGrant;
            }

            set
            {
                _isClientSecretRequiredForPasswordGrant = value;
                _hasPropertySet[IS_CLIENT_SECRET_REQUIRED_FOR_PASSWORD_GRANT] = true;
            }
        }

        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public Link ToUsers
        {
            get
            {
                if (_hasUser)
                {
                    return new Link() { Href = ApiBase.API_V1 + "/clients/" + ClientId + "/users" };
                }
                else
                {
                    return null;
                }
            }
        }

     
        [JsonProperty("self")]
        public Link ToSelf
        {
            get
            {
                return new Link() { Href = ApiBase.API_V1 + "/clients/" + ClientId };
            }
        }


        [JsonProperty("changeClient")]
        public ActionLink UpdateClientLink
        {
            get
            {
                return new ActionLink() { Href = ApiBase.API_V1 + "/clients/" + ClientId, Action = "POST" };
            }
        }

        [JsonProperty("newSecret")]
        public ActionLink ChangeSecretLink
        {
            get
            {
                return new ActionLink() { Href = ApiBase.API_V1 + "/clients/" + ClientId+"/secret", Action = "PUT" };
            }
        }

        [JsonProperty("deleteClient")]
        public ActionLink DeleteClientLink
        {
            get
            {
                return new ActionLink() { Href = ApiBase.API_V1 + "/clients/" + ClientId, Action = "DELETE" };
            }
        }




        [JsonIgnore]
        public IList<string> GetScopeAsList
        {
            get
            {
                IList<string> scps = new List<string>();
                ISet<string> set = new HashSet<string>();
                string[] scopes = _scope.Split(' ');
                foreach (string scope in scopes)
                {
                    if (!set.Contains(scope))
                    {
                        set.Add(scope);
                        scps.Add(scope);
                    }

                }
                return scps;
            }
        }

        [JsonIgnore]
        public IDictionary<string, bool> HasPropertySet
        {
            get
            {
                return _hasPropertySet;
            }

            set
            {
                _hasPropertySet = value;
            }
        }

        public static Client CreateInstance(IClientAccount ca)
        {
            Client client = new Client();
            client.ClientId = ca.ClientId;
            client.AccessTokenExpiryInSeconds = ca.AccessTokenExpiryInSeconds;
            client.ClientSecret = ca.ClientSecret;
            client.Description = ca.Description;
            client.FriendlyName = ca.FriendlyName;
            client.HasUser = ca.HasUser;
            client.IsBlocked = ca.IsBlocked;
            client.IsClientIdRequiredForRefreshToken = ca.IsClientIdRequiredForRefreshToken;
            client.IsClientSecretRequiredForPasswordGrant = ca.IsClientSecretRequiredForPasswordGrant;
            client.IsClientSecretRequiredForRefreshToken = ca.IsClientSecretRequiredForRefreshToken;
            client.IsUserIdRequiredForRefreshToken = ca.IsUserIdRequiredForRefreshToken;
            client.IsUserPasswordRequiredForRefreshToken = ca.IsUserPasswordRequiredForRefreshToken;
            client.RefreshTokenExpiryInSeconds = ca.RefreshTokenExpiryInSeconds;
            client.Scope = ca.Scope;
            return client;

        }
     
    }
}
