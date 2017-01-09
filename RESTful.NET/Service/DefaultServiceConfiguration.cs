using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Service
{
    public class DefaultServiceConfiguration : ServiceConfiguration
    {
        public DefaultServiceConfiguration()
        {
            this.Host = "localhost";
            this.IsSecured = false;
            this.Port = 8080;
        }
    }
}
