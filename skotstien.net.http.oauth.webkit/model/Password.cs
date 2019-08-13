using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class Password
    {
        private string _oldPassword;
        private string _newPassword;

        [JsonProperty("oldPassword")]
        public string OldPassword
        {
            get
            {
                return _oldPassword;
            }

            set
            {
                _oldPassword = value;
            }
        }

        [JsonProperty("newPassword")]
        public string NewPassword
        {
            get
            {
                return _newPassword;
            }

            set
            {
                _newPassword = value;
            }
        }
    }
}
