using System.Runtime.Serialization;

namespace CloudBust.Application.OData.Profile
{
    [DataContract]
    public class LoginGC
    {
        public static string[] Platforms = {"ios", "osx", "tvos", "watch"};

        public LoginGC()
        {
            Username = string.Empty;
            PublicKeyUrl = string.Empty;
            Signature = string.Empty;
            Salt = string.Empty;
            Name = string.Empty;
            ApiKey = string.Empty;
            VendorId = string.Empty;
            Model = string.Empty;
            SystemName = string.Empty;
            SystemVersion = string.Empty;
            Platform = "ios";
            Build = 0;
            Timestamp = 0;
        }

        [DataMember]
        public string Username { get; private set; }

        [DataMember]
        public string PublicKeyUrl { get; private set; }

        [DataMember]
        public string Signature { get; private set; }

        [DataMember]
        public string Salt { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public ulong Timestamp { get; private set; }

        [DataMember]
        public string ApiKey { get; private set; }

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