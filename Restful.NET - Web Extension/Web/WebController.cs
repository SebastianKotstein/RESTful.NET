using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Web
{
    public class WebController : HttpController
    {
        private string _rootPath;
        private RessourceCache _cache;

        public string RootPath
        {
            get
            {
                return _rootPath;
            }

            set
            {
                _rootPath = value;
            }
        }

        public WebController()
        {
            _cache = new RessourceCache();
        }

        public WebController(string rootPath) : this()
        {
            _rootPath = rootPath;

        }

        [Attributes.Path("/*",Context.HttpMethod.GET)]
        public void Get(HttpContext context)
        {
            ChachedRessource res = _cache.GetRessouce(_rootPath + context.Request.Path.Replace('/', '\\'));
            if(res != null)
            {
                context.Response.Payload.WriteBytes(res.Ressource);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                context.Response.Status = HttpStatus.NotFound;
            }            
        }
    }
}
