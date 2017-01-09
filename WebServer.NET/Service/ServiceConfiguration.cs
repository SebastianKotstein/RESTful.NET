namespace SKotstein.Net.Http.Service
{
    public class ServiceConfiguration
    {
        private bool _isSecured = false;
        private int _port = 0;
        private string _host = "";

        private TracingMode _allowTracing = TracingMode.AUTO;
        private OptionsMode _allowOptions = OptionsMode.AUTO;
        private CorsMode _cors = CorsMode.ONLY_IF_ORIGIN_IS_SET;
        private AllowedOriginMode _allowedOrigin = AllowedOriginMode.ANY;
        private AllowedHeadersMode _allowedHeadersMode = AllowedHeadersMode.ANY;
        private string _allowedHeaders = "accept, content-type";
        

        public bool IsSecured
        {
            get
            {
                return _isSecured;
            }
            set
            {
                _isSecured = value;
            }
        }

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

        public TracingMode AllowTracing
        {
            get
            {
                return _allowTracing;
            }
            set
            {
                _allowTracing = value;
            }
        }

        public OptionsMode AllowOptions
        {
            get
            {
                return _allowOptions;
            }
            set
            {
                _allowOptions = value;
            }
        }

        public CorsMode Cors
        {
            get
            {
                return _cors;
            }
            set
            {
                _cors = value;
            }
        }

        public AllowedOriginMode AllowedOrigin
        {
            get
            {
                return _allowedOrigin;
            }
            set
            {
                _allowedOrigin = value;
            }
        }

        public AllowedHeadersMode AllowedHeaders
        {
            get
            {
                return _allowedHeadersMode;
            }
            set
            {
                _allowedHeadersMode = value;
            }
        }

        public string AllowedStaticHeaders
        {
            get
            {
                return _allowedHeaders;
            }
            set
            {
                _allowedHeaders = value;
            }
        } 
    }

    public enum TracingMode
    {
        DISABLED,
        AUTO,
        MANUAL
    }

    public enum OptionsMode
    {
        DISABLED,
        AUTO,
        MANUAL
    }

    public enum CorsMode
    {
        ONLY_IF_ORIGIN_IS_SET,
        NEVER
    }

    public enum AllowedOriginMode
    {
        ANY,
        REQUESTER_ONLY
    }

    public enum AllowedHeadersMode
    {
        ANY,
        STATIC
    }
}