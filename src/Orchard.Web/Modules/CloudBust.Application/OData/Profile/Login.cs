using System.Runtime.Serialization;

namespace CloudBust.Application.OData.Profile
{
    [DataContract]
    public class Login
    {
        public static string[] Platforms = {"ios", "osx", "tvos", "watch", "js"};

        public Login()
        {
            Username = string.Empty;
            Password = string.Empty;
            ApiKey = string.Empty;
            Hash = string.Empty;
            Platform = string.Empty;
            Build = 0;
            VendorId = string.Empty;
            Model = string.Empty;
            SystemName = string.Empty;
            SystemVersion = string.Empty;
        }

        [DataMember]
        public string Username { get; private set; }

        [DataMember]
        public string Password { get; private set; }

        [DataMember]
        public string ApiKey { get; private set; }

        [DataMember]
        public string Hash { get; private set; }

        [DataMember]
        public string Platform { get; private set; }

        [DataMember]
        public int Build { get; private set; }

        [DataMember]
        public string VendorId { get; private set; }

        [DataMember]
        public string Model { get; private set; }

        [DataMember]
        public string SystemName { get; private set; }

        [DataMember]
        public string SystemVersion { get; private set; }
    }
}