using System.Web.Routing;
using Orchard.Themes;
using Orchard.UI.Admin;
using CloudBust.Foundation.Services;

namespace CloudBust.Foundation
{
    public class OverrideTheAdmin : IThemeSelector
    {
        private readonly IFoundationService _foundationService;
        public OverrideTheAdmin(IFoundationService foundationService)
        {
            _foundationService = foundationService;
        }
        public ThemeSelectorResult GetTheme(RequestContext context)
        {
            if (AdminFilter.IsApplied(context) && _foundationService.GetAutoEnableAdmin())
            {
                return new ThemeSelectorResult { Priority = 110, ThemeName = "TheAdminFoundation" };
            }

            return null;
        }
    }

}