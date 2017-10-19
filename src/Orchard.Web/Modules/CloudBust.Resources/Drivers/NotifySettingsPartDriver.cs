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
    [OrchardFeature("CloudBust.Resources.Notify")]
    public class NotifySettingsPartDriver : ContentPartDriver<NotifySettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly INotifyService _notifyService;

        public NotifySettingsPartDriver(ISignals signals, INotifyService notifyService)
        {
            _signals = signals;
            _notifyService = notifyService;
        }

        protected override string Prefix { get { return "NotifySettings"; } }

        protected override DriverResult Editor(NotifySettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Notify_NotifySettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/Notify.NotifySettings",
                                   Model: new NotifySettingsViewModel
                                   {
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                   },
                                   Prefix: Prefix)).OnGroup("Notify");
        }

        protected override DriverResult Editor(NotifySettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("CloudBust.Resources.Changed");
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(NotifySettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(NotifySettingsPart part, ImportContentContext context)
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