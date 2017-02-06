using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Demo.Http.ContactManager
{
    public class Contact
    {
        private string _name;
        private string _surname;

        private Address _address;

        private IList<Phone> _phoneNumbers;

        private int _id;


        public Contact(string name, string surname, Address homeAddress)
        {
            Name = name;
            Surname = surname;
            Address = homeAddress;
            PhoneNumbers = new List<Phone>();
        }

        public Contact() : this("", "", null)
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

        [JsonProperty("name")]
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

        [JsonProperty("surname")]
        public string Surname
        {
            get
            {
                return _surname;
            }

            set
            {
                _surname = value;
            }
        }

        [JsonProperty("address")]
        public Address Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
            }
        }

        [JsonProperty("phone")]
        public IList<Phone> PhoneNumbers
        {
            get
            {
                return _phoneNumbers;
            }

            set
            {
                _phoneNumbers = value;
            }
        }

        internal void SetId(int value)
        {
            _id = value;
        }

    }
}
