using System.Web.Mvc;
using CloudBust.Common.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Filters;
using Orchard.UI.Admin;
using Orchard.UI.Resources;

namespace CloudBust.Common.Filters {
    [OrchardFeature("CloudBust.Common.CustomCode")]
    public class CustomCodeFilter : FilterProvider, IResultFilter {
        private readonly IOrchardServices _orchardServices;
        private readonly IResourceManager _resourceManager;

        public CustomCodeFilter(IResourceManager resourceManager, IOrchardServices orchardServices) {
            _resourceManager = resourceManager;
            _orchardServices = orchardServices;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) { }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            if (filterContext != null && AdminFilter.IsApplied(filterContext.RequestContext))
                return;

            if (!(filterContext?.Result is ViewResult))
                return;

            var part = _orchardServices.WorkContext.CurrentSite.As<CustomCodeSettingsPart>();
            if (!string.IsNullOrWhiteSpace(part?.HeadCode)) _resourceManager.RegisterHeadScript(part.HeadCode);

            if (!string.IsNullOrWhiteSpace(part?.FootCode)) _resourceManager.RegisterFootScript(part.FootCode);
        }
    }
}