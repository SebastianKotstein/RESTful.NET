using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.webkit
{
    public class ErrorMessage
    {
        private string _message;
        private int _statusCode;

        public ErrorMessage(string message, int statusCode)
        {
            _message = message;
            _statusCode = statusCode;
        }

        [JsonProperty("message")]
        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
            }
        }

        [JsonProperty("code")]
        public int StatusCode
        {
            get
            {
                return _statusCode;
            }

            set
            {
                _statusCode = value;
            }
        }
    }
}
