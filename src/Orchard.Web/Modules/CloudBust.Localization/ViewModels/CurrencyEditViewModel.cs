namespace CloudBust.Localization.ViewModels
{
    public class CurrencyEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ThreeDigitCode { get; set; }
        public string Sign { get; set; }
        public bool ShowSignAfter { get; set; }
    }
}