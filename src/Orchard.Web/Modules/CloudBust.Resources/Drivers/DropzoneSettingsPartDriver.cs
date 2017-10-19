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
    [OrchardFeature("CloudBust.Resources.DropZone")]
    public class DropzoneSettingsPartDriver : ContentPartDriver<DropzoneSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IDropzoneService _dropzoneService;

        public DropzoneSettingsPartDriver(ISignals signals, IDropzoneService dropzoneService)
        {
            _signals = signals;
            _dropzoneService = dropzoneService;
        }

        protected override string Prefix { get { return "DropzoneSettings"; } }

        protected override DriverResult Editor(DropzoneSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_Dropzone_DropzoneSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/Dropzone.DropzoneSettings",
                                   Model: new DropzoneSettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                   },
                                   Prefix: Prefix)).OnGroup("Dropzone");
        }

        protected override DriverResult Editor(DropzoneSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(DropzoneSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(DropzoneSettingsPart part, ImportContentContext context)
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