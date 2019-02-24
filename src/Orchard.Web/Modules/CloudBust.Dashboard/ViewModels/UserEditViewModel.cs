using CloudBust.Application.Models;

namespace CloudBust.Dashboard.ViewModels
{
    public class UserEditViewModel  {
        public UserProfilePart UserProfile { get; }
        public ApplicationRecord Application { get; }

        public string HostUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public bool ShowEmail { get; set; }

        public UserEditViewModel(ApplicationRecord app, UserProfilePart profile, string hostUrl)
        {
            this.Application = app;
            this.UserProfile = profile;
            HostUrl = hostUrl;
            FirstName = profile.FirstName;
            LastName = profile.LastName;
            Email = profile.Email;
            Bio = profile.Bio;
            Location = profile.Location;
            ShowEmail = profile.ShowEmail;
        }

    }
}
