using SKotstein.Net.Http.Context;

using SKotstein.Net.Http.Pipelining.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Core
{
    /// <summary>
    /// An implementation of this abstract class is responsible for accepting and parsing incoming HTTP requests.
    /// </summary>
    public abstract class HttpInboundAdapter : SingleInboundAdapter<HttpContext>
    {
        public const string SCHEMA_HTTP = "http";
        public const string SCHEMA_HTTPS = "https";

        private string _schema;
        private int _port;
        private string _host;

        /// <summary>
        /// Gets the prefix having the format SCHEMA://HOST:PORT/, e.g. http://www.example.com:8080/
        /// </summary>
        public string Prefix
        {
            get
            {
                return @_schema+"://"+_host+":"+_port+"/";
            }
        }

        /// <summary>
        /// Gets or sets the schema (protocol) for this inbound adapter. Note that only "http" (see <see cref="SCHEMA_HTTP"/>) and "https" (see <see cref="SCHEMA_HTTPS"/>) is allowerd.
        /// </summary>
        public string Schema
        {
            get
            {
                return _schema;
            }
            set
            {
                _schema = value;
            }
        }

        /// <summary>
        /// Gets or sets the network port the inbound adapter is listening on.
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        /// <summary>
        /// Gets or sets the host part of the prefix. This can be a domain name (FQDN) or an IP address.
        /// </summary>
        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
            }
        }

    }
}
