using System.Linq;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.UI.Resources;
using CloudBust.Foundation.Services;
using Orchard.UI.Admin;
using Orchard.Environment;
using Orchard.DisplayManagement;
using System.Web;
using System;
using Orchard.Localization;


namespace CloudBust.Foundation.Shapes
{
    public class FoundationShapes : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IFoundationService _foundationService;
        public FoundationShapes(Work<WorkContext> workContext, IFoundationService foundationService)
        {
            _workContext = workContext;
            _foundationService = foundationService;
            T = NullLocalizer.Instance;
        }
        public Localizer T { get; set; }
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("HeadScripts").OnDisplaying(shapeDisplayingContext =>
                {
                    var request = _workContext.Value.HttpContext.Request;
                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("script");
                    var currentjQuery = scripts
                                            .Where(l => l.Name == "jQuery")
                                            .FirstOrDefault();
                    if (currentjQuery == null)
                    {
                        resourceManager.Require("script", "jQuery").AtFoot();
                    }
                    var currentFoundation = scripts
                                            .Where(l => l.Name == "Foundation")
                                            .FirstOrDefault();
                    if (currentFoundation == null)
                    {
                        if (!_foundationService.GetAutoEnableAdmin() && AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }
                        if(_foundationService.GetDoNotEnableFrontEnd() && !AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }

                        if (_foundationService.GetUseNicescroll())
                            resourceManager.Include("script", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.foundation/js/jquery.nicescroll.min.js", "~/Modules/CloudBust.Foundation/Scripts/jquery.nicescroll.js").AtFoot();
                        if (_foundationService.GetUsePlaceholder())
                            resourceManager.Include("script", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.foundation/js/jquery.placeholder.min.js", "~/Modules/CloudBust.Foundation/Scripts/jquery.placeholder.js").AtFoot();


                        resourceManager.Include("script", "~/Modules/CloudBust.Foundation/Scripts/what-input.min.js", "~/Modules/CloudBust.Foundation/Scripts/what-input.js").AtFoot();
                        resourceManager.Include("script", "~/Modules/CloudBust.Foundation/Scripts/foundation.6.4.2.min.js", "~/Modules/CloudBust.Foundation/Scripts/foundation.6.4.2.js").AtFoot();

                        if(AdminFilter.IsApplied(request.RequestContext))
                            resourceManager.Include("script", "~/Modules/CloudBust.Foundation/Scripts/admin.foundation.js", "~/Modules/CloudBust.Foundation/Scripts/admin.foundation.js").AtFoot();

                        if (_foundationService.GetUseDatePicker())
                            resourceManager.Include("script", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.foundation/js/foundation-datepicker.min.js", "~/Modules/CloudBust.Foundation/Scripts/foundation-datepicker.js").AtFoot();

                        if (_foundationService.GetUseSelect())// 
                            resourceManager.Include("script", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.foundation/js/foundation-select.min.js", "~/Modules/CloudBust.Foundation/Scripts/foundation-select.js").AtFoot();

                    }
                });
            builder.Describe("HeadLinks")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    var request = _workContext.Value.HttpContext.Request;
                    var resourceManager = _workContext.Value.Resolve<IResourceManager>();
                    var scripts = resourceManager.GetRequiredResources("stylesheet");
                    

                    var currentFoundation = scripts
                                                .Where(l => l.Name == "Foundation")
                                                .FirstOrDefault();

                    if (currentFoundation == null)
                    {
                        if (!_foundationService.GetAutoEnableAdmin() && AdminFilter.IsApplied(request.RequestContext))
                        {
                            resourceManager.Include("stylesheet", "~/themes/theadmin/styles/site.css", "~/themes/theadmin/styles/site.css").AtHead();
                            return;
                        }
                        if (_foundationService.GetDoNotEnableFrontEnd() && !AdminFilter.IsApplied(request.RequestContext))
                        {
                            return;
                        }

                        // foundation always first
                        resourceManager.Include("stylesheet", "~/modules/cloudbust.foundation/styles/foundation.6.4.2.min.css", "~/modules/cloudbust.foundation/styles/foundation.6.4.2.css").AtHead();
                        if (_foundationService.GetUseIcons())
                            resourceManager.Include("stylesheet", "~/modules/cloudbust.foundation/styles/foundation-icons.css", "~/modules/cloudbust.foundation/styles/foundation-icons.css").AtHead();
                        if (_foundationService.GetUseDatePicker())
                            resourceManager.Include("stylesheet", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.foundation/css/foundation-datepicker.min.css", "~/modules/cloudbust.foundation/styles/foundation-datepicker.css").AtHead();
                        if (_foundationService.GetUseSelect())
                            resourceManager.Include("stylesheet", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.foundation/css/foundation-select.min.css", "~/modules/cloudbust.foundation/styles/foundation-select.css").AtHead();
                        // contrib always last
                        resourceManager.Include("stylesheet", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.foundation/css/contrib.min.css", "~/modules/cloudbust.foundation/styles/contrib.css").AtHead();

                        if (AdminFilter.IsApplied(request.RequestContext))
                        {
                            resourceManager.Include("stylesheet", "~/modules/cloudbust.foundation/styles/adminsite.css", "~/modules/cloudbust.foundation/styles/adminsite.css").AtHead();
                            resourceManager.Include("stylesheet", "~/modules/cloudbust.foundation/styles/adminbuttons.css", "~/modules/cloudbust.foundation/styles/adminbuttons.css").AtHead();
                        }
                    }
                });
        }

        [Shape]
        public IHtmlString FoundationPublishedState(dynamic Display, DateTime createdDateTimeUtc, DateTime? publisheddateTimeUtc, string owner)
        {
            if (!publisheddateTimeUtc.HasValue)
            {
                return T("this document is a Draft copy.");
            }

            return T("Last update by: {0}, {1}", owner, Display.DateTime(DateTimeUtc: createdDateTimeUtc));
        }

    }
}