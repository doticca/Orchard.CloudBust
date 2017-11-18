using CloudBust.Resources.Models;
using CloudBust.Resources.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using System;

namespace CloudBust.Resources.Drivers {
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class ParticlesPartDriver : ContentPartDriver<ParticlesPart>
    {
        private readonly IParticlesService _particlesService;
        public ParticlesPartDriver(IParticlesService particlesService)
        {
            _particlesService = particlesService;
        }
        protected override DriverResult Display(ParticlesPart part, string displayType, dynamic shapeHelper)
        {
            if (string.IsNullOrWhiteSpace(part.JsonUrl))
            {
                var jsonurl = _particlesService.GetJsonUrl();
                part.JsonUrl = jsonurl;
            }
            return ContentShape("Parts_Particles",
                            () => shapeHelper.Parts_Particles(Url: part.JsonUrl));
        }
        protected override DriverResult Editor(ParticlesPart part, dynamic shapeHelper)
        {
            var jsonurl = _particlesService.GetJsonUrl();
            part.JsonUrl = jsonurl;

            return ContentShape("Parts_Particles_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts/Particles.Edit", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(ParticlesPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var jsonurl = _particlesService.GetJsonUrl();
            part.JsonUrl = jsonurl;

            if (updater != null) {
                updater.TryUpdateModel(part, Prefix, null, null);
            }

            return Editor(part, shapeHelper);
        }

        protected override void Importing(ParticlesPart part, Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;
        }

        protected override void Exporting(ParticlesPart part, Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);
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