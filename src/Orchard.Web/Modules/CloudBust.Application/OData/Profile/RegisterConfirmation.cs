using System.Runtime.Serialization;

namespace CloudBust.Application.OData.Profile
{
    [DataContract]
    public class RegisterConfirmation
    {
        public RegisterConfirmation()
        {
            Nonce = string.Empty;
        }

        [DataMember]
        public string Nonce { get; private set; }
    }
}