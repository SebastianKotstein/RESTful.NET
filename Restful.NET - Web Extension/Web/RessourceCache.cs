using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Web
{
    public class RessourceCache
    {
        private IDictionary<string, ChachedRessource> _cache = new Dictionary<string, ChachedRessource>();

        public bool HasRessource(string path)
        {
            return _cache.ContainsKey(path);
        }

        public ChachedRessource GetRessouce(string path)
        {
            if (HasRessource(path))
            {
                try
                {
                    ChachedRessource res = _cache[path];
                    if(res.CachedDate != File.GetLastWriteTimeUtc(path))
                    {
                        res.Ressource = File.ReadAllBytes(path);
                        res.CachedDate = File.GetLastWriteTimeUtc(path);
                    }
                    return res;
                    
                }
                catch(Exception e)
                {
                    return null;
                }
            }
            else
            {
                try
                {
                    ChachedRessource res = new ChachedRessource();
                    res.CachedDate = File.GetLastWriteTimeUtc(path);
                    res.Ressource = File.ReadAllBytes(path);
                    res.Path = path;

                    _cache.Add(path, res);

                    return res;
                }
                catch(Exception e)
                {
                    return null;
                }

            }
            
        }

        public void ClearCache()
        {
            _cache.Clear();
        }
    }
}
