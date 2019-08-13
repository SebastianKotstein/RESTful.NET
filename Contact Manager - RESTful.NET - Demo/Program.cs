using SKotstein.Net.Http.Admin;
using SKotstein.Net.Http.Core;
using SKotstein.Net.Http.Service;
using SKotstein.Net.Http.Web;
using System;
using System.Reflection;

namespace SKotstein.Demo.Http.ContactManager
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Step 1: create Business Layer 
            IContactHandler businessLayer = new ContactHandler();

            //Step 2: create REST Layer and link Business Layer
            HttpController restLayer = new ContactManagementController(businessLayer);

            //Step 3: add test object
            Contact contact = new Contact();
            contact.Surname = "Max";
            contact.Name = "Mustermann";

            Address address = new Address();
            address.City = "Hamburg";
            address.Zip = "20359";
            address.Street = "Bei den St. Pauli-Landungsbrücken";
            address.Country = "Deutschland";
            contact.Address = address;

            Phone phone = new Phone();
            phone.Number = "+4940428771234";
            phone.Type = PhoneNumberType.home;

            Phone phone2 = new Phone();
            phone2.Number = "+491572343865";
            phone2.Type = PhoneNumberType.mobile;

            businessLayer.AddContact(contact);
            businessLayer.AddPhone(contact.Id,phone);
            businessLayer.AddPhone(contact.Id, phone2);
            
            //Step 4: create server and link REST Layer
            HttpService service = new DefaultHttpSysService(true, "+", 500);
            service.AddController(restLayer,false);
            service.AddController(new AdministrationRestController());

            //Step 5: Start server
            
            Console.WriteLine("Routes:");
            Console.WriteLine(service.Routes);
            service.Start();

            Console.WriteLine("Webserver has been started");
            Console.WriteLine(service.Prefix);
            Console.ReadKey();

        }
    }
}
