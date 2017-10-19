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
    [OrchardFeature("CloudBust.Resources.Magnific")]
    public class MagnificSettingsPartDriver : ContentPartDriver<MagnificSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IMagnificService _magnificService;

        public MagnificSettingsPartDriver(ISignals signals, IMagnificService magnificService)
        {
            _signals = signals;
            _magnificService = magnificService;
        }

        protected override string Prefix { get { return "MagnificSettings"; } }

        protected override DriverResult Editor(MagnificSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_Magnific_MagnificSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/Magnific.MagnificSettings",
                                   Model: new MagnificSettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                   },
                                   Prefix: Prefix)).OnGroup("Magnific");
        }

        protected override DriverResult Editor(MagnificSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(MagnificSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(MagnificSettingsPart part, ImportContentContext context)
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