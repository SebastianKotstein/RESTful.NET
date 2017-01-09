using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Context
{
    /// <summary>
    /// Represents the entity body so-called payload of an <see cref="HttpRequest"/> or <see cref="HttpResponse"/>. 
    /// Note that the stored content is written into a buffer before sending. The process of sending is handled by the framework automatically. 
    /// </summary>
    public class HttpPayload
    {
        private MemoryStream _stream = new MemoryStream();
        private Encoding _defaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Constructor
        /// </summary>
        internal HttpPayload()
        {
            
        }

        /// <summary>
        /// Gets or sets the default encoding for a stored string.
        /// </summary>
        public Encoding DefaultEncoding
        {
            get
            {
                return _defaultEncoding;
            }
            set
            {
                _defaultEncoding = value;
            }
        }

        /// <summary>
        /// Returns the lenght in byte of the stored content.
        /// </summary>
        public long Length
        {
            get
            {
                return _stream.Length;
            }
        }

        /// <summary>
        /// Reads the whole content of the payload, decodes it by the chosen <see cref="DefaultEncoding"/> method and returns it. 
        /// </summary>
        /// <returns>content as a string</returns>
        public string ReadAll()
        {
            return ReadAll(_defaultEncoding);
        }

        /// <summary>
        /// Reads the whole content of the payload, decodes it by the specified decoding method and returns it. 
        /// </summary>
        /// <param name="decoding">decoding method</param>
        /// <returns>content as a string</returns>
        public string ReadAll(Encoding decoding)
        {
            return Read(0, (int)_stream.Length, decoding);
        }

        /// <summary>
        /// Reads a block of the payload specified by the position and length (count), decodes it by the specified decoding method and returns it. 
        /// </summary>
        /// <param name="position">start position</param>
        /// <param name="count">length</param>
        /// <returns>content as a string</returns>
        public string Read(int position, int count)
        {
            return Read(position, count, _defaultEncoding);
        }

        /// <summary>
        ///  Reads a block of the payload specified by the position and length (count), decodes it by the specified decoding method and returns it. 
        /// </summary>
        /// <param name="position">start position</param>
        /// <param name="count">length</param>
        /// <param name="decodes">decoding method</param>
        /// <returns>content as a string</returns>
        public string Read(int position, int count, Encoding decodes)
        {
            return decodes.GetString(ReadBytes(position, count));
        }

        /// <summary>
        /// Reads a single byte at the specified position and returns it.
        /// </summary>
        /// <param name="position">position</param>
        /// <returns>byte</returns>
        public byte ReadByte(int position)
        {
            byte[] data = new byte[1];
            return ReadBytes(position, 1)[0];
        }

        /// <summary>
        /// Returns the whole content of the payload.
        /// </summary>
        /// <returns>content of payload</returns>
        public byte[] ReadAllBytes()
        {
            byte[] data = new byte[_stream.Length];
            return ReadBytes(0, (int)_stream.Length);
        }

        /// <summary>
        /// Reads a block of the payload specified by the position and the length (count) and returns it as a byte[].
        /// </summary>
        /// <param name="position">position</param>
        /// <param name="count">lenght</param>
        /// <returns>byte[] with content</returns>
        public byte[] ReadBytes(int position, int count)
        {
            byte[] data = new byte[count];
            _stream.Position = position;
            _stream.Read(data, 0, count);
            return data;
        }

        /// <summary>
        /// Encodes a string by the default encoding method and appends it to the payload.
        /// </summary>
        /// <param name="value"></param>
        public void Write(string value)
        {
            Write(value, _defaultEncoding);
        }

        /// <summary>
        /// Encodes a string by the specified encoding method and appends it to the payload.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        public void Write(string value, Encoding encoding)
        {
            WriteBytes(encoding.GetBytes(value));
        }

        /// <summary>
        /// Appends the specified byte[] to the payload.
        /// </summary>
        /// <param name="values">byte[]</param>
        public void WriteBytes(byte[] values)
        {
            _stream.Position = _stream.Length;
            _stream.Write(values, 0, values.Length);
        }

        /// <summary>
        /// Appends a single byte to the payload.
        /// </summary>
        /// <param name="value">byte</param>
        public void WriteByte(byte value)
        {
            _stream.Position = _stream.Length;
            _stream.WriteByte(value);
        }

        /// <summary>
        /// Writes the buffer of this payload to another stream.
        /// </summary>
        /// <param name="stream"></param>
        internal void WriteTo(Stream stream)
        {
            _stream.WriteTo(stream);
        }

        /// <summary>
        /// Reads the passed stream and appends it content to this payload.
        /// </summary>
        /// <param name="stream">source stream</param>
        /// <param name="length">length of the source stream</param>
        internal void ReadFrom(Stream stream, int length)
        {
            byte[] data = new byte[length];
            stream.Read(data, 0, length);
            WriteBytes(data);
        }

        /// <summary>
        /// Clears the whole payload.
        /// </summary>
        public void ClearAll()
        {
            _stream.Dispose();
            _stream = new MemoryStream();
        }

    }

    /// <summary>
    /// Covers MIME-Types (Multipurpose Internet Mail Extensions) which are typically used in course of RESTful services.
    /// Use those MIME-Types for specifying the Content-Type header via the <see cref="SKotstein.Net.Http.Attributes.ContentType"/> attribute or by setting the header field manually via <see cref="HttpResponseHeaders"/>.
    /// </summary>
    public class MimeType
    {
        public const string APPLICATION = "application";
        public const string AUDIO = "audio";
        public const string EXAMPLE = "example";
        public const string IMAGE = "image";
        public const string MESSAGE = "message";
        public const string MODEL = "model";
        public const string MULTIPART = "multipart";
        public const string TEXT = "text";
        public const string VIDEO = "video";

        public const string APPLICATION_JAVASCRIPT = APPLICATION + "/javascript";
        public const string APPLICATION_JSON = APPLICATION + "/json";
        public const string APPLICATION_GZIP = APPLICATION + "/gzip";
        public const string APPLICATION_XHTML_XML = APPLICATION + "/xhtml+xml";
        public const string APPLICATION_XML = APPLICATION + "/xml";
        public const string APPLICATION_ZIP = APPLICATION + "/zip";

        public const string IMAGE_GIF = IMAGE + "/gif";
        public const string IMAGE_JPEG = IMAGE + "/jpeg";
        public const string IMAGE_PNG = IMAGE + "/png";

        public const string MESSAGE_HTTP = MESSAGE + "/http";

        public const string TEXT_CSV = TEXT + "/comma-separated-values";
        public const string TEXT_CSS = TEXT + "/css";
        public const string TEXT_HTML = TEXT + "/html";
        public const string TEXT_JAVASCRIPT = TEXT + "/javascript";
        public const string TEXT_XML = TEXT + "/xml";
        public const string TEXT_PLAN = TEXT + "/plain";
        
    }
}
