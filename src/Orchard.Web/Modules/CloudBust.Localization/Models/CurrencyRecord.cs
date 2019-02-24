namespace CloudBust.Localization.Models
{
    public class CurrencyRecord
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string ThreeDigitCode { get; set; }
        public virtual string Sign { get; set; }
        public virtual bool ShowSignAfter { get; set; }
    }
}