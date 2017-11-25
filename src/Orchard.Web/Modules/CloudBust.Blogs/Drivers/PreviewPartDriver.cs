using CloudBust.Blogs.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.Drivers;
using System;
using Orchard.ContentManagement;

namespace CloudBust.Blogs.Drivers
{
    public class CssPartDriver : ContentPartDriver<PreviewPart>
    {
        private const string TemplateName = "Parts/Preview";
        protected override string Prefix
        {
            get { return "Preview"; }
        }
        protected override DriverResult Display(PreviewPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Preview",
                () => shapeHelper.Parts_Preview(ContentItem: part));
        }

        protected override DriverResult Editor(PreviewPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Preview_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(PreviewPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return ContentShape("Parts_Preview_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override void Importing(PreviewPart part, Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            string partName = part.PartDefinition.Name;
            part.Record.PreviewText = GetAttribute<string>(context, partName, "PreviewText");
        }

        protected override void Exporting(PreviewPart part, Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            string partName = part.PartDefinition.Name;
            context.Element(partName).SetAttributeValue("PreviewText", string.IsNullOrWhiteSpace(part.Record.PreviewText) ? string.Empty : part.Record.PreviewText.ToString());
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