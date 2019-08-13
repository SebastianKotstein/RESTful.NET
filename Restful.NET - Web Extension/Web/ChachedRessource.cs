using SKotstein.Net.Http.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Web
{
    public class ChachedRessource
    {

        private string _path;
        private MimeType _mimeType;
        private byte[] _ressource;

        private DateTime _cachedDate;

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

        public MimeType MimeType
        {
            get
            {
                return _mimeType;
            }

            set
            {
                _mimeType = value;
            }
        }

        public byte[] Ressource
        {
            get
            {
                return _ressource;
            }

            set
            {
                _ressource = value;
            }
        }

        public DateTime CachedDate
        {
            get
            {
                return _cachedDate;
            }

            set
            {
                _cachedDate = value;
            }
        }
    }
}
