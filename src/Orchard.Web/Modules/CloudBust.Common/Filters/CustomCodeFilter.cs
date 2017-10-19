using CloudBust.Common.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Mvc.Filters;
using Orchard.UI.Resources;
using System;
using System.Web.Mvc;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Filters {

    [OrchardFeature("CloudBust.Common.CustomCode")]
    public class CustomCodeFilter : FilterProvider, IResultFilter {
        private readonly IResourceManager _resourceManager;
        private readonly IOrchardServices _orchardServices;

        public CustomCodeFilter(IResourceManager resourceManager, IOrchardServices orchardServices) {
			_resourceManager = resourceManager;
			_orchardServices = orchardServices;
		}

        public void OnResultExecuted(ResultExecutedContext filterContext) {
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
                return;

            if (Orchard.UI.Admin.AdminFilter.IsApplied(filterContext.RequestContext))
                return;

            var part = _orchardServices.WorkContext.CurrentSite.As<CustomCodeSettingsPart>();
            if (part != null) {
                if (!String.IsNullOrWhiteSpace(part.HeadCode)) {
                    _resourceManager.RegisterHeadScript(part.HeadCode);
                }
                if (!String.IsNullOrWhiteSpace(part.FootCode)) {
                    _resourceManager.RegisterFootScript(part.FootCode);
                }
            }
        }
    }
}