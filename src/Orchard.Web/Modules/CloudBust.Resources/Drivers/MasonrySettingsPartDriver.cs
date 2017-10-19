using CloudBust.Resources.Models;
using CloudBust.Resources.Services;
using Orchard.Caching;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudBust.Resources.ViewModels;

namespace CloudBust.Resources.Drivers
{
    [OrchardFeature("CloudBust.Resources.Masonry")]
    public class MasonrySettingsPartDriver : ContentPartDriver<MasonrySettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IMasonryService _masonryService;

        public MasonrySettingsPartDriver(ISignals signals, IMasonryService masonryService)
        {
            _signals = signals;
            _masonryService = masonryService;
        }

        protected override string Prefix { get { return "MasonrySettings"; } }

        protected override DriverResult Editor(MasonrySettingsPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_Masonry_MasonrySettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/Masonry.MasonrySettings",
                                   Model: new MasonrySettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                   },
                                   Prefix: Prefix)).OnGroup("Masonry");
        }

        protected override DriverResult Editor(MasonrySettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(MasonrySettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(MasonrySettingsPart part, ImportContentContext context)
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