using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Attributes
{
    /// <summary>
    /// The <see cref="ContentType"/> attribute can be optionally applied to a method representing a REST function (see <see cref="Path"/> attribute for declaring a method as a REST function).
    /// By applying this attribute the Content-Type header field of the HTTP response is automatically set with the specified value, but only if the HTTP response contains an entity body (payload) and the header field
    /// is not manually overwritten within the mapped method. Furthermore, if the <see cref="CharsetValue"/> is set to one of the Charsets in <see cref="Charset"/>, the encoding method of the payload (see <see cref="SKotstein.Net.Http.Context.HttpPayload.DefaultEncoding"/>) will automatically set to the specified value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ContentType : System.Attribute
    {
        private string _contentType = null;
        private string _charset = null;

        /// <summary>
        /// Creates a Content-Type attribute with its Content-Type value.
        /// </summary>
        /// <param name="contentType"></param>
        public ContentType(string contentType)
        {
            _contentType = contentType;
        }

        /// <summary>
        /// Creates a Content-Type attribute with its Content-Type value and the Charset value.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="charset"></param>
        public ContentType(string contentType, string charset) : this(contentType)
        {
            _charset = charset;
        }

        /// <summary>
        /// Gets the Content-Type value.
        /// </summary>
        public string ContentTypeValue
        {
            get
            {
                return _contentType;
            }
        }

        /// <summary>
        /// Gets the Charset value.
        /// </summary>
        public string CharsetValue
        {
            get
            {
                return _charset;
            }
        }
    }

    /// <summary>
    /// Encapsulates a list of supported charsets such that <see cref="SKotstein.Net.Http.Context.HttpPayload.DefaultEncoding"/> can be set.
    /// </summary>
    public class Charset
    {
        public const string UTF7 = "charset=utf-7";
        public const string UTF8 = "charset=utf-8";
        //public const string UTF16 = "charset=utf-16";
        public const string UTF32 = "charset=utf-32";

        public const string ASCII = "charset=ascii";

        //public const string ISO88591 = "charset=ISO-8859-1";

    }
}
