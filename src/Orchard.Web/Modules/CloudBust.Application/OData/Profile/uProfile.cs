using System.Runtime.Serialization;
using CloudBust.Application.Models;

namespace CloudBust.Application.OData.Profile
{
    [DataContract]
    public class UProfile
    {
        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string Username { get; private set; }

        [DataMember]
        public bool ShowEmail { get; private set; }

        [DataMember]
        public string FirstName { get; private set; }

        [DataMember]
        public string LastName { get; private set; }

        [DataMember]
        public string WebSite { get; private set; }

        [DataMember]
        public string Bio { get; private set; }

        [DataMember]
        public string Location { get; private set; }

        public void UpdateProfile(UserProfilePart profile)
        {
            profile.FirstName = string.IsNullOrWhiteSpace(FirstName) ? string.Empty : FirstName;
            profile.LastName = string.IsNullOrWhiteSpace(LastName) ? string.Empty : LastName;
            profile.Location = string.IsNullOrWhiteSpace(Location) ? string.Empty : Location;
            profile.WebSite = string.IsNullOrWhiteSpace(WebSite) ? string.Empty : WebSite;
            profile.Bio = string.IsNullOrWhiteSpace(Bio) ? string.Empty : Bio;
            profile.ShowEmail = ShowEmail;
        }

        public void PatchProfile(UserProfilePart profile)
        {
            profile.FirstName = string.IsNullOrWhiteSpace(FirstName) ? profile.FirstName : FirstName;
            profile.LastName = string.IsNullOrWhiteSpace(LastName) ? profile.LastName : LastName;
            profile.Location = string.IsNullOrWhiteSpace(Location) ? profile.Location : Location;
            profile.WebSite = string.IsNullOrWhiteSpace(WebSite) ? profile.WebSite : WebSite;
            profile.Bio = string.IsNullOrWhiteSpace(Bio) ? profile.Bio : Bio;
            profile.ShowEmail = ShowEmail;
        }
    }
}