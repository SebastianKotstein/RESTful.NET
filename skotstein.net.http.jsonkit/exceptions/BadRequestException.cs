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
    /// The exception that is thrown when a request does not meet the expectations of the service. 
    /// A thrown <see cref="BadRequestException"/> is automatically catched by the underlying RESTful.NET Framework and converted into an appropriate HTTP Reponse (i.e. there is not need to catch this exception
    /// before sending an HTTP Response). The payload of the HTTP Response contains a JSON structure whith detail information. 
    /// Use the constructor <see cref="BadRequestException.BadRequestException(string)"/> to create a new instance where the HTTP Response
    /// will have a JSON structure typeof <see cref="ErrorMessage"/>, or use <see cref="BadRequestException.BadRequestException(object)"/> for setting your own JSON type.
    /// </summary>
    public class BadRequestException : HttpRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/>. The passed message (string) is embedded into a JSON structure typeof <see cref="ErrorMessage"/> which is the payload of the HTTP Response.
        /// </summary>
        /// <param name="message">the error message which is embedded into a JSON structure type of <see cref="ErrorMessage"/></param>
        public BadRequestException(string message)
        {
            this.ErrorMessage = JsonSerializer.SerializeJson(new ErrorMessage(400, message)) ?? throw new HttpRequestException("Cannot serialize 'ErrorMessage' into JSON", MimeType.TEXT_PLAN) { Status = HttpStatus.InternalServerError };
            this.ContentType = MimeType.APPLICATION_JSON;
            this.Status = HttpStatus.BadRequest;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/>. The passed object is converted into an appropriate JSON structure and embedded into the payload of the HTTP Response.
        /// </summary>
        /// <param name="message">the error message object which is converted into JSON</param>
        public BadRequestException(object message)
        {
            this.ErrorMessage = JsonSerializer.SerializeJson(message) ?? throw new HttpRequestException("Cannot serialize '"+message.GetType().FullName+"' into JSON", MimeType.TEXT_PLAN) { Status = HttpStatus.InternalServerError }; ;
            this.ContentType = MimeType.APPLICATION_JSON;
            this.Status = HttpStatus.BadRequest;
        }
    }
}
