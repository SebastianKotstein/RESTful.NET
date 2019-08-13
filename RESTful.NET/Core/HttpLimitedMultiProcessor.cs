using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Manipulation;
using SKotstein.Net.Http.Pipelining.Multi;
using SKotstein.Net.Http.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Core
{
    /// <summary>
    /// The <see cref="HttpLimitedMultiProcessor"/> represents a multi processing group having its own threads. Designated incoming HTTP requests are processed in similtanously by REST functions of the nested <see cref="HttpController"/> objects. 
    /// </summary>
    public class HttpLimitedMultiProcessor : LimitedMultiFilter<RoutedContext, HttpContext>, IHttpProcessor
    {
        private HttpService _reference;
        private HttpProcessorOperations _ops;
        private string _processingGroupName;

        private HttpManipulatorCollection<RoutedContext> _internalPreManipulators;
        private HttpManipulatorCollection<RoutedContext> _preManipulators;

        private HttpManipulatorCollection<RoutedContext> _internalPostManipulators;
        private HttpManipulatorCollection<RoutedContext> _postManipulators;

        public HttpManipulatorCollection<RoutedContext> PreManipulators
        {
            get
            {
                return _preManipulators;
            }
        }

        public HttpManipulatorCollection<RoutedContext> PostManipulators
        {
            get
            {
                return _postManipulators;
            }
        }

        public bool IsMultiProcessor
        {
            get
            {
                return true;
            }
        }

        public string ProcessingGroupName
        {
            get
            {
                return _processingGroupName;
            }

            set
            {
                _processingGroupName = value;
            }
        }

        internal HttpLimitedMultiProcessor(HttpService reference)
        {
            _reference = reference;

            _internalPreManipulators = new HttpManipulatorCollection<RoutedContext>();
            _internalPostManipulators = new HttpManipulatorCollection<RoutedContext>();

            _preManipulators = new HttpManipulatorCollection<RoutedContext>();
            _postManipulators = new HttpManipulatorCollection<RoutedContext>();

            _ops = new HttpProcessorOperations(_reference);
        }

        protected override HttpContext Execute(RoutedContext task)
        {
            //process HTTP Request
            _ops.ProcessHttpRequest(task, _internalPreManipulators, _preManipulators, _internalPostManipulators, _postManipulators);

            return task.Context;
        }

        protected override void Init()
        {
            _internalPreManipulators.Add(new ContentTypePreSetter());
            _internalPreManipulators.Add(new CorsHeaderSetter(_reference));

            _internalPostManipulators.Add(new ContentTypePostSetter());
        }

        protected override void Final()
        {
            
        }

        public void StartProcessor()
        {
            Start();
        }

        public void StopProcessor()
        {
            Stop();
        }
    }
}
