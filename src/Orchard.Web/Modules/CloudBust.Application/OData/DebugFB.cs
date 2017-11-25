using System.Runtime.Serialization;

namespace CloudBust.Application.OData
{
    [DataContract]
    public class DebugFB
    {
        [DataMember]
        public string Application { get; private set; }
        public bool isValid { get; private set; }
        public string AppId { get; private set; }
        public string UserId { get; private set; }
        public DebugFB()
        {
            Application = string.Empty;
            isValid = false;
            AppId = string.Empty;
            UserId = string.Empty;
        }

        public DebugFB(dynamic result)
        {
            Application = result.data["application"];
            isValid = result.data["is_valid"];
            AppId = result.data["app_id"];
            UserId = result.data["user_id"];
        }

    }
}