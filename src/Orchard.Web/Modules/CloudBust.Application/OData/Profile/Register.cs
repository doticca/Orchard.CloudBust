using System.Runtime.Serialization;

namespace CloudBust.Application.OData.Profile
{
    [DataContract]
    public class Register
    {
        [DataMember]
        public string FirstName { get; private set; }
        [DataMember]
        public string LastName { get; private set; }
        [DataMember]
        public string Email { get; private set; }
        [DataMember]
        public string Password { get; private set; }
        [DataMember]
        public string ApiKey { get; private set; }

        public Register()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ApiKey = string.Empty;
        }
    }
}