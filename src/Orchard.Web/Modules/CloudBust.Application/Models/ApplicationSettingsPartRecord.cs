using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace CloudBust.Application.Models
{
    public class ApplicationSettingsPartRecord: ContentPartRecord
    {
        //public virtual bool WebIsCloudBust { get; set; }
        public virtual string ApplicationKey { get; set; }
        public virtual string ApplicationName { get; set; }

        public ApplicationSettingsPartRecord()
        {
            //WebIsCloudBust = false;
            ApplicationKey = string.Empty;
            ApplicationName = string.Empty;
        }
    }
}