using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Manipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Core
{
    public interface IHttpProcessor
    {
        bool IsMultiProcessor { get; }

        string ProcessingGroupName { get; set; }

        HttpManipulatorCollection<RoutedContext> PreManipulators { get; }

        HttpManipulatorCollection<RoutedContext> PostManipulators { get; }

        void StartProcessor();
        void StopProcessor();


    }
}
