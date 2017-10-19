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
    [OrchardFeature("CloudBust.Resources.Highlight")]
    public class HighlightSettingsPartDriver : ContentPartDriver<HighlightSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IHighlightService _highlightService;

        public HighlightSettingsPartDriver(ISignals signals, IHighlightService highlightService)
        {
            _signals = signals;
            _highlightService = highlightService;
        }

        protected override string Prefix { get { return "HighlightSettings"; } }

        protected override DriverResult Editor(HighlightSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_Highlight_HighlightSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/Highlight.HighlightSettings",
                                   Model: new HighlightSettingsViewModel
                                   {
                                       Style = part.Style,
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                       FullBundle = part.FullBundle
                                   },
                                   Prefix: Prefix)).OnGroup("Highlight");
        }

        protected override DriverResult Editor(HighlightSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(HighlightSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("Style", part.Style);
            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
            element.SetAttributeValue("FullBundle", part.FullBundle);
        }

        protected override void Importing(HighlightSettingsPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.Style = GetAttribute<string>(context, partName, "Style");
            part.Record.AutoEnable = GetAttribute<bool>(context, partName, "AutoEnable");
            part.Record.AutoEnableAdmin = GetAttribute<bool>(context, partName, "AutoEnableAdmin");
            part.Record.FullBundle = GetAttribute<bool>(context, partName, "FullBundle");
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