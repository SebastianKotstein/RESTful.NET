using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.jsonkit
{
    /// <summary>
    /// The exception that is thrown when a requested resource cannot be found. 
    /// A thrown <see cref="ResourceNotFoundException"/> is automatically catched by the underlying
    /// RESTful.NET Framework and converted into an appropriate HTTP Response (i.e. there is no need to catch this exception before sending an HTTP Response).
    /// The payload of the HTTP Response contains a JSON structure with detail information.
    /// Use the constructor <see cref="ResourceNotFoundException.ResourceNotFoundException(string)"/> to create a new instance where the HTTP Response
    /// will have a JSON structure typeof <see cref="ErrorMessage"/>, or use <see cref="ResourceNotFoundException.ResourceNotFoundException(object)"/> for setting your own JSON type.
    /// </summary>
    public class ResourceNotFoundException : HttpRequestException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException"/>. The passed message (string) is embedded into a JSON structure typeof <see cref="ErrorMessage"/> which is the payload of the HTTP Response.
        /// </summary>
        /// <param name="message">the error message</param>
        public ResourceNotFoundException(string message)
        {
            this.ErrorMessage = JsonSerializer.SerializeJson(new ErrorMessage(404, message)) ?? throw new HttpRequestException("Cannot serialize 'ErrorMessage' into JSON", MimeType.TEXT_PLAN) { Status = HttpStatus.InternalServerError };
            this.ContentType = MimeType.APPLICATION_JSON;
            this.Status = HttpStatus.NotFound;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException"/>. The passed object is converted into an appropriate JSON structure and embedded into the payload of the HTTP Response.
        /// </summary>
        /// <param name="message">the error message object which is converted into JSON</param>
        public ResourceNotFoundException(object message)
        {
            this.ErrorMessage = JsonSerializer.SerializeJson(message) ?? throw new HttpRequestException("Cannot serialize '" + message.GetType().FullName + "' into JSON", MimeType.TEXT_PLAN) { Status = HttpStatus.InternalServerError }; ;
            this.ContentType = MimeType.APPLICATION_JSON;
            this.Status = HttpStatus.NotFound;
        }
    }
}
