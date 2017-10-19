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
    [OrchardFeature("CloudBust.Resources.ImagesLoaded")]
    public class imagesLoadedSettingsPartDriver : ContentPartDriver<imagesLoadedSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IimagesLoadedService _imagesLoadedService;

        public imagesLoadedSettingsPartDriver(ISignals signals, IimagesLoadedService imagesLoadedService)
        {
            _signals = signals;
            _imagesLoadedService = imagesLoadedService;
        }

        protected override string Prefix { get { return "imagesLoadedSettings"; } }

        protected override DriverResult Editor(imagesLoadedSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_ImagesLoaded_imagesLoadedSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/ImagesLoaded.imagesLoadedSettings",
                                   Model: new imagesLoadedSettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                   },
                                   Prefix: Prefix)).OnGroup("imagesLoaded");
        }

        protected override DriverResult Editor(imagesLoadedSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(imagesLoadedSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(imagesLoadedSettingsPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.AutoEnable = GetAttribute<bool>(context, partName, "AutoEnable");
            part.Record.AutoEnableAdmin = GetAttribute<bool>(context, partName, "AutoEnableAdmin");
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