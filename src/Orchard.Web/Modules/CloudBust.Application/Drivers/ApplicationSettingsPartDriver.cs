using CloudBust.Application.Models;
using CloudBust.Application.Services;
using Orchard.Caching;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement;
using System;
using CloudBust.Application.ViewModels;
using Orchard.Environment.Extensions;

namespace CloudBust.Application.Drivers
{
    [OrchardFeature("CloudBust.Application.WebApp")]
    public class ApplicationSettingsPartDriver : ContentPartDriver<ApplicationSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IApplicationsService _applicationsService;

        public ApplicationSettingsPartDriver(ISignals signals, IApplicationsService applicationsService)
        {
            _signals = signals;
            _applicationsService = applicationsService;
        }

        protected override string Prefix { get { return "ApplicationSettings"; } }

        protected override DriverResult Editor(ApplicationSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_CloudBust_ApplicationSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/CloudBust.ApplicationSettings",
                                   Model: new ApplicationSettingsViewModel
                                   {
                                       //WebIsCloudBust = part.WebIsCloudBust,
                                       ApplicationKey = part.ApplicationKey,
                                       Applications = _applicationsService.GetApplications(),
                                       ApplicationName = part.ApplicationName
                                   },
                                   Prefix: Prefix)).OnGroup("CloudBust");
        }

        protected override DriverResult Editor(ApplicationSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            var appkey = part.ApplicationKey;
            part.ApplicationName = _applicationsService.GetApplicationByKey(appkey).Name;

            _signals.Trigger(CBSignals.SignalWebApp);
            _signals.Trigger(CBSignals.SignalServer);
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(ApplicationSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            //element.SetAttributeValue("WebIsCloudBust", part.WebIsCloudBust);
            element.SetAttributeValue("ApplicationKey", part.ApplicationKey);
            element.SetAttributeValue("ApplicationName", part.ApplicationName);
        }

        protected override void Importing(ApplicationSettingsPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            //part.Record.WebIsCloudBust = GetAttribute<bool>(context, partName, "WebIsCloudBust");
            part.Record.ApplicationKey = GetAttribute<string>(context, partName, "ApplicationKey");
            part.Record.ApplicationName = GetAttribute<string>(context, partName, "ApplicationName");
        }

        private TV GetAttribute<TV>(ImportContentContext context, string partName, string elementName)
        {
            string value = context.Attribute(partName, elementName);
            if (value != null)
            {
                return (TV)Convert.ChangeType(value, typeof(TV));
            }
            return default(TV);
        }
    }
}