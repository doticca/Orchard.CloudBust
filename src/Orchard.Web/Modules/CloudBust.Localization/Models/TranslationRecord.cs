namespace CloudBust.Localization.Models
{
    public class TranslationRecord
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string TwoDigitCode { get; set; }
    }
}