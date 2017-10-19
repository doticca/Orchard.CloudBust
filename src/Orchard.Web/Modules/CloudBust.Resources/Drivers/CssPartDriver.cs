using CloudBust.Resources.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.Drivers;
using System;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources.Drivers
{
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class CssPartDriver : ContentPartDriver<CssPart>
    {
        private const string TemplateName = "Parts/Css";
        protected override string Prefix
        {
            get { return "Css"; }
        }
        protected override DriverResult Display(CssPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Css",
                () => shapeHelper.Parts_Css(Css: part));
        }

        protected override DriverResult Editor(CssPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Css_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(CssPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return ContentShape("Parts_Css_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override void Importing(CssPart part, Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            string partName = part.PartDefinition.Name;
            part.Record.Css = GetAttribute<string>(context, partName, "Css");
        }

        protected override void Exporting(CssPart part, Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            string partName = part.PartDefinition.Name;
            context.Element(partName).SetAttributeValue("Css", string.IsNullOrWhiteSpace(part.Record.Css) ? string.Empty : part.Record.Css.ToString());
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