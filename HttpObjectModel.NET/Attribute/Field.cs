using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Field : System.Attribute
    {
        private string _name;
        
        public Field(string name)
        {
            _name = name;
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
