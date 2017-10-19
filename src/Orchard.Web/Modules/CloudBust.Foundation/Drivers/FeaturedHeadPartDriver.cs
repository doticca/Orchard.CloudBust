using CloudBust.Foundation.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using System;

namespace CloudBust.Foundation.Drivers {
    public class FeaturedHeadPartDriver : ContentPartDriver<FeaturedHeadPart>
    {
        public FeaturedHeadPartDriver()
        {

        }
        protected override DriverResult Display(FeaturedHeadPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_FeaturedHead",
                            () => shapeHelper.Parts_FeaturedHead());
        }
        protected override DriverResult Editor(FeaturedHeadPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_FeaturedHead_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts/FeaturedHead.Edit", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(FeaturedHeadPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater != null) {
                updater.TryUpdateModel(part, Prefix, null, null);
            }

            return Editor(part, shapeHelper);
        }

        protected override void Importing(FeaturedHeadPart part, Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.HeightMedium = GetAttribute<int>(context, partName, "HeightMedium");
            part.Record.HeightLarge = GetAttribute<int>(context, partName, "HeightLarge");
            part.Record.BackgroundColor = GetAttribute<string>(context, partName, "BackgroundColor");
            part.Record.BackgroundColorMedium = GetAttribute<string>(context, partName, "BackgroundColorMedium");
            part.Record.BackgroundColorLarge = GetAttribute<string>(context, partName, "BackgroundColorLarge");
            part.Record.BackgroundImageLarge = GetAttribute<string>(context, partName, "BackgroundImageLarge");
            part.Record.BackgroundImageMedium = GetAttribute<string>(context, partName, "BackgroundImageMedium");
        }

        protected override void Exporting(FeaturedHeadPart part, Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("HeightMedium", part.HeightMedium);
            element.SetAttributeValue("HeightLarge", part.HeightLarge);
            element.SetAttributeValue("BackgroundColor", part.BackgroundColor);
            element.SetAttributeValue("BackgroundColorMedium", part.BackgroundColorMedium);
            element.SetAttributeValue("BackgroundColorLarge", part.BackgroundColorLarge);
            element.SetAttributeValue("BackgroundImageLarge", part.BackgroundImageLarge);
            element.SetAttributeValue("BackgroundImageMedium", part.BackgroundImageMedium);
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