using System;
using System.Linq;
using System.Web;
using CloudBust.Foundation.Services;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Resources;

namespace CloudBust.Foundation.Shapes {
    public class FoundationShapes : IShapeTableProvider {
        private readonly IFoundationService _foundationService;
        private readonly Work<WorkContext> _workContext;

        public FoundationShapes(Work<WorkContext> workContext, IFoundationService foundationService) {
            _workContext = workContext;
            _foundationService = foundationService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("HeadScripts").OnDisplaying(shapeDisplayingContext => {
                var request = _workContext.Value.HttpContext.Request;
                var isAdmin = AdminFilter.IsApplied(request.RequestContext);
                var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                var scripts = resourceManager.GetRequiredResources("script").ToList();

                var currentjQuery = scripts.FirstOrDefault(l => l.Name == "jQuery");

                if (currentjQuery == null) {
                    resourceManager.Require("script", "jQuery").AtFoot();
                }

                var currentFoundation = scripts.FirstOrDefault(l => l.Name == "Foundation");

                if (currentFoundation != null) {
                    return;
                }

                if (!_foundationService.GetAutoEnableAdmin() && isAdmin) {
                    return;
                }

                if (_foundationService.GetDoNotEnableFrontEnd() && !isAdmin) {
                    return;
                }

                if (_foundationService.GetUseNicescroll()) {
                    resourceManager.Include("script",
                        "https://cloudbust.blob.core.windows.net/public/js/jquery.nicescroll.min.js",
                        "~/Modules/CloudBust.Foundation/Scripts/jquery.nicescroll.js").AtFoot();
                    resourceManager.Include("script",
                                        "https://cloudbust.blob.core.windows.net/public/js/jquery.nicescroll.iframehelper.min.js",
                                        "~/Modules/CloudBust.Foundation/Scripts/jquery.nicescroll.iframehelper.js")
                                   .AtFoot();
                }

                if (_foundationService.GetUsePlaceholder()) {
                    resourceManager.Include("script",
                        "https://cloudbust.blob.core.windows.net/public/js/jquery.placeholder.min.js",
                        "~/Modules/CloudBust.Foundation/Scripts/jquery.placeholder.js").AtFoot();
                }

                resourceManager.Require("script", "Foundation").AtFoot();

                if (_foundationService.GetUseDatePicker()) {
                    resourceManager.Include("script",
                        "https://cloudbust.blob.core.windows.net/public/js/foundation-datepicker.min.js",
                        "~/Modules/CloudBust.Foundation/Scripts/foundation-datepicker.js").AtFoot();
                }

                if (_foundationService.GetUseSelect()) {
                    resourceManager.Include("script",
                        "https://cloudbust.blob.core.windows.net/public/js/perfect-scrollbar.min.js",
                        "~/Modules/CloudBust.Foundation/Scripts/perfect-scrollbar.js").AtFoot();
                    resourceManager.Include("script",
                                        "https://cloudbust.blob.core.windows.net/public/js/foundation.perfectScrollbar.min.js",
                                        "~/Modules/CloudBust.Foundation/Scripts/foundation.perfectScrollbar.js")
                                   .AtFoot();
                    resourceManager.Include("script",
                        "https://cloudbust.blob.core.windows.net/public/js/foundation.select.min.js",
                        "~/Modules/CloudBust.Foundation/Scripts/foundation.select.js").AtFoot();
                }

                if (isAdmin) {
                    resourceManager.Include("script",
                        "https://cloudbust.blob.core.windows.net/public/js/admin.foundation.min.js",
                        "~/Modules/CloudBust.Foundation/Scripts/admin.foundation.js").AtFoot();
                }
            });

            builder.Describe("HeadLinks")
                   .OnDisplaying(shapeDisplayingContext => {
                        var request = _workContext.Value.HttpContext.Request;
                        var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                        var scripts = resourceManager.GetRequiredResources("stylesheet");
                        var isAdmin = AdminFilter.IsApplied(request.RequestContext);

                        var currentFoundation = scripts.FirstOrDefault(l => l.Name == "Foundation");

                        if (currentFoundation != null) {
                            return;
                        }

                        if (!_foundationService.GetAutoEnableAdmin() && isAdmin) {
                            resourceManager.Include("stylesheet",
                                "https://cloudbust.blob.core.windows.net/public/css/theadmin/site.min.css",
                                "~/themes/theadmin/styles/site.css").AtHead();
                            return;
                        }

                        if (_foundationService.GetDoNotEnableFrontEnd() && !isAdmin) {
                            return;
                        }

                        // foundation always first
                        if (isAdmin) {
                            // load default foundation css for admin
                            resourceManager.Include("stylesheet",
                                "https://cloudbust.blob.core.windows.net/public/css/foundation.6.5.1.float.min.css",
                                "~/modules/cloudbust.foundation/styles/foundation.6.5.1.float.css").AtHead();
                        }
                        else {
                            var gridstyle = _foundationService.GetGridStyleText();
                            resourceManager.Include("stylesheet",
                                "https://cloudbust.blob.core.windows.net/public/css/foundation.6.5.1." +
                                gridstyle + ".min.css",
                                "~/modules/cloudbust.foundation/styles/foundation.6.5.1." + gridstyle +
                                ".css").AtHead();
                        }

                        if (_foundationService.GetUseIcons()) {
                            resourceManager.Include("stylesheet",
                                "https://cloudbust.blob.core.windows.net/public/css/foundation-icons.css",
                                "~/modules/cloudbust.foundation/styles/foundation-icons.css").AtHead();
                        }

                        if (_foundationService.GetUseDatePicker()) {
                            resourceManager.Include("stylesheet",
                                                "https://cloudbust.blob.core.windows.net/public/css/foundation-datepicker.min.css",
                                                "~/modules/cloudbust.foundation/styles/foundation-datepicker.css")
                                           .AtHead();
                        }

                        if (_foundationService.GetUseSelect()) {
                            resourceManager.Include("stylesheet",
                                                "https://cloudbust.blob.core.windows.net/public/css/foundation-perfect-scrollbar.min.css",
                                                "~/modules/cloudbust.foundation/styles/foundation-perfect-scrollbar.css")
                                           .AtHead();
                            resourceManager.Include("stylesheet",
                                "https://cloudbust.blob.core.windows.net/public/css/foundation-select.min.css",
                                "~/modules/cloudbust.foundation/styles/foundation-select.css").AtHead();
                        }

                        // contrib always last
                        resourceManager.Include("stylesheet",
                            "https://cloudbust.blob.core.windows.net/public/css/contrib.min.css",
                            "~/modules/cloudbust.foundation/styles/contrib.css").AtHead();

                        if (isAdmin) {
                            resourceManager.Include("stylesheet",
                                "https://cloudbust.blob.core.windows.net/public/css/theadmin/adminsite.min.css",
                                "~/modules/cloudbust.foundation/styles/adminsite.css").AtHead();
                            resourceManager.Include("stylesheet",
                                "https://cloudbust.blob.core.windows.net/public/css/theadmin/adminbuttons.min.css",
                                "~/modules/cloudbust.foundation/styles/adminbuttons.css").AtHead();
                        }
                    });
        }

        [Shape]
        public IHtmlString FoundationPublishedState(dynamic Display, DateTime createdDateTimeUtc,
            DateTime? publisheddateTimeUtc, string owner) {
            return !publisheddateTimeUtc.HasValue
                ? T("this document is a Draft copy.")
                : (IHtmlString) T("Last update by: {0}, {1}", owner, Display.DateTime(DateTimeUtc: createdDateTimeUtc));
        }
    }
}