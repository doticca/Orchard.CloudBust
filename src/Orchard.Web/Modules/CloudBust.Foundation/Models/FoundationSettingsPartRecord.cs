using Orchard.ContentManagement.Records;

namespace CloudBust.Foundation.Models
{
    public class FoundationSettingsPartRecord: ContentPartRecord
    {
        public virtual bool AutoEnableAdmin { get; set; }
        public virtual bool DoNotEnableFrontEnd { get; set; }
        public virtual bool UseDatePicker { get; set; }
        public virtual bool UseSelect { get; set; }
        public virtual bool UseIcons { get; set; }
        public virtual bool UsePlaceholder { get; set; }
        public virtual bool UseNicescroll { get; set; }
        public FoundationSettingsPartRecord()
        {
            AutoEnableAdmin = false;
            DoNotEnableFrontEnd = false;
            UseDatePicker = true;
            UseSelect = true;
            UseIcons = true;
            UsePlaceholder = true;
            UseNicescroll = true;
        }
    }
}