using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Administration
{
    public class AdministrationController
    {
        [Path("/admin",Context.HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_XHTML_XML)]
        public void GetMainPage(HttpContext context)
        {

        }
    }
}
