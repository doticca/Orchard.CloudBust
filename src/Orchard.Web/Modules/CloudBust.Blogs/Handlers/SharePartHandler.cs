using Contrib.Voting.Models;
using Contrib.Voting.Services;
using CloudBust.Blogs.Models;
using CloudBust.Blogs.Settings;
using Orchard;
using Orchard.ContentManagement.Handlers;
using System;
using System.Linq;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Handlers
{
    public class SharePartHandler : ContentHandler
    {
        private readonly IOrchardServices _orchardServices;

        public SharePartHandler(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;

            OnInitializing<SharePart>((context, part) =>
            {
                part.ShowFacebook = part.Settings.GetModel<ShareTypePartSettings>().ShowFacebook;
                part.ShowTwitter = part.Settings.GetModel<ShareTypePartSettings>().ShowTwitter;
                part.ShowMail = part.Settings.GetModel<ShareTypePartSettings>().ShowMail;
            });
            OnGetDisplayShape<SharePart>((context, part) =>
            {
                var settings = part.Settings.GetModel<ShareTypePartSettings>();
                if (context.DisplayType.Equals("Detail", StringComparison.InvariantCultureIgnoreCase))
                {

                }

            });
        }
    }
}