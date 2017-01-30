using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Service
{
    public class DefaultServiceConfiguration : ServiceConfiguration
    {
        /// <summary>
        /// Default service configuration with <see cref="ServiceConfiguration.Host"/>="localhost", <see cref="ServiceConfiguration.Port"/>=8080 and <see cref="ServiceConfiguration.IsSecured"/>=false
        /// </summary>
        public DefaultServiceConfiguration()
        {
            this.Host = "localhost";
            this.IsSecured = false;
            this.Port = 8080;
        }
    }
}
