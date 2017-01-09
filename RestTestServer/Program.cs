using SKotstein.Net.Http.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTestServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            HttpService service = new DefaultHttpSysService(false, "+", 8080);
            service.AddController(new ContactManagementController());
            Console.WriteLine("Routes:");
            Console.WriteLine(service.Routes);
            service.Start();
            Console.WriteLine("Webserver has started");
            Console.ReadKey();
        }
    }
}
