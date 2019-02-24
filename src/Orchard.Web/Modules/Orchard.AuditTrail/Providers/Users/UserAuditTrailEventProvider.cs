using Orchard.AuditTrail.Services;
using Orchard.AuditTrail.Services.Models;
using Orchard.Environment.Extensions;

namespace Orchard.AuditTrail.Providers.Users {
    [OrchardFeature("Orchard.AuditTrail.Users")]
    public class UserAuditTrailEventProvider : AuditTrailEventProviderBase {
        public const string SendingSms = "SendingSms";
        public const string SmsSent = "SmsSent";
        public const string SmsError = "SmsError";
        public const string LoggedIn = "LoggedIn";
        public const string LoggedOut = "LoggedOut";
        public const string LogInFailed = "LogInFailed";
        public const string PasswordChanged = "PasswordChanged";

        public override void Describe(DescribeContext context) {
            context.For("User", T("Users"))
                .Event(this, SendingSms, T("Sending SMS"), T("A user is about to send an SMS."), enableByDefault: true)
                .Event(this, SmsError, T("SMS Error"), T("An error occured while sending an SMS."), enableByDefault: true)
                .Event(this, SmsSent, T("SMS Sent"), T("A user has sent an SMS."), enableByDefault: true)
                .Event(this, LoggedIn, T("Logged in"), T("A user was successfully logged in."), enableByDefault: true)
                .Event(this, LoggedOut, T("Logged out"), T("A user actively logged out."), enableByDefault: true)
                .Event(this, LogInFailed, T("Login failed"), T("An attempt to login failed due to incorrect credentials."), enableByDefault: true)
                .Event(this, PasswordChanged, T("Password changed"), T("A user's password was changed."), enableByDefault: true);
        }
    }
}