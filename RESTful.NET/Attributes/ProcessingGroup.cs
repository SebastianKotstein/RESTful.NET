using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Attributes
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class ProcessingGroup : System.Attribute
    {
    }
}
