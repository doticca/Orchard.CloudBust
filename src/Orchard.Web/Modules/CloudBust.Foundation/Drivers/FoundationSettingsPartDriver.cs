using CloudBust.Foundation.Models;
using CloudBust.Foundation.Services;
using Orchard.Caching;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement;
using System;
using CloudBust.Foundation.ViewModels;

namespace js.Slick.Drivers
{
    public class FoundationSettingsPartDriver : ContentPartDriver<FoundationSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IFoundationService _foundationService;

        public FoundationSettingsPartDriver(ISignals signals, IFoundationService foundationService)
        {
            _signals = signals;
            _foundationService = foundationService;
        }

        protected override string Prefix { get { return "FoundationSettings"; } }

        protected override DriverResult Editor(FoundationSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_Foundation_FoundationSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/Foundation.FoundationSettings",
                                   Model: new FoundationSettingsViewModel
                                   {
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                       DoNotEnableFrontEnd = part.DoNotEnableFrontEnd,
                                       UseDatePicker = part.UseDatePicker,
                                       UseSelect = part.UseSelect,
                                       UseIcons = part.UseIcons,
                                       UseNicescroll = part.UseNicescroll,
                                       UsePlaceholder = part.UsePlaceholder,
                                       GridStyle = part.GridStyle
                                   },
                                   Prefix: Prefix)).OnGroup("Foundation");
        }

        protected override DriverResult Editor(FoundationSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Foundation.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(FoundationSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
            element.SetAttributeValue("DoNotEnableFrontEnd", part.AutoEnableAdmin);
            element.SetAttributeValue("UseDatePicker", part.UseDatePicker);
            element.SetAttributeValue("UseSelect", part.UseSelect);
            element.SetAttributeValue("UseIcons", part.UseIcons);
            element.SetAttributeValue("UseNicescroll", part.UseNicescroll);
            element.SetAttributeValue("UsePlaceholder", part.UsePlaceholder);
        }

        protected override void Importing(FoundationSettingsPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.AutoEnableAdmin = GetAttribute<bool>(context, partName, "AutoEnableAdmin");
            part.Record.DoNotEnableFrontEnd = GetAttribute<bool>(context, partName, "DoNotEnableFrontEnd");
            part.Record.UseDatePicker = GetAttribute<bool>(context, partName, "UseDatePicker");
            part.Record.UseSelect = GetAttribute<bool>(context, partName, "UseSelect");
            part.Record.UseIcons = GetAttribute<bool>(context, partName, "UseIcons");
            part.Record.UseNicescroll = GetAttribute<bool>(context, partName, "UseNicescroll");
            part.Record.UsePlaceholder = GetAttribute<bool>(context, partName, "UsePlaceholder");
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