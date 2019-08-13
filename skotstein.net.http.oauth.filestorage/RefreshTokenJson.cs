using Newtonsoft.Json;
using skotstein.net.http.oauth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.filestorage
{
    public class RefreshTokenJson : IRefreshToken
    {

        private string _refreshToken;
        private string _subject;
        private string _clientId;
        private long _validUntil;
        private string _scope;
        private bool _isInvalidated;

        [JsonProperty("refreshToken")]
        public string RefreshToken
        {
            get
            {
                return _refreshToken;
            }

            set
            {
                _refreshToken = value;
            }
        }

        [JsonProperty("subject")]
        public string Subject
        {
            get
            {
                return _subject;
            }

            set
            {
                _subject = value;
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


        [JsonProperty("validUntil")]
        public long ValidUntil
        {
            get
            {
                return _validUntil;
            }

            set
            {
                _validUntil = value;
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

        [JsonProperty("isInvalidated")]
        public bool IsInvalidated
        {
            get
            {
                return _isInvalidated;
            }

            set
            {
                _isInvalidated = value;   
            }
        }

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
    }
}
