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
    [OrchardFeature("CloudBust.Resources.FontAwesome")]
    public class FontAwesomeSettingsPartDriver : ContentPartDriver<FontAwesomeSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IFontAwesomeService _fontawesomeService;

        public FontAwesomeSettingsPartDriver(ISignals signals, IFontAwesomeService fontawesomeService)
        {
            _signals = signals;
            _fontawesomeService = fontawesomeService;
        }

        protected override string Prefix { get { return "FontAwesomeSettings"; } }

        protected override DriverResult Editor(FontAwesomeSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_FontAwesome_FontAwesomeSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/FontAwesome.FontAwesomeSettings",
                                   Model: new FontAwesomeSettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin
                                   },
                                   Prefix: Prefix)).OnGroup("FontAwesome");
        }

        protected override DriverResult Editor(FontAwesomeSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(FontAwesomeSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(FontAwesomeSettingsPart part, ImportContentContext context)
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