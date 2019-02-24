using System;
using CloudBust.Resources.Models;
using CloudBust.Resources.Services;
using CloudBust.Resources.ViewModels;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Drivers {
    [OrchardFeature("CloudBust.Resources.ImagesLoaded")]
    public class ImagesLoadedSettingsPartDriver : ContentPartDriver<ImagesLoadedSettingsPart> {
        private readonly IImagesLoadedService _imagesLoadedService;
        private readonly ISignals _signals;

        public ImagesLoadedSettingsPartDriver(ISignals signals, IImagesLoadedService imagesLoadedService) {
            _signals = signals;
            _imagesLoadedService = imagesLoadedService;
        }

        protected override string Prefix => "ImagesLoadedSettings";

        protected override DriverResult Editor(ImagesLoadedSettingsPart part, dynamic shapeHelper) {
            return ContentShape("Parts_ImagesLoaded_ImagesLoadedSettings",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/ImagesLoaded.ImagesLoadedSettings",
                    Model: new ImagesLoadedSettingsViewModel {
                        AutoEnable = part.AutoEnable,
                        AutoEnableAdmin = part.AutoEnableAdmin
                    },
                    Prefix: Prefix)).OnGroup("ImagesLoaded");
        }

        protected override DriverResult Editor(ImagesLoadedSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(ImagesLoadedSettingsPart part, ExportContentContext context) {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(ImagesLoadedSettingsPart part, ImportContentContext context) {
            var partName = part.PartDefinition.Name;

            part.Record.AutoEnable = GetAttribute<bool>(context, partName, "AutoEnable");
            part.Record.AutoEnableAdmin = GetAttribute<bool>(context, partName, "AutoEnableAdmin");
        }

        private TV GetAttribute<TV>(ImportContentContext context, string partName, string elementName) {
            var value = context.Attribute(partName, elementName);
            if (value != null) return (TV) Convert.ChangeType(value, typeof(TV));
            return default(TV);
        }
    }
}