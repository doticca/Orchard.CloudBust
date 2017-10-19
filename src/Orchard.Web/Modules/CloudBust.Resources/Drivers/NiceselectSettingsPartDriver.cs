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
    [OrchardFeature("CloudBust.Resources.Niceselect")]
    public class NiceselectSettingsPartDriver : ContentPartDriver<NiceselectSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly INiceselectService _niceselectService;

        public NiceselectSettingsPartDriver(ISignals signals, INiceselectService niceselectService)
        {
            _signals = signals;
            _niceselectService = niceselectService;
        }

        protected override string Prefix { get { return "NiceselectSettings"; } }

        protected override DriverResult Editor(NiceselectSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_Niceselect_NiceselectSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/Niceselect.NiceselectSettings",
                                   Model: new NiceselectSettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                   },
                                   Prefix: Prefix)).OnGroup("Niceselect");
        }

        protected override DriverResult Editor(NiceselectSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(NiceselectSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
        }

        protected override void Importing(NiceselectSettingsPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.AutoEnable = GetAttribute<bool>(context, partName, "AutoEnable");
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