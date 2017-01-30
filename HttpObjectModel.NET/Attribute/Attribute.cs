using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Attribute : System.Attribute
    {
        private string _field;
        private string _name;

        public Attribute(string name, string field)
        {
            _field = field;
            _name = name;
        }

        public string Field
        {
            get
            {
                return _field;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

    }
}
