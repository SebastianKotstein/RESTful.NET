using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.filestorage
{
    public class ClientAccountJson : IClientAccount
    {
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


        [JsonProperty("clientId")]
        public string ClientId
        {
            get
            {
                return _clientId;
            }

            set
            {
                _clientId = value;
            }
        }

        [JsonProperty("clientSecret")]
        public string ClientSecret
        {
            get
            {
                return _clientSecret;
            }

            set
            {
                _clientSecret = value;
            }
        }

        [JsonProperty("scope")]
        public string Scope
        {
            get
            {
                return _scope;
            }

            set
            {
                _scope = value;
            }
        }

        [JsonProperty("isBlocked")]
        public bool IsBlocked
        {
            get
            {
                return _isBlocked;
            }

            set
            {
                _isBlocked = value;
            }
        }

        [JsonProperty("accessTokenExpiry")]
        public long AccessTokenExpiryInSeconds
        {
            get
            {
                return _accessTokenExpiryInSeconds;
            }

            set
            {
                _accessTokenExpiryInSeconds = value;
            }
        }

        [JsonProperty("refreshTokenExpiry")]
        public long RefreshTokenExpiryInSeconds
        {
            get
            {
                return _refreshTokenExpiryInSeconds;
            }

            set
            {
                _refreshTokenExpiryInSeconds = value;
            }
        }

        [JsonProperty("hasUser")]
        public bool HasUser
        {
            get
            {
                return _hasUser;
            }

            set
            {
                _hasUser = value;
            }
        }

        [JsonProperty("isClientIdRequiredForRefreshToken")]
        public bool IsClientIdRequiredForRefreshToken
        {
            get
            {
                return _isClientIdRequiredForRefreshToken;
            }

            set
            {
                _isClientIdRequiredForRefreshToken = value;
            }
        }

        [JsonProperty("isClientSecretRequiredForRefreshToken")]
        public bool IsClientSecretRequiredForRefreshToken
        {
            get
            {
                return _isClientSecretRequiredForRefreshToken;
            }

            set
            {
                _isClientSecretRequiredForRefreshToken = value;
            }
        }

        [JsonProperty("isUserIdRequiredForRefreshToken")]
        public bool IsUserIdRequiredForRefreshToken
        {
            get
            {
                return _isUserIdRequiredForRefreshToken;
            }

            set
            {
                _isUserIdRequiredForRefreshToken = value;
            }
        }

        [JsonProperty("isUserPasswordRequiredForRefreshToken")]
        public bool IsUserPasswordRequiredForRefreshToken
        {
            get
            {
                return _isUserPasswordRequiredForRefreshToken;
            }

            set
            {
                _isUserPasswordRequiredForRefreshToken = value;
            }
        }

        [JsonProperty("isClientSecretRequiredForPasswordGrant")]
        public bool IsClientSecretRequiredForPasswordGrant
        {
            get
            {
                return _isClientSecretRequiredForPasswordGrant;
            }

            set
            {
                _isClientSecretRequiredForPasswordGrant = value;
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

        [JsonProperty("name")]
        public string FriendlyName
        {
            get
            {
                return _friendlyName;
            }

            set
            {
                _friendlyName = value;
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public static ClientAccountJson CreateInstance(IClientAccount ca)
        {
            ClientAccountJson client = new ClientAccountJson();
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
