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
    [OrchardFeature("CloudBust.Resources.OwlCarousel")]
    public class OwlCarouselSettingsPartDriver : ContentPartDriver<OwlCarouselSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IOwlCarouselService _owlcarouselService;

        public OwlCarouselSettingsPartDriver(ISignals signals, IOwlCarouselService owlcarouselService)
        {
            _signals = signals;
            _owlcarouselService = owlcarouselService;
        }

        protected override string Prefix { get { return "OwlCarouselSettings"; } }

        protected override DriverResult Editor(OwlCarouselSettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_OwlCarousel_OwlCarouselSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/OwlCarousel.OwlCarouselSettings",
                                   Model: new OwlCarouselSettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                   },
                                   Prefix: Prefix)).OnGroup("OwlCarousel");
        }

        protected override DriverResult Editor(OwlCarouselSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(OwlCarouselSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(OwlCarouselSettingsPart part, ImportContentContext context)
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