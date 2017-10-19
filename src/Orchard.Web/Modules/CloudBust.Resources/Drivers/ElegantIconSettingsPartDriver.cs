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
    [OrchardFeature("CloudBust.Resources.ElegantIcon")]
    public class ElegantIconSettingsPartDriver : ContentPartDriver<ElegantIconSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IElegantIconService _eleganticonService;

        public ElegantIconSettingsPartDriver(ISignals signals, IElegantIconService eleganticonService)
        {
            _signals = signals;
            _eleganticonService = eleganticonService;
        }

        protected override string Prefix { get { return "ElegantIconSettings"; } }

        protected override DriverResult Editor(ElegantIconSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_ElegantIcon_ElegantIconSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/ElegantIcon.ElegantIconSettings",
                                   Model: new ElegantIconSettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin
                                   },
                                   Prefix: Prefix)).OnGroup("ElegantIcon");
        }

        protected override DriverResult Editor(ElegantIconSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(ElegantIconSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(ElegantIconSettingsPart part, ImportContentContext context)
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