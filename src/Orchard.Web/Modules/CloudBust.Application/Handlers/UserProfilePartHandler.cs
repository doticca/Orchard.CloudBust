using CloudBust.Application.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;
using System;

namespace CloudBust.Application.Handlers
{
    public class UserProfilePartHandler : ContentHandler
    {
        public UserProfilePartHandler(
            IRepository<UserProfilePartRecord> repository
            )
        {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<UserProfilePart>("User"));
            Filters.Add(StorageFilter.For(repository));

            OnInitializing<UserProfilePart>((context, part) =>
            {
                part.UniqueID = Guid.NewGuid().ToString("N");
            });
        }

        public Localizer T { get; set; }
    }
}