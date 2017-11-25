using System.Runtime.Serialization;

namespace CloudBust.Application.OData.Profile
{
    [DataContract]
    public class RegisterConfirmation
    {
        [DataMember]
        public string nonce { get; private set; }

        public RegisterConfirmation()
        {
            nonce = string.Empty;
        }
    }
}