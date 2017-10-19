using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Models
{
    [OrchardFeature("CloudBust.Resources.GoogleMaps")]
    public class GoogleMapsSettingsPart : ContentPart<GoogleMapsSettingsPartRecord> {    
        public bool AutoEnable
        {
            get { return Record.AutoEnable; }
            set { Record.AutoEnable = value; }
        }
        public string ApiKey
        {
            get { return Record.ApiKey; }
            set { Record.ApiKey = value; }
        }
        public string CallBack
        {
            get { return Record.CallBack; }
            set { Record.CallBack = value; }
        }
        public string Language
        {
            get { return Record.Language; }
            set { Record.Language = value; }
        }
        public bool AutoEnableAdmin
        {
            get { return Record.AutoEnableAdmin; }
            set { Record.AutoEnableAdmin = value; }
        }
        public bool Sensor
        {
            get { return Record.Sensor; }
            set { Record.Sensor = value; }
        }
        public bool Async
        {
            get { return Record.Async; }
            set { Record.Async = value; }
        }
        public bool Defer
        {
            get { return Record.Defer; }
            set { Record.Defer = value; }
        }
    }
}