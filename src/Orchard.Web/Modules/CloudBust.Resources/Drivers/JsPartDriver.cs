using CloudBust.Resources.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.Drivers;
using System;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Drivers
{
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class JsPartDriver : ContentPartDriver<JsPart>
    {
        private const string TemplateName = "Parts/Js";
        protected override string Prefix
        {
            get { return "Js"; }
        }
        protected override DriverResult Display(JsPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Js",
                () => shapeHelper.Parts_Js(Js: part));
        }

        protected override DriverResult Editor(JsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Js_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(JsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return ContentShape("Parts_Js_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override void Importing(JsPart part, Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            string partName = part.PartDefinition.Name;
            part.Record.Js = GetAttribute<string>(context, partName, "Js");
        }

        protected override void Exporting(JsPart part, Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            string partName = part.PartDefinition.Name;
            context.Element(partName).SetAttributeValue("Js", string.IsNullOrWhiteSpace(part.Record.Js) ? string.Empty : part.Record.Js.ToString());
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