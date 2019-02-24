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
    [OrchardFeature("CloudBust.Resources.IziModal")]
    public class IziModalSettingsPartDriver : ContentPartDriver<IziModalSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IIziModalService _izimodalService;

        public IziModalSettingsPartDriver(ISignals signals, IIziModalService izimodalService)
        {
            _signals = signals;
            _izimodalService = izimodalService;
        }

        protected override string Prefix { get { return "IziModalSettings"; } }

        protected override DriverResult Editor(IziModalSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_IziModal_IziModalSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/IziModal.IziModalSettings",
                                   Model: new IziModalSettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                   },
                                   Prefix: Prefix)).OnGroup("IziModal");
        }

        protected override DriverResult Editor(IziModalSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(IziModalSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(IziModalSettingsPart part, ImportContentContext context)
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