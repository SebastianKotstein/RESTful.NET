using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Admin.Model
{
    public class PathWrapper
    {
        private string _path;
        private string _method;

        [JsonProperty("path")]
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }

        [JsonProperty("method")]
        public string Method
        {
            get
            {
                return _method;
            }
            set
            {
                _method = value;
            }
        }
    }
}
