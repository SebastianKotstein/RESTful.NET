using Newtonsoft.Json;
using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SKotstein.Demo.Http.ContactManager
{
    /// <summary>
    /// Implements the logic of the REST Layer, i.e. the methods mapping the REST functions
    /// </summary>
    public class ContactManagementController : HttpController
    {
        /// <summary>
        /// Bridge (interface) to the Business Layer
        /// </summary>
        private IContactHandler _bridge;

        /// <summary>
        /// Creates the REST Layer and connects it with the passed Business Layer (implementation of <see cref="IContactHandler"/>)
        /// </summary>
        /// <param name="businessLayer"><see cref="IContactHandler"/> implementation alias Business Layer</param>
        public ContactManagementController(IContactHandler businessLayer)
        {
            _bridge = businessLayer;
        }


        [Path("/contact",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetAll(HttpContext context)
        {
            
            IList<Contact> list = _bridge.GetAllContacts();
            string json = SerializeJson(list);

            if(json != null)
            {
                context.Response.Payload.Write(json);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                context.Response.Status = HttpStatus.InternalServerError;
            }
        }

       

        /*
        [Path("/contact/a", HttpMethod.GET)]
        public void GetSingle2(HttpContext context, string contactId)
        {
        }
        */

        [Path("/contact/{id}", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetSingle(HttpContext context, string contactId)
        {
            int id;
            try { id = Int32.Parse(contactId); }catch(Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            Contact contact = _bridge.GetContact(id);
            if(contact != null)
            {
                string json = SerializeJson(contact);

                if (json != null)
                {
                    context.Response.Payload.Write(json);
                    context.Response.Status = HttpStatus.OK;
                }
                else
                {
                    context.Response.Status = HttpStatus.InternalServerError;
                }
            }
            else
            {
                context.Response.Status = HttpStatus.NotFound;
            }
        }

      

       

        [Path("/contact", HttpMethod.PUT)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void Add(HttpContext context)
        {
            Contact contact = DeserializeJson<Contact>(context.Request.Payload.ReadAll());
            if(contact != null)
            {
                contact = _bridge.AddContact(contact);
                string json = SerializeJson(contact);

                if (json != null)
                {
                    context.Response.Payload.Write(json);
                    context.Response.Status = HttpStatus.Created;
                }
                else
                {
                    context.Response.Status = HttpStatus.InternalServerError;
                }
            }
            else
            {
                context.Response.Status = HttpStatus.BadRequest;
            }
        }

        [Path("/contact/{id}", HttpMethod.POST)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void Update(HttpContext context, string contactId)
        {
            int id;
            try { id = Int32.Parse(contactId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            Contact contact = DeserializeJson<Contact>(context.Request.Payload.ReadAll());
            if (contact != null)
            {
                contact.SetId(id);
                contact = _bridge.UpdateContact(contact);
                if(contact == null)
                {
                   context.Response.Status = HttpStatus.NotFound;
                }

                string json = SerializeJson(contact);
                if (json != null)
                {
                    context.Response.Payload.Write(json);
                    context.Response.Status = HttpStatus.OK;
                    
                }
                else
                {
                    context.Response.Status = HttpStatus.InternalServerError;
                }
            }
            else
            {
                context.Response.Status = HttpStatus.BadRequest;
            }
        }

        [Path("/contact/{id}", HttpMethod.DELETE)]
        public void Delete(HttpContext context, string contactId)
        {
            int id;
            try { id = Int32.Parse(contactId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            _bridge.RemoveContact(id);
            context.Response.Status = HttpStatus.OK;

        }

        [Path("/contact/{id}/address", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetAddress(HttpContext context, string contactId)
        {
            int id;
            try { id = Int32.Parse(contactId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            Address address = _bridge.GetAddress(id);
            if(address != null)
            {
                string json = SerializeJson(address);

                if (json != null)
                {
                    context.Response.Payload.Write(json);
                    context.Response.Status = HttpStatus.OK;
                }
                else
                {
                    context.Response.Status = HttpStatus.InternalServerError;
                }
            }
            else
            {
                context.Response.Status = HttpStatus.NotFound;
            }
        }

        [Path("/contact/{id}/address", HttpMethod.POST)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void UpdateAddress(HttpContext context, string contactId)
        {
            int id;
            try { id = Int32.Parse(contactId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            Address address = DeserializeJson<Address>(context.Request.Payload.ReadAll());
            if (address != null)
            {
                address = _bridge.UpdateAddress(id, address);
                if (address != null)
                {
                    string json = SerializeJson(address);
                    if (json != null)
                    {
                        context.Response.Payload.Write(json);
                        context.Response.Status = HttpStatus.OK;
                    }
                    else
                    {
                        context.Response.Status = HttpStatus.InternalServerError;
                    }
                }
                else
                {
                    context.Response.Status = HttpStatus.NotFound;
                }
            }
            else
            {
                context.Response.Status = HttpStatus.BadRequest;
            }
        }

        [Path("/contact/{id}/phone", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetPhones(HttpContext context, string contactId)
        {
            int id;
            try { id = Int32.Parse(contactId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            IList<Phone> phones = _bridge.GetAllPhoneNumbers(id);

            if (phones != null)
            {
                string json = SerializeJson(phones);
                if(json != null)
                {
                    context.Response.Payload.Write(json);
                    context.Response.Status = HttpStatus.OK;
                }
                else
                {
                    context.Response.Status = HttpStatus.InternalServerError;
                }
            }
            else
            {
                context.Response.Status = HttpStatus.NotFound;
            }
        }

        [Path("/contact/{id}/phone/{pId}", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetPhones(HttpContext context, string contactId, string phoneId)
        {
            int cId;
            try { cId = Int32.Parse(contactId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            int pId;
            try { pId = Int32.Parse(phoneId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            Phone phone = _bridge.GetPhoneNumber(cId, pId);

            if(phone!= null)
            {
                string json = SerializeJson(phone);
                if (json != null)
                {
                    context.Response.Payload.Write(json);
                    context.Response.Status = HttpStatus.OK;
                }
                else
                {
                    context.Response.Status = HttpStatus.InternalServerError;
                }
            }
            else
            {
                context.Response.Status = HttpStatus.NotFound;
            }
        }

        [Path("/contact/{id}/phone", HttpMethod.PUT)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void AddPhone(HttpContext context, string contactId)
        {
            int id;
            try { id = Int32.Parse(contactId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            Phone phone = DeserializeJson<Phone>(context.Request.Payload.ReadAll());
            if(phone != null)
            {
                context.Response.Status = HttpStatus.BadRequest;
            }
            phone = _bridge.AddPhone(id, phone);

            if(phone!= null)
            {
                string json = SerializeJson(phone);
                if (json != null)
                {
                    context.Response.Payload.Write(json);
                    context.Response.Status = HttpStatus.Created;
                }
                else
                {
                    context.Response.Status = HttpStatus.InternalServerError;
                }
            }
            else
            {
                context.Response.Status = HttpStatus.NotFound;
            }

        }

        [Path("/contact/{id}/phone/{pId}", HttpMethod.POST)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void UpdatePhone(HttpContext context, string contactId, string phoneId)
        {
            int cId;
            try { cId = Int32.Parse(contactId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            int pId;
            try { pId = Int32.Parse(phoneId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            Phone phone = DeserializeJson<Phone>(context.Request.Payload.ReadAll());
            if (phone != null)
            {
                context.Response.Status = HttpStatus.BadRequest;
            }
            phone.SetId(pId);

            phone = _bridge.UpdatePhone(cId, phone);

            if(phone != null)
            {
                string json = SerializeJson(phone);
                if (json != null)
                {
                    context.Response.Payload.Write(json);
                    context.Response.Status = HttpStatus.Created;
                }
                else
                {
                    context.Response.Status = HttpStatus.InternalServerError;
                }
            }
            else
            {
                context.Response.Status = HttpStatus.NotFound;
            }
        }

        [Path("/contact/{id}/phone/{pId}", HttpMethod.DELETE)]
        public void DeletePhone(HttpContext context, string contactId, string phoneId)
        {
            int cId;
            try { cId = Int32.Parse(contactId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            int pId;
            try { pId = Int32.Parse(phoneId); } catch (Exception e) { context.Response.Status = HttpStatus.BadRequest; return; }

            if (_bridge.RemovePhone(cId, pId))
            {
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                context.Response.Status = HttpStatus.NotFound;
            }
        }

        /*
        [Path("/*", HttpMethod.GET)]
        public void SuperGenericTest(HttpContext context)
        {

        }
        */
        


        /// <summary>
        /// Converts a JSON structure into an object.
        /// Returns null, if a exception while converting occurs.
        /// </summary>
        /// <typeparam name="T">type of the class</typeparam>
        /// <param name="json">JSON structure</param>
        /// <returns>object</returns>
        private T DeserializeJson<T>(string json) where T : class
        {
            try
            {
                //throws an exception if JSON is null!
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts an object into a JSON structure.
        /// Returns null, if a exception while converting occurs or the passed object is null.
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>JSON structure</returns>
        public string SerializeJson(object o)
        {
            try
            {
                //returns null, if o is null
                return JsonConvert.SerializeObject(o);
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
