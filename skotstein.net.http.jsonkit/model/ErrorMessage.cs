using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.jsonkit
{
    /// <summary>
    /// Represents a error message where <see cref="StatusCode"/> contains the HTTP status code and <see cref="Message"/> gives further details about the error.
    /// An instance of <see cref="ErrorMessage"/> can be converted into an appropriate JSON structure (use <see cref="JsonSerializer"/>) and returned in the form of the payload of the HTTP response.
    /// </summary>
    public class ErrorMessage
    {
        private int _statusCode;
        private string _message;

        /// <summary>
        /// Gets or sets the HTTP status code (the three digit code) being associated with this error message
        /// </summary>
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

        /// <summary>
        /// Gets or sets the error message giving further details about the error
        /// </summary>
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

        /// <summary>
        /// Initializes an instance of <see cref="ErrorMessage"/>
        /// </summary>
        /// <param name="statusCode">the HTTP status code</param>
        /// <param name="message">error message for further details</param>
        public ErrorMessage(int statusCode, string message)
        {
            _statusCode = statusCode;
            _message = message;
        }

    }
}
