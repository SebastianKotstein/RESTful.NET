using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Demo.Http.ContactManager
{
    public class Address
    {
        private string _street;
        private string _city;
        private string _zip;
        private string _country;


        public Address(string street, string city, string zip, string country)
        {
            Street = street;
            City = city;
            Zip = zip;
            Country = country;
        }

        public Address() : this("", "", "", ""){

        }

        [JsonProperty("street")]
        public string Street
        {
            get
            {
                return _street;
            }

            set
            {
                _street = value;
            }
        }

        [JsonProperty("city")]
        public string City
        {
            get
            {
                return _city;
            }

            set
            {
                _city = value;
            }
        }

        [JsonProperty("zip")]
        public string Zip
        {
            get
            {
                return _zip;
            }

            set
            {
                _zip = value;
            }
        }

        [JsonProperty("country")]
        public string Country
        {
            get
            {
                return _country;
            }

            set
            {
                _country = value;
            }
        }

    }
}
