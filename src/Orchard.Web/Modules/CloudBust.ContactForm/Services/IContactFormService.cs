using Orchard;
using System.Collections.Generic;
using CloudBust.ContactForm.Models;

namespace CloudBust.ContactForm.Services
{
    public interface IContactFormService : IDependency
    {
        ContactFormRecord GetContactForm(int id);
        IEnumerable<ContactFormRecord> GetContactForms();
        ContactFormRecord CreateContactForm(string email, string name, string company, string note);
        bool UpdateContactForm(int id, string email, string name, string company, string note);
        bool DeleteContactForm(int id);
    }
}