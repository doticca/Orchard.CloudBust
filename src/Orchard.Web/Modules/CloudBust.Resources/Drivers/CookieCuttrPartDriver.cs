using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using CloudBust.Resources.Models;
using Orchard;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Drivers {
    [OrchardFeature("CloudBust.Resources.CookieCuttr")]
    public class CookieCuttrPartDriver : ContentPartDriver<CookieCuttrPart> {
        private readonly IWorkContextAccessor _workContextAccessor;

        public CookieCuttrPartDriver(
            IWorkContextAccessor workContextAccessor
            ) 
        {
                _workContextAccessor = workContextAccessor;
        }

        protected override DriverResult Display(CookieCuttrPart part, string displayType, dynamic shapeHelper)
        {
            var workContext = _workContextAccessor.GetContext();
            var cookieSettings = workContext.CurrentSite.As<CookieCuttrSettingsPart>().Record;

            return ContentShape("Parts_CookieCuttr",
                            () => shapeHelper.Parts_CookieCuttr(CookieSettings: cookieSettings));
        }
    }
}