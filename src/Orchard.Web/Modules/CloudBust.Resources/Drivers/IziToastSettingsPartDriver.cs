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
    [OrchardFeature("CloudBust.Resources.IziToast")]
    public class IziToastSettingsPartDriver : ContentPartDriver<IziToastSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IIziToastService _izitoastService;

        public IziToastSettingsPartDriver(ISignals signals, IIziToastService izitoastService)
        {
            _signals = signals;
            _izitoastService = izitoastService;
        }

        protected override string Prefix { get { return "IziToastSettings"; } }

        protected override DriverResult Editor(IziToastSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_IziToast_IziToastSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/IziToast.IziToastSettings",
                                   Model: new IziToastSettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                   },
                                   Prefix: Prefix)).OnGroup("IziToast");
        }

        protected override DriverResult Editor(IziToastSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(IziToastSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(IziToastSettingsPart part, ImportContentContext context)
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