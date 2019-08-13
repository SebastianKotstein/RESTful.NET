using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Admin.Model
{
    public class HttpProcessorWrapper
    {
        private string _processingGroup;
        private bool _multithreaded;

        private IList<HttpControllerWrapper> _controllers = new List<HttpControllerWrapper>();

        [JsonProperty("processingGroup")]
        public string ProcessingGroup
        {
            get
            {
                return _processingGroup;
            }

            set
            {
                _processingGroup = value;
            }
        }

        [JsonProperty("multithreaded")]
        public bool Multithreaded
        {
            get
            {
                return _multithreaded;
            }

            set
            {
                _multithreaded = value;
            }
        }

        [JsonProperty("count")]
        public int Count
        {
            get
            {
                return _controllers.Count;
            }

        }

        [JsonProperty("controllers")]
        public IList<HttpControllerWrapper> Controllers
        {
            get
            {
                return _controllers;
            }

            set
            {
                _controllers = value;
            }
        }

        
    }

    public class HttpProcessorsWrapper
    {
        private IList<HttpProcessorWrapper> _processors = new List<HttpProcessorWrapper>();

        [JsonProperty("count")]
        public int Count
        {
            get
            {
                return _processors.Count;
            }
        }

        [JsonProperty("processors")]
        public IList<HttpProcessorWrapper> Processors
        {
            get
            {
                return _processors;
            }

            set
            {
                _processors = value;
            }
        }

        
    }
}
