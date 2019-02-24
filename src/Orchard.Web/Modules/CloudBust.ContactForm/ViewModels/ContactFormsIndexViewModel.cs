using System.Collections.Generic;
using CloudBust.ContactForm.Models;

namespace CloudBust.ContactForm.ViewModels
{
    public class ContactFormsIndexViewModel
    {
        public IEnumerable<ContactFormRecord> ContactForms { get; set; }
    }
}