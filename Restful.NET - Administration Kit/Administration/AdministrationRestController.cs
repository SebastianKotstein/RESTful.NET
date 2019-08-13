using Newtonsoft.Json;
using SKotstein.Net.Http.Admin.Model;
using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using SKotstein.Net.Http.Routing;
using SKotstein.Net.Http.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Admin
{
    public class AdministrationRestController : HttpController
    {
       

        public AdministrationRestController()
        {
            
        }

        [Path("/admin/routes", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetRoutes(HttpContext context)
        {
            RoutingEntriesWrapper entries = new RoutingEntriesWrapper();

            foreach(RoutingEntry re in Service.RoutingEngine.RoutingEntries.Values)
            {
                RoutingEntryWrapper entry = new RoutingEntryWrapper();
                entry.Nested = re;
                entries.RoutingEntries.Add(entry);
            }

            string json = Serialize(entries);
            if(json != null)
            {
                context.Response.Payload.Write(json);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                context.Response.Status = HttpStatus.InternalServerError;
            }
        }

        [Path("/admin/routes/{id}", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetRoute(HttpContext context, string id)
        {
            RoutingEntryWrapper entry = null;

            foreach (RoutingEntry re in Service.RoutingEngine.RoutingEntries.Values)
            {

                if (re.Identifier.ToLower().CompareTo(id.ToLower()) == 0)
                {
                    entry = new RoutingEntryWrapper();
                    entry.Nested = re;
                }
            }
            if(entry == null)
            {
                context.Response.Status = HttpStatus.NotFound;
                return;
            }

            string json = Serialize(entry);
            if (json != null)
            {
                context.Response.Payload.Write(json);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                context.Response.Status = HttpStatus.InternalServerError;
            }
        }

        [Path("/admin/routes", HttpMethod.POST)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void TestRoute(HttpContext context)
        {
            string payload = context.Request.Payload.ReadAll();
            PathWrapper path = Deserialize<PathWrapper>(payload);

            if(path == null)
            {
                context.Response.Status = HttpStatus.BadRequest;
                return;
            }

            RoutingEntry re = Service.RoutingEngine.GetEntry(path.Method+path.Path);
            RoutingEntriesWrapper entries = new RoutingEntriesWrapper();
            if(re != null)
            {
                RoutingEntryWrapper entry = new RoutingEntryWrapper();
                entry.Nested = re;
                entries.RoutingEntries.Add(entry);
            }

            string json = Serialize(entries);
            if(json != null)
            {
                context.Response.Payload.Write(json);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                context.Response.Status = HttpStatus.InternalServerError;
            }
        }

        [Path("/admin/processors",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetProcessors(HttpContext context)
        {
            HttpProcessorsWrapper root = new HttpProcessorsWrapper();
            
            foreach(IHttpProcessor processor in Service.HttpProcessors.Values)
            {
                HttpProcessorWrapper pw = new HttpProcessorWrapper();
                pw.Multithreaded = processor.IsMultiProcessor;
                pw.ProcessingGroup = processor.ProcessingGroupName;

                foreach(string uuid in DetermineHttpController(processor.ProcessingGroupName))
                {
                    HttpControllerWrapper cw = new HttpControllerWrapper();
                    foreach(RoutingEntry re in Service.RoutingEngine.RoutingEntries.Values)
                    {
                        if(re.ProcessingGroup.CompareTo(processor.ProcessingGroupName)==0 && re.HttpController.Uuid.CompareTo(uuid) == 0)
                        {
                            NestedRoutingEntry nre = new NestedRoutingEntry();
                            nre.Id = re.Identifier;
                            nre.Path = re.Path;
                            cw.Routing.Add(nre);

                            //do it everytime
                            cw.Nested = re.HttpController;
                        }
                    }
                    pw.Controllers.Add(cw);
                }
                root.Processors.Add(pw);
            }

            string json = Serialize(root);
            if(json != null)
            {
                context.Response.Payload.Write(json);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                context.Response.Status = HttpStatus.InternalServerError;
            }
            
        }

        [Path("/admin/processors/{name}", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetProcessors(HttpContext context, string name)
        {
            HttpProcessorsWrapper root = new HttpProcessorsWrapper();

            if (!Service.HttpProcessors.ContainsKey(name)){
                context.Response.Status = HttpStatus.NotFound;
                return;
            }
            else
            {
                IHttpProcessor processor = Service.HttpProcessors[name];
                HttpProcessorWrapper pw = new HttpProcessorWrapper();
                pw.Multithreaded = processor.IsMultiProcessor;
                pw.ProcessingGroup = processor.ProcessingGroupName;

                foreach (string uuid in DetermineHttpController(processor.ProcessingGroupName))
                {
                    HttpControllerWrapper cw = new HttpControllerWrapper();
                    foreach (RoutingEntry re in Service.RoutingEngine.RoutingEntries.Values)
                    {
                        if (re.ProcessingGroup.CompareTo(processor.ProcessingGroupName) == 0 && re.HttpController.Uuid.CompareTo(uuid) == 0)
                        {
                            NestedRoutingEntry nre = new NestedRoutingEntry();
                            nre.Id = re.Identifier;
                            nre.Path = re.Path;
                            cw.Routing.Add(nre);

                            //do it everytime
                            cw.Nested = re.HttpController;
                        }
                    }
                    pw.Controllers.Add(cw);
                }
                root.Processors.Add(pw);
            }

            string json = Serialize(root);
            if(json != null)
            {
                context.Response.Payload.Write(json);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                context.Response.Status = HttpStatus.InternalServerError;
            }

        }

        private ISet<string> DetermineHttpController(string processingGroup)
        {
            ISet<string> _hcs = new HashSet<string>();

            foreach(RoutingEntry re in Service.RoutingEngine.RoutingEntries.Values)
            {
                if (!_hcs.Contains(re.HttpController.Uuid) && re.ProcessingGroup.CompareTo(processingGroup)==0)
                {
                    _hcs.Add(re.HttpController.Uuid);
                }
            }

            return _hcs;
        }

        /// <summary>
        /// Converts a JSON structure into an object.
        /// Returns null, if a exception while converting occurs.
        /// </summary>
        /// <typeparam name="T">type of the class</typeparam>
        /// <param name="json">JSON structure</param>
        /// <returns>object</returns>
        public T Deserialize<T>(string json) where T : class
        {
            try
            {
                //throws an exception if JSON is null!
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Converts an object into a JSON structure.
        /// Returns null, if a exception while converting occurs or the passed object is null.
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>JSON structure</returns>
        public string Serialize(object o)
        {
            try
            {
                //returns null, if o is null
                return JsonConvert.SerializeObject(o);
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
