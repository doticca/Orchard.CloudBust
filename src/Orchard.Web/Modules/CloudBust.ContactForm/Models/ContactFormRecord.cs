namespace CloudBust.ContactForm.Models
{
    public class ContactFormRecord
    {
        public virtual int Id { get; set; }
        public virtual string Email { get; set; }
        public virtual string Name { get; set; }
        public virtual string Company { get; set; }
        public virtual string Note { get; set; }
    }
}