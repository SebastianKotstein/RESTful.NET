using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Demo.Http.ContactManager
{
    public class Phone
    {
        private string _number;
        private PhoneNumberType _type;
        private int _id;


        public Phone(string _number, PhoneNumberType _type)
        {
            Number = _number;
            Type = _type;
        }

        public Phone() : this("", PhoneNumberType.home)
        {

        }

        [JsonProperty("id")]
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                //always system generated
            }
        }

        [JsonProperty("tel")]
        public string Number
        {
            get
            {
                return _number;
            }

            set
            {
                _number = value;
            }
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PhoneNumberType Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        internal void SetId(int value)
        {
            _id = value;
        }
    }

    public enum PhoneNumberType
    {
        home,
        mobile,
        business,
        businessmobile
    }
}
