using SKotstein.Net.Http.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Core
{
    /// <summary>
    /// Base type for classes hosting methods which are linked to a REST function
    /// </summary>
    public class HttpController
    {
        private HttpService _service;
        private string _uuid = null;

        /// <summary>
        /// Gets the underlying <see cref="HttpService"/>
        /// </summary>
        public HttpService Service{
            get
            {
                return _service;
            }
            internal set
            {
                _service = value;
            }
        }

        public string Uuid
        {
            get
            {
                return _uuid;
            }
            set
            {
                _uuid = value;
            }
        }

        public string FriendlyName
        {
            get
            {
                return this.GetType().FullName;
            }
        }
    }
}
