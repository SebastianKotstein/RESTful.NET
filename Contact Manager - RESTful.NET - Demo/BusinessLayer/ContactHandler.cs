using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Demo.Http.ContactManager
{
    /// <summary>
    /// The <see cref="ContactHandler"/> represents the core component of the Business Layer and contains its main logic.
    /// The <see cref="IContactHandler"/> interface is implemented by this class. Consult the <see cref="IContactHandler"/> for method definitions.
    /// </summary>
    public class ContactHandler : IContactHandler
    {
        /// <summary>
        /// Internal dictionary with stored contacts. The contact ID is used as a key.
        /// </summary>
        private IDictionary<int, Contact> _contacts;

        /// <summary>
        /// Internal counter for system generated IDs.
        /// </summary>
        private static int _counter = 0;

        public ContactHandler()
        {
            _contacts = new Dictionary<int, Contact>();
        }

        public Contact AddContact(Contact contact)
        {
            contact.SetId(ContactHandler._counter++);
            _contacts.Add(contact.Id,contact);
            foreach(Phone phone in contact.PhoneNumbers)
            {
                phone.SetId(ContactHandler._counter++);
            }
            return contact;
        }

        public Phone AddPhone(int contactId, Phone phone)
        {
            if (_contacts.ContainsKey(contactId))
            {
                phone.SetId(ContactHandler._counter++);
                _contacts[contactId].PhoneNumbers.Add(phone);
                return phone;
            }
            else
            {
                return null;
            }
        }

        public Address GetAddress(int contactId)
        {
            if (_contacts.ContainsKey(contactId))
            {
                return _contacts[contactId].Address;
            }
            else
            {
                return null;
            }
        }

        public IList<Contact> GetAllContacts()
        {
            return _contacts.Values.ToList();
        }

        public IList<Phone> GetAllPhoneNumbers(int contactId)
        {
            if (_contacts.ContainsKey(contactId))
            {
                return _contacts[contactId].PhoneNumbers;
            }
            else
            {
                return null;
            }
        }

        public Contact GetContact(int id)
        {
            if (_contacts.ContainsKey(id))
            {
                return _contacts[id];
            }
            else
            {
                return null;
            }
        }

        public Phone GetPhoneNumber(int contactId, int phoneId)
        {
            if (_contacts.ContainsKey(contactId))
            {
                return _contacts[contactId].PhoneNumbers.First(p => p.Id == phoneId);
            }
            else
            {
                return null;
            }
        }

        public void RemoveContact(int contactId)
        {
            if (_contacts.ContainsKey(contactId))
            {
                Contact contact = _contacts[contactId];
                _contacts.Remove(contactId);
            }

        }

        public bool RemovePhone(int contactId, int phoneId)
        {
            if (_contacts.ContainsKey(contactId))
            {
                Phone phone = _contacts[contactId].PhoneNumbers.First(p => p.Id == phoneId);
                if(phone != null)
                {
                    _contacts[contactId].PhoneNumbers.Remove(phone);
                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public Address UpdateAddress(int contactId, Address address)
        {
            if (_contacts.ContainsKey(contactId))
            {
                _contacts[contactId].Address = address;
                return address;

            }
            else
            {
                return null;
            }

        }

        public Contact UpdateContact(Contact contact)
        {
            if (_contacts.ContainsKey(contact.Id))
            {
                //do not update phone numbers
                contact.PhoneNumbers = _contacts[contact.Id].PhoneNumbers;

                _contacts[contact.Id] = contact;
                return contact;

            }
            else
            {
                return null;
            }
        }

        public Phone UpdatePhone(int contactId, Phone phone)
        {
            if (_contacts.ContainsKey(contactId))
            {
                Phone p = null;
                foreach(Phone pp in _contacts[contactId].PhoneNumbers)
                {
                    if(pp.Id == phone.Id)
                    {
                        p = pp;
                    }
                }
                if (p != null)
                {
                    p.Number = phone.Number;
                    p.Type = phone.Type;
                    return p;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
    }
}
