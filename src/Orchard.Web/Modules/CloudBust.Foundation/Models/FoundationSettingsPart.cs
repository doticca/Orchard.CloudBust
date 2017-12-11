using Orchard.ContentManagement;

namespace CloudBust.Foundation.Models
{
    public class FoundationSettingsPart : ContentPart<FoundationSettingsPartRecord> {    
        public bool AutoEnableAdmin
        {
            get { return Record.AutoEnableAdmin; }
            set { Record.AutoEnableAdmin = value; }
        }
        public bool DoNotEnableFrontEnd
        {
            get { return Record.DoNotEnableFrontEnd; }
            set { Record.DoNotEnableFrontEnd = value; }
        }
        public bool UseDatePicker
        {
            get { return Record.UseDatePicker; }
            set { Record.UseDatePicker = value; }
        }
        public bool UseSelect
        {
            get { return Record.UseSelect; }
            set { Record.UseSelect = value; }
        }
        public bool UseIcons
        {
            get { return Record.UseIcons; }
            set { Record.UseIcons = value; }
        }
        public bool UsePlaceholder
        {
            get { return Record.UsePlaceholder; }
            set { Record.UsePlaceholder = value; }
        }
        public bool UseNicescroll
        {
            get { return Record.UseNicescroll; }
            set { Record.UseNicescroll = value; }
        }
        public int GridStyle
        {
            get { return Record.GridStyle; }
            set { Record.GridStyle = value; }
        }
        public string GridStyleText
        {
            get
            {
                int gs = GridStyle;
                switch(gs)
                {
                    case 0:
                        return "float";
                    case 1:
                        return "flex";
                    case 2:
                        return "xy";
                    default:
                        return "float";
                }
            }
            set
            {
                switch (value)
                {
                    case "float":
                        GridStyle = 0;
                        break;
                    case "flex":
                        GridStyle = 1;
                        break;
                    case "xy":
                        GridStyle = 2;
                        break;
                    default:
                        GridStyle = 0;
                        break;
                }
            }
        }
    }
}