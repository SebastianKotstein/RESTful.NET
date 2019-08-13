using Newtonsoft.Json;
using skotstein.net.http.oauth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class User : IUserAccount
    {
        public const string USER_ID_PROPERTY = "userId";
        public const string NAME_PROPERTY = "name";
        public const string DESCRIPTION_PROPERTY = "description";
        public const string IS_BLOCKED_PROPERTY = "isBlocked";
        public const string PASSWORD_PROPERTY = "password";
        public const string USERNAME_PROPERTY = "username";
        public const string CLIENT_ID_PROPERTY = "clientId";
        public const string SCOPE_PROPERTY = "scope";

        private string _userId;
        private string _name;
        private string _description;
        private bool _isBlocked;
        private string _password;
        private string _username;
        private string _clientId;
        private string _scope;

        private IDictionary<string, bool> _hasPropertySet = new Dictionary<string, bool>()
        {
            {USER_ID_PROPERTY,false },
            {NAME_PROPERTY,false },
            {DESCRIPTION_PROPERTY,false },
            {IS_BLOCKED_PROPERTY,false },
            {PASSWORD_PROPERTY,false },
            {USERNAME_PROPERTY,false },
            {CLIENT_ID_PROPERTY,false },
            {SCOPE_PROPERTY,false}
        };

        [JsonProperty(USER_ID_PROPERTY)]
        public string UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                _userId = value;
                _hasPropertySet[USER_ID_PROPERTY] = true;
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

        [JsonProperty(PASSWORD_PROPERTY)]
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                _hasPropertySet[PASSWORD_PROPERTY] = true;
            }
        }

        [JsonProperty(USERNAME_PROPERTY)]
        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
                _hasPropertySet[USERNAME_PROPERTY] = true;
            }
        }

        [JsonProperty(SCOPE_PROPERTY)]
        public string Scope
        {
            get
            {
                return _scope;
            }

            set
            {
                _scope = value;
                _hasPropertySet[SCOPE_PROPERTY] = true;
            }
        }

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

        [JsonProperty(NAME_PROPERTY)]
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
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


        [JsonProperty("self")]
        public Link Self
        {
            get
            {
                return new Link() { Href = ApiBase.API_V1 + "/clients/" + ClientId + "/users/"+UserId };
            }
        }

        [JsonProperty("changeUser")]
        public ActionLink ChangeUserLink
        {
            get
            {
                return new ActionLink() { Href = ApiBase.API_V1 + "/clients/" + ClientId + "/users/"+UserId, Action = "PUT" };
            }
        }

        [JsonProperty("changePassword")]
        public ActionLink ChangePasswordLink
        {
            get
            {
                return new ActionLink() { Href = ApiBase.API_V1 + "/clients/" + ClientId + "/users/" + UserId+"/password", Action = "PUT" };
            }
        }

        [JsonProperty("deleteUser")]
        public ActionLink DeleteUserLink
        {
            get
            {
                return new ActionLink() { Href = ApiBase.API_V1 + "/clients/" + ClientId + "/users/" + UserId, Action = "DELETE" };
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

        public static User CreateInstance(IUserAccount ua)
        {
            User user = new User();
            user.UserId = ua.UserId;
            user.Name = ua.Name;
            user.Description = ua.Description;
            user.IsBlocked = ua.IsBlocked;
            user.Password = ua.Password;
            user.Scope = ua.Scope;
            user.Username = ua.Username;
            return user;
        }
    }
}
