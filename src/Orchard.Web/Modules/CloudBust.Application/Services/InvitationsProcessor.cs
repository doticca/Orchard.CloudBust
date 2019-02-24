using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using CloudBust.Application.Models;
using Orchard.Security;
using System.Collections.Generic;
using Orchard.Messaging.Services;
using Orchard.DisplayManagement;
using Orchard.Mvc.Extensions;
using System.Web.Mvc;
using Orchard;
using System.Web.Hosting;
using System.Web;
using System.IO;
using System.Web.Routing;
using Orchard.Environment.ShellBuilders;
using Orchard.Settings;

namespace CloudBust.Application.Services {
    public class InvitationsProcessor : IInvitationsProcessor {

        private readonly IProfileService _profileService;
        private readonly IMessageService _messageService;
        private readonly IShapeFactory _shapeFactory;
        private readonly ISiteService _siteService;
        private readonly IShapeDisplay _shapeDisplay;

        public InvitationsProcessor(
            IProfileService profileService,
            IMessageService messageService,
            IShapeFactory shapeFactory,
            IWorkContextAccessor workContextAccessor,
            IShapeDisplay shapeDisplay,
            ISiteService siteService
            )
        {
            _profileService = profileService;
            _messageService = messageService;
            _shapeFactory = shapeFactory;
            _siteService = siteService;
            _shapeDisplay = shapeDisplay;
        }

        public void Invite(int id, string url)
        {
            if (_profileService.IsPendingInvitationProcessed(id)) {
                return;
            }

            var invitation = _profileService.GetPendingInvitation(id);
            var userprofile = _profileService.Get(invitation.UserProfilePartRecord);
            if (userprofile == null)
            {
                return;
            }

            var user = userprofile.As<IUser>();
            var app = invitation.ApplicationRecord;
              
            var template = _shapeFactory.Create("Template_User_Invitation", Arguments.From(new
            {
                User = user,
                Application = app,
                Invitation = invitation,
                Url = url
            }));
            var subject = "Join " + userprofile.FirstName + "'s care group on We+Care";

            template.Metadata.Wrappers.Add("Template_User_Wrapper");

            var parameters = new Dictionary<string, object> {
                {"Application", app.AppKey},
                {"Subject", subject},
                {"Body", _shapeDisplay.Display(template)},
                {"Recipients", invitation.invitationEmail}
            };

            _messageService.Send("Email", parameters);
            _profileService.PendingInvitationProcessed(id);
        }
    }
}