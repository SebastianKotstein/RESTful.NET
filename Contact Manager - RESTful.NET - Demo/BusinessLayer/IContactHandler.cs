using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Demo.Http.ContactManager
{
    /// <summary>
    /// This interface defines methods of the Business Layer and is implemented by the <see cref="ContactHandler"/>. 
    /// The method are used by the REST Layer (see <see cref="ContactManagementController"/>).
    /// </summary>
    public interface IContactHandler
    {
        /// <summary>
        /// Returns a list containing all handled contacts. The list is empty if there is no contact.
        /// </summary>
        /// <returns>List with all contacts. The list might be empty.</returns>
        IList<Contact> GetAllContacts();

        /// <summary>
        /// Returns the contact object having the specifid ID or null if there is not such a contact having the passed ID.
        /// </summary>
        /// <param name="id">contact id</param>
        /// <returns>corresponding contact object or null</returns>
        Contact GetContact(int id);

        /// <summary>
        /// Adds the passed contact object and returns the added object with the system generated ID.
        /// </summary>
        /// <param name="contact">contact object which should be added</param>
        /// <returns>added contact object with generated ID</returns>
        Contact AddContact(Contact contact);

        /// <summary>
        /// Updates a contact by the passed values. The contact is identified by the contained ID (which cannot be changed). The method returns the changed object or null,
        /// if there is not such a contact having the passed ID.
        /// </summary>
        /// <param name="contact">contact object with changed values</param>
        /// <returns>changed contact object or null</returns>
        Contact UpdateContact(Contact contact);

        /// <summary>
        /// Removes the contact object with the specified ID or does nothing if there is not such a contact having the passed ID.
        /// </summary>
        /// <param name="contactId">contact ID</param>
        void RemoveContact(int contactId);

        /// <summary>
        /// Returns the address of the contact object with the specified ID or null if there is not such a contact having the passed ID.
        /// </summary>
        /// <param name="contactId">contact ID</param>
        /// <returns>address object</returns>
        Address GetAddress(int contactId);

        /// <summary>
        /// Updates a address by the passed values of the contact having the specified ID. The method returns the changed address object or null if there is not such a contact having the passed ID.
        /// </summary>
        /// <param name="contactId">contact ID</param>
        /// <param name="address">address object with changed values</param>
        /// <returns>changed address object</returns>
        Address UpdateAddress(int contactId, Address address);

        /// <summary>
        /// Returns a list with all phone numbers of the contact object having the specifid ID. The list is empty if the contact has no phone number or null if there is not such a contact having the passed ID.
        /// </summary>
        /// <param name="contactId">contact ID</param>
        /// <returns>List with phones objects. The list might be empty or null.</returns>
        IList<Phone> GetAllPhoneNumbers(int contactId);

        /// <summary>
        /// Returns the phone object having the specified phone ID of the contact object with the specified contact ID. The return value is null if there is not such a phone object or contact object.
        /// </summary>
        /// <param name="contactId">contact ID</param>
        /// <param name="phoneId">phone ID</param>
        /// <returns></returns>
        Phone GetPhoneNumber(int contactId, int phoneId);

        /// <summary>
        /// Adds the passed phone object to the contact having the specified contact ID and returns the added object with the system generated phone ID. The return value is null if there is not such a contact having the passed ID.
        /// </summary>
        /// <param name="contactId">contact ID</param>
        /// <param name="phone">phone object which should be added</param>
        /// <returns>added phone object or null</returns>
        Phone AddPhone(int contactId, Phone phone);

        /// <summary>
        /// Updates a phone object by the passed values with the contained phone ID of the contact having the specified ID. The method returns the changed phone object or null if there is not such a phone or contact having the passed ID.
        /// </summary>
        /// <param name="contactId">contact ID</param>
        /// <param name="phone">phone object with changed values</param>
        /// <returns>changes phone object or null</returns>
        Phone UpdatePhone(int contactId, Phone phone);

        /// <summary>
        /// Removes the phone having the specified ID of the contact with the specified ID. The method returns true if the phone object has been removed (or has not been in the list) or false if there is not such a contact having the passed ID.
        /// </summary>
        /// <param name="contactId">contact ID</param>
        /// <param name="phoneId">phone ID</param>
        /// <returns>true if the phone object has been removed (or has not been in the list) or false if there is not such a contact having the passed ID</returns>
        bool RemovePhone(int contactId, int phoneId);

    }
}
