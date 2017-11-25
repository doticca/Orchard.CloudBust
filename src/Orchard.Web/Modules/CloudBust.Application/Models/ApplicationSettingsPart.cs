using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Application.Models
{
    public class ApplicationSettingsPart : ContentPart<ApplicationSettingsPartRecord> {    
        public string ApplicationKey
        {
            get { return Record.ApplicationKey; }
            set { Record.ApplicationKey = value; }
        }
        public string ApplicationName
        {
            get { return Record.ApplicationName; }
            set { Record.ApplicationName = value; }
        }
    }
}