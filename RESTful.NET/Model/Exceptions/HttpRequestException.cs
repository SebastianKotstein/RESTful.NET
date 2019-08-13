using SKotstein.Net.Http.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Model.Exceptions
{
    /// <summary>
    /// The exception is thrown if an error while processing an HTTP request is encountered
    /// </summary>
    public class HttpRequestException : Exception
    {
        private HttpStatus _status = HttpStatus.InternalServerError;
        private string _errorMessage;
        private string _contentType;

        public HttpRequestException() : this(null,null)
        {
            
        }

        public HttpRequestException(string errorMessage, string contentType)
        {
            _errorMessage = errorMessage;
            _contentType = contentType;
        }

        public HttpStatus Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
            }
        }

        public string ContentType
        {
            get
            {
                return _contentType;
            }

            set
            {
                _contentType = value;
            }
        }
    }
}
