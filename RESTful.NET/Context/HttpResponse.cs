using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Context
{
    /// <summary>
    /// Encapsulates the HTTP-specific information of an HTTP response.
    /// </summary>
    public class HttpResponse
    {
        private HttpPayload _payload;
        private HttpResponseHeaders _headers;
        private int _statusCode;
        private string _statusDescription;

        /// <summary>
        /// constructor
        /// </summary>
        internal HttpResponse()
        {
            _headers = new HttpResponseHeaders();
            _payload = new HttpPayload();
        }

        /// <summary>
        /// Gets the payload instance associated with this response.
        /// </summary>
        public HttpPayload Payload
        {
            get
            {
                return _payload;
            }
            internal set
            {
                _payload = value;
            }
        }

        /// <summary>
        /// Gets the header instance containing all headers associated with this response.
        /// </summary>
        public HttpResponseHeaders Headers
        {
            get
            {
                return _headers;
            }
        }

        /// <summary>
        /// Gets or sets the status code associated with this response.
        /// </summary>
        public int StatusCode
        {
            get
            {
                
                return _statusCode;
            }
            set
            {
                value = _statusCode;
            }
        }

        /// <summary>
        /// Gets or sets the status description associated with this response.
        /// </summary>
        public string StatusDescription
        {
            get
            {
                return _statusDescription;
            }
            set
            {
                value = _statusDescription;
            }
        }

        /// <summary>
        /// Transforms the status code and status description into a string having the format CODE - DESCRIPTION, e.g. 200 - OK.
        /// </summary>
        public string StatusAsString
        {
            get
            {
                return _statusCode + " - " + _statusDescription;   
            }
        }

        /// <summary>
        /// Returns the status associated with this response as an <see cref="HttpStatus"/> enum.
        /// </summary>
        public HttpStatus Status
        {
            get
            {
                switch (_statusCode)
                {
                    //100er
                    case 100:
                        return HttpStatus.Continue;
                    case 101:
                        return HttpStatus.SwitchingProtocols;
                    case 102:
                        return HttpStatus.Processing;

                    //200er
                    case 200:
                        return HttpStatus.OK;
                    case 201:
                        return HttpStatus.Created;
                    case 202:
                        return HttpStatus.Accepted;
                    case 203:
                        return HttpStatus.NonAuthoratitiveInformation;
                    case 204:
                        return HttpStatus.NoContent;
                    case 205:
                        return HttpStatus.ResetContent;
                    case 206:
                        return HttpStatus.PartialContent;
                    case 207:
                        return HttpStatus.MultiStatus;
                    case 208:
                        return HttpStatus.AlreadyReported;
                    case 226:
                        return HttpStatus.IMUsed;
                
                    //300er
                    case 300:
                        return HttpStatus.MultipleChoices;
                    case 301:
                        return HttpStatus.MovedPermanently;
                    case 302:
                        return HttpStatus.Found;
                    case 303:
                        return HttpStatus.SeeOther;
                    case 304:
                        return HttpStatus.NotModified;
                    case 305:
                        return HttpStatus.UseProxy;
                    case 307:
                        return HttpStatus.TemporaryRedirect;
                    
                    //400er
                    case 400:
                        return HttpStatus.BadRequest;
                    case 401:
                        return HttpStatus.Unauthorized;
                    case 402:
                        return HttpStatus.PaymentRequired;
                    case 403:
                        return HttpStatus.Forbidden;
                    case 404:
                        return HttpStatus.NotFound;
                    case 405:
                        return HttpStatus.MethodNotAllowed;
                    case 406:
                        return HttpStatus.NotAcceptable;
                    case 407:
                        return HttpStatus.ProxyAuthenticationRequired;
                    case 408:
                        return HttpStatus.RequestTimeOut;
                    case 409:
                        return HttpStatus.Conflict;
                    case 410:
                        return HttpStatus.Gone;
                    case 411:
                        return HttpStatus.LengthRequired;
                    case 412:
                        return HttpStatus.PreconditionFailed;
                    case 413:
                        return HttpStatus.RequestEntityTooLarge;
                    case 414:
                        return HttpStatus.RequestURLTooLong;
                    case 415:
                        return HttpStatus.UnsupportedMediaType;
                    case 416:
                        return HttpStatus.RequestedRangeNotSatisfiable;
                    case 417:
                        return HttpStatus.ExceptionFailed;
                    case 418:
                        return HttpStatus.ImATeapot;
                    case 420:
                        return HttpStatus.PolicyNotFulfilled;
                    case 421:
                        return HttpStatus.MisdirectedRequest;
                    case 422:
                        return HttpStatus.UnprocessableEntity;
                    case 423:
                        return HttpStatus.Locked;
                    case 424:
                        return HttpStatus.FailedDependency;
                    case 425:
                        return HttpStatus.UnorderedCollection;
                    case 426:
                        return HttpStatus.UpgradeRequired;
                    case 428:
                        return HttpStatus.PreconditionRequired;
                    case 429:
                        return HttpStatus.TooManyRequests;
                    case 431:
                        return HttpStatus.RequestHeaderFieldsTooLarge;
                    case 451:
                        return HttpStatus.UnavailableForLegalReasons;
                    
                    //500er
                    case 500:
                        return HttpStatus.InternalServerError;
                    case 501:
                        return HttpStatus.NotImplemented;
                    case 502:
                        return HttpStatus.BadGateway;
                    case 503:
                        return HttpStatus.ServiceUnavailable;
                    case 504:
                        return HttpStatus.GatewayTimeOut;
                    case 505:
                        return HttpStatus.HTTPVersionNotSupported;
                    case 506:
                        return HttpStatus.VariantAlsoNegotiates;
                    case 507:
                        return HttpStatus.InsufficientStorage;
                    case 508:
                        return HttpStatus.LoopDetected;
                    case 509:
                        return HttpStatus.BandwithLimitExceeded;
                    case 510:
                        return HttpStatus.NotExtended;
                    case 511:
                        return HttpStatus.NetworkAuthenticationRequired;
                    default:
                        return HttpStatus._ProprietaryStatusCode;
                }
                
            }
            set
            {
                switch (value)
                {
                    //100er
                    case HttpStatus.Continue:
                        _statusCode = 100;
                        _statusDescription = "Continue";
                        break;
                    case HttpStatus.SwitchingProtocols:
                        _statusCode = 101;
                        _statusDescription = "Switching Protocols";
                        break;
                    case HttpStatus.Processing:
                        _statusCode = 102;
                        _statusDescription = "Processing";
                        break;

                    //200er
                    case HttpStatus.OK:
                        _statusCode = 200;
                        _statusDescription = "OK";
                        break;
                    case HttpStatus.Created:
                        _statusCode = 201;
                        _statusDescription = "Created";
                        break;
                    case HttpStatus.Accepted:
                        _statusCode = 202;
                        _statusDescription = "Accepted";
                        break;
                    case HttpStatus.NonAuthoratitiveInformation:
                        _statusCode = 203;
                        _statusDescription = "Non-Authoratitive Information";
                        break;
                    case HttpStatus.NoContent:
                        _statusCode = 204;
                        _statusDescription = "No Content";
                        break;
                    case HttpStatus.ResetContent:
                        _statusCode = 205;
                        _statusDescription = "Reset Content";
                        break;
                    case HttpStatus.PartialContent:
                        _statusCode = 206;
                        _statusDescription = "Partial Content";
                        break;
                    case HttpStatus.MultiStatus:
                        _statusCode = 207;
                        _statusDescription = "Multi-Status";
                        break;
                    case HttpStatus.AlreadyReported:
                        _statusCode = 208;
                        _statusDescription = "Already Reported";
                        break;
                    case HttpStatus.IMUsed:
                        _statusCode = 226;
                        _statusDescription = "IM Used";
                        break;

                    //300er
                    case HttpStatus.MultipleChoices:
                        _statusCode = 300;
                        _statusDescription = "Multiple Choices";
                        break;
                    case HttpStatus.MovedPermanently:
                        _statusCode = 301;
                        _statusDescription = "Moved Permanently";
                        break;
                    case HttpStatus.Found:
                        _statusCode = 302;
                        _statusDescription = "Found";
                        break;
                    case HttpStatus.SeeOther:
                        _statusCode = 303;
                        _statusDescription = "See Other";
                        break;
                    case HttpStatus.NotModified:
                        _statusCode = 304;
                        _statusDescription = "Not Modified";
                        break;
                    case HttpStatus.UseProxy:
                        _statusCode = 305;
                        _statusDescription = "Use Proxy";
                        break;
                    case HttpStatus.TemporaryRedirect:
                        _statusCode = 307;
                        _statusDescription = "Temporary Redirect";
                        break;
                    case HttpStatus.PermanentRedirect:
                        _statusCode = 308;
                        _statusDescription = "Permanent Redirect";
                        break;

                    //400er
                    case HttpStatus.BadRequest:
                        _statusCode = 400;
                        _statusDescription = "Bad Request";
                        break;
                    case HttpStatus.Unauthorized:
                        _statusCode = 401;
                        _statusDescription = "Unauthorized";
                        break;
                    case HttpStatus.PaymentRequired:
                        _statusCode = 402;
                        _statusDescription = "Payment Required";
                        break;
                    case HttpStatus.Forbidden:
                        _statusCode = 403;
                        _statusDescription = "Forbidden";
                        break;
                    case HttpStatus.NotFound:
                        _statusCode = 404;
                        _statusDescription = "Not Found";
                        break;
                    case HttpStatus.MethodNotAllowed:
                        _statusCode = 405;
                        _statusDescription = "Method Not Allowed";
                        break;
                    case HttpStatus.NotAcceptable:
                        _statusCode = 406;
                        _statusDescription = "Not Acceptable";
                        break;
                    case HttpStatus.ProxyAuthenticationRequired:
                        _statusCode = 407;
                        _statusDescription = "Proxy Authentication Required";
                        break;
                    case HttpStatus.RequestTimeOut:
                        _statusCode = 408;
                        _statusDescription = "Request Time-out";
                        break;
                    case HttpStatus.Conflict:
                        _statusCode = 409;
                        _statusDescription = "Conflict";
                        break;
                    case HttpStatus.Gone:
                        _statusCode = 410;
                        _statusDescription = "Gone";
                        break;
                    case HttpStatus.LengthRequired:
                        _statusCode = 411;
                        _statusDescription = "Length Required";
                        break;
                    case HttpStatus.PreconditionFailed:
                        _statusCode = 412;
                        _statusDescription = "Precondition Failed";
                        break;
                    case HttpStatus.RequestEntityTooLarge:
                        _statusCode = 413;
                        _statusDescription = "Request Entity Too Large";
                        break;
                    case HttpStatus.RequestURLTooLong:
                        _statusCode = 414;
                        _statusDescription = "Request-URL Too Long";
                        break;
                    case HttpStatus.UnsupportedMediaType:
                        _statusCode = 415;
                        _statusDescription = "Unsupported Media Type";
                        break;
                    case HttpStatus.RequestedRangeNotSatisfiable:
                        _statusCode = 416;
                        _statusDescription = "Requested range not satisfiable";
                        break;
                    case HttpStatus.ExceptionFailed:
                        _statusCode = 417;
                        _statusDescription = "Exception Failed";
                        break;
                    case HttpStatus.ImATeapot:
                        _statusCode = 418;
                        _statusDescription = "I'm a teapot";
                        break;
                    case HttpStatus.PolicyNotFulfilled:
                        _statusCode = 420;
                        _statusDescription = "Policy Not Fulfilled";
                        break;
                    case HttpStatus.MisdirectedRequest:
                        _statusCode = 421;
                        _statusDescription = "Misdirected Request";
                        break;
                    case HttpStatus.UnprocessableEntity:
                        _statusCode = 422;
                        _statusDescription = "Unprocessable Entity";
                        break;
                    case HttpStatus.Locked:
                        _statusCode = 423;
                        _statusDescription = "Locked";
                        break;
                    case HttpStatus.FailedDependency:
                        _statusCode = 424;
                        _statusDescription = "Failed Dependency";
                        break;
                    case HttpStatus.UnorderedCollection:
                        _statusCode = 425;
                        _statusDescription = "Unordered Collection";
                        break;
                    case HttpStatus.UpgradeRequired:
                        _statusCode = 426;
                        _statusDescription = "Upgrade Required";
                        break;
                    case HttpStatus.PreconditionRequired:
                        _statusCode = 428;
                        _statusDescription = "Precondition Required";
                        break;
                    case HttpStatus.TooManyRequests:
                        _statusCode = 429;
                        _statusDescription = "Too Many Requests";
                        break;
                    case HttpStatus.RequestHeaderFieldsTooLarge:
                        _statusCode = 431;
                        _statusDescription = "Request Header Fields Too Large";
                        break;
                    case HttpStatus.UnavailableForLegalReasons:
                        _statusCode = 451;
                        _statusDescription = "Unavailable For Legal Reasons";
                        break;

                    //500er
                    case HttpStatus.InternalServerError:
                        _statusCode = 500;
                        _statusDescription = "Internal Server Error";
                        break;
                    case HttpStatus.NotImplemented:
                        _statusCode = 501;
                        _statusDescription = "Not Implemented";
                        break;
                    case HttpStatus.BadGateway:
                        _statusCode = 502;
                        _statusDescription = "Bad Gateway";
                        break;
                    case HttpStatus.ServiceUnavailable:
                        _statusCode = 503;
                        _statusDescription = "Service Unavailable";
                        break;
                    case HttpStatus.GatewayTimeOut:
                        _statusCode = 504;
                        _statusDescription = "Gateway Time-out";
                        break;
                    case HttpStatus.HTTPVersionNotSupported:
                        _statusCode = 505;
                        _statusDescription = "HTTP Version not supported";
                        break;
                    case HttpStatus.VariantAlsoNegotiates:
                        _statusCode = 506;
                        _statusDescription = "Variant Also Negotiates";
                        break;
                    case HttpStatus.InsufficientStorage:
                        _statusCode = 507;
                        _statusDescription = "Insufficient Storage";
                        break;
                    case HttpStatus.LoopDetected:
                        _statusCode = 508;
                        _statusDescription = "Loop Detected";
                        break;
                    case HttpStatus.BandwithLimitExceeded:
                        _statusCode = 509;
                        _statusDescription = "Bandwith Limit Exceeded";
                        break;
                    case HttpStatus.NotExtended:
                        _statusCode = 510;
                        _statusDescription = "Not Extended";
                        break;
                    case HttpStatus.NetworkAuthenticationRequired:
                        _statusCode = 511;
                        _statusDescription = "Network Authentication Required";
                        break;
                    case HttpStatus._ProprietaryStatusCode:
                        throw new Exception("Setting _ProprietaryStatusCode is not allowed");
                    
                }
            }
        }
    }

    /// <summary>
    /// Enumeration of the HTTP status
    /// </summary>
    public enum HttpStatus : int
    {
        Continue = 100,
        SwitchingProtocols = 101,
        Processing = 102,

        OK = 200,
        Created = 201,
        Accepted = 202,
        NonAuthoratitiveInformation = 203,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,
        MultiStatus = 207,
        AlreadyReported = 208,
        IMUsed = 226,

        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        TemporaryRedirect = 307,
        PermanentRedirect = 308,

        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeOut = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestEntityTooLarge = 413,
        RequestURLTooLong = 414,
        UnsupportedMediaType = 415,
        RequestedRangeNotSatisfiable = 416,
        ExceptionFailed = 417,
        ImATeapot = 418,
        PolicyNotFulfilled = 420,
        MisdirectedRequest = 421,
        UnprocessableEntity = 422,
        Locked = 423,
        FailedDependency = 424,
        UnorderedCollection = 425,
        UpgradeRequired = 426,
        PreconditionRequired = 428,
        TooManyRequests = 429,
        RequestHeaderFieldsTooLarge = 431,
        UnavailableForLegalReasons = 451,

        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeOut = 504,
        HTTPVersionNotSupported = 505,
        VariantAlsoNegotiates = 506,
        InsufficientStorage = 507,
        LoopDetected = 508,
        BandwithLimitExceeded = 509,
        NotExtended = 510,
        NetworkAuthenticationRequired = 511,

        _ProprietaryStatusCode = 0

    }
}
