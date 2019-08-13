using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.filestorage
{
    public class UserAccountJson : IUserAccount
    {
        private string _userId;
        private bool _isBlocked;
        private string _password;
        private string _name;
        private string _username;
        private string _scope;
        private string _clientId;
        private string _description;


        [JsonProperty("userId")]
        public string UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                _userId = value;
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

        [JsonProperty("password")]
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }

        [JsonProperty("username")]
        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
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
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
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

        public static UserAccountJson CreateInstance(IUserAccount ua)
        {
            UserAccountJson user = new UserAccountJson();
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
