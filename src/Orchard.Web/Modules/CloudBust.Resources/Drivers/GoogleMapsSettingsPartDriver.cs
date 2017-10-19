using CloudBust.Resources.Models;
using CloudBust.Resources.Services;
using Orchard.Caching;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System;
using CloudBust.Resources.ViewModels;

namespace CloudBust.Resources.Drivers
{
    [OrchardFeature("CloudBust.Resources.GoogleMaps")]
    public class GoogleMapsSettingsPartDriver : ContentPartDriver<GoogleMapsSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IGoogleMapsService _googlemapsService;

        public GoogleMapsSettingsPartDriver(ISignals signals, IGoogleMapsService googlemapsService)
        {
            _signals = signals;
            _googlemapsService = googlemapsService;
        }

        protected override string Prefix { get { return "GoogleMapsSettings"; } }

        protected override DriverResult Editor(GoogleMapsSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_GoogleMaps_GoogleMapsSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/GoogleMaps.GoogleMapsSettings",
                                   Model: new GoogleMapsSettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                       ApiKey = part.ApiKey,
                                       Language = part.Language,
                                       Sensor = part.Sensor,
                                       Async = part.Async,
                                       Defer = part.Defer,
                                       CallBack = part.CallBack
                                   },
                                   Prefix: Prefix)).OnGroup("GoogleMaps");
        }

        protected override DriverResult Editor(GoogleMapsSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(GoogleMapsSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
            element.SetAttributeValue("ApiKey", part.ApiKey);
            element.SetAttributeValue("CallBack", part.CallBack);
            element.SetAttributeValue("Sensor", part.Sensor);
            element.SetAttributeValue("Language", part.Language);
            element.SetAttributeValue("Async", part.Async);
            element.SetAttributeValue("Defer", part.Defer);
        }

        protected override void Importing(GoogleMapsSettingsPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.AutoEnable = GetAttribute<bool>(context, partName, "AutoEnable");
            part.Record.AutoEnableAdmin = GetAttribute<bool>(context, partName, "AutoEnableAdmin");
            part.Record.Sensor = GetAttribute<bool>(context, partName, "Sensor");
            part.Record.Async = GetAttribute<bool>(context, partName, "Async");
            part.Record.Defer = GetAttribute<bool>(context, partName, "Defer");
            part.Record.ApiKey = GetAttribute<string>(context, partName, "ApiKey");
            part.Record.CallBack = GetAttribute<string>(context, partName, "CallBack");
            part.Record.Language = GetAttribute<string>(context, partName, "Language");
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