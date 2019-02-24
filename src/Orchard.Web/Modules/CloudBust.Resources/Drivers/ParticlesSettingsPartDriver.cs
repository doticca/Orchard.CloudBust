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
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class ParticlesSettingsPartDriver : ContentPartDriver<ParticlesSettingsPart> 
    {
        private readonly ISignals _signals;
        private readonly IParticlesService _particlesService;
        private const string signalstring = "CloudBust.Resources.Particles.Changed";
        public ParticlesSettingsPartDriver(ISignals signals, IParticlesService particlesService)
        {
            _signals = signals;
            _particlesService = particlesService;
        }

        protected override string Prefix { get { return "ParticlesSettings"; } }

        protected override DriverResult Editor(ParticlesSettingsPart part, dynamic shapeHelper)
        {
            var jsonSuggestions = _particlesService.GetJsonUrlSuggestions();
            return ContentShape("Parts_Particles_ParticlesSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/Particles.ParticlesSettings",
                                   Model: new ParticlesSettingsViewModel
                                   {
                                       JsonUrl = part.JsonUrl,
                                       AutoEnable = part.AutoEnable,
                                       AutoEnableAdmin = part.AutoEnableAdmin,
                                       JsonUrlSuggestions = jsonSuggestions
                                   },
                                   Prefix: Prefix)).OnGroup("Particles");
        }

        protected override DriverResult Editor(ParticlesSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger(signalstring);
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(ParticlesSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("JsonUrl", part.JsonUrl);
            element.SetAttributeValue("AutoEnable", part.AutoEnable);
            element.SetAttributeValue("AutoEnableAdmin", part.AutoEnableAdmin);
        }

        protected override void Importing(ParticlesSettingsPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.JsonUrl = GetAttribute<string>(context, partName, "JsonUrl");
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