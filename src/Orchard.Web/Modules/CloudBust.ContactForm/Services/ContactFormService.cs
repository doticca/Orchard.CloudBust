using System.Collections.Generic;
using System.Linq;
using CloudBust.ContactForm.Models;
using Orchard.Data;

namespace CloudBust.ContactForm.Services
{
    public class ContactFormService : IContactFormService
    {
        private readonly IRepository<ContactFormRecord> _contactFormRepository;

        public ContactFormService(
            IRepository<ContactFormRecord> contactFormRepository
        )
        {
            _contactFormRepository = contactFormRepository;
        }

        public ContactFormRecord GetContactForm(int id)
        {
            try
            {
                var contactFormRecord = _contactFormRepository.Get(id);
                return contactFormRecord;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<ContactFormRecord> GetContactForms()
        {
            try
            {
                var contactFormRecords = from contactFormRecord in _contactFormRepository.Table select contactFormRecord;
                return contactFormRecords.ToList();
            }
            catch
            {
                return new List<ContactFormRecord>();
            }
        }

        public ContactFormRecord CreateContactForm(string email, string name, string company, string note)
        {
            var contactForm = new ContactFormRecord
            {
                Email = email,
                Name = name,
                Company = company,
                Note = note
            };

            _contactFormRepository.Create(contactForm);

            return contactForm;
        }       

        public bool UpdateContactForm(int id, string email, string name, string company, string note)
        {
            var contactFormRecord = GetContactForm(id);
            if (contactFormRecord == null) {
                return false;
            }

            contactFormRecord.Email = email;
            contactFormRecord.Name = name;
            contactFormRecord.Company = company;
            contactFormRecord.Note = note;

            return true;
        }

        public bool DeleteContactForm(int id)
        {
            var contactFormRecord = GetContactForm(id);
            if (contactFormRecord == null) {
                return false;
            }

            _contactFormRepository.Delete(contactFormRecord);

            return true;
        }
    }
}