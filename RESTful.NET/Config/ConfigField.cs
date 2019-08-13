using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Config
{
    public class ConfigField : ConfigItem
    {
        private string _name;
        private string _value;

        public ConfigField() : this("","")
        {

        }

        public ConfigField(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string AsString
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public int AsInteger
        {
            get
            {
                int i;
                Int32.TryParse(_value, out i);
                return i;
            }
            set
            {
                _value = "" + value;
            }
        }

        public double AsDouble
        {
            get
            {
                double d;
                Double.TryParse(_value, out d);
                return d;
            }
            set
            {
                _value = "" + value;
            }
        }

        public bool AsBool
        {
            get
            {
                bool b;
                Boolean.TryParse(_value, out b);
                return b;
            }
            set
            {
                _value = "" + value;
            }
        }
    }
}
