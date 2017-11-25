using CloudBust.Application.Services;
using Orchard.Commands;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Security;
using Orchard.Settings;

//using System.Web.Mvc;

namespace CloudBust.Application.Commands
{
    public class CategoryCommands : DefaultOrchardCommandHandler
    {
        private readonly IApplicationsService _applicationsService;
        private readonly IMembershipService _membershipService;
        private readonly ISiteService _siteService;

        public CategoryCommands(
            IApplicationsService applicationsService,
            IMembershipService membershipService,
            ISiteService siteService
            )
        {
            _applicationsService = applicationsService;
            _membershipService = membershipService;
            _siteService = siteService;
        }

        [OrchardSwitch]
        public string Name { get; set; }

        [OrchardSwitch]
        public string Description { get; set; }

        [CommandName("category create")]
        [CommandHelp("category create /Name:<categoryname> /Description:<description> \r\n\t" + "Creates a new Category")]
        [OrchardSwitches("Name,Description")]
        public void Create()
        {
            string Owner = _siteService.GetSiteSettings().SuperUser;
            var owner = _membershipService.GetUser(Owner);

            if (owner == null)
            {
                Context.Output.WriteLine(T("Invalid username: {0}", Owner));
                return;
            }

            //if (!_applicationsService.VerifyCategoryUnicity(Name))
            //{
            //    Context.Output.WriteLine(T("Category with that category name already exists."));
            //    return;
            //}

            if (Description == null)
            {
                Context.Output.WriteLine(T("You must specify a description for the new category."));
                return;
            }

            var category = _applicationsService.CreateCategory(Name, Description);

            if (category == null)
            {
                Context.Output.WriteLine(T("Could not create category {0}. The Category service returned an error", Name));
                return;
            }

            Context.Output.WriteLine(T("Category created successfully"));
        }
    }
}