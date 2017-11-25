using System.Runtime.Serialization;

namespace CloudBust.Application.OData.Profile
{
    [DataContract]
    public class LoginFB
    {
        [DataMember]
        public string Username { get; private set; }
        [DataMember]
        public string Token { get; private set; }
        [DataMember]
        public string ApiKey { get; private set; }

        public LoginFB()
        {
            Username = string.Empty;
            Token = string.Empty;
            ApiKey = string.Empty;
        }
    }
}