using CloudBust.Foundation.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using System;

namespace CloudBust.Foundation.Drivers {
    public class TrioPromoPartDriver : ContentPartDriver<TrioPromoPart>
    {
        public TrioPromoPartDriver()
        {

        }
        protected override DriverResult Display(TrioPromoPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_TrioPromo",
                            () => shapeHelper.Parts_TrioPromo());
        }
        protected override DriverResult Editor(TrioPromoPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_TrioPromo_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts/TrioPromo.Edit", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(TrioPromoPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater != null) {
                updater.TryUpdateModel(part, Prefix, null, null);
            }

            return Editor(part, shapeHelper);
        }

        protected override void Importing(TrioPromoPart part, Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.FirstImage = GetAttribute<string>(context, partName, "FirstImage");
            part.Record.SecondImage = GetAttribute<string>(context, partName, "SecondImage");
            part.Record.ThirdImage = GetAttribute<string>(context, partName, "ThirdImage");
            part.Record.FirstTitle = GetAttribute<string>(context, partName, "FirstTitle");
            part.Record.SecondTitle = GetAttribute<string>(context, partName, "SecondTitle");
            part.Record.ThirdTitle = GetAttribute<string>(context, partName, "ThirdTitle");
            part.Record.FirstPromoText = GetAttribute<string>(context, partName, "FirstPromoText");
            part.Record.SecondPromoText = GetAttribute<string>(context, partName, "SecondPromoText");
            part.Record.ThirdPromoText = GetAttribute<string>(context, partName, "ThirdPromoText");
            part.Record.FirstLinkText = GetAttribute<string>(context, partName, "FirstLinkText");
            part.Record.SecondLinkText = GetAttribute<string>(context, partName, "SecondLinkText");
            part.Record.ThirdLinkText = GetAttribute<string>(context, partName, "ThirdLinkText");
            part.Record.FirstLinkUrl = GetAttribute<string>(context, partName, "FirstLinkUrl");
            part.Record.SecondLinkUrl = GetAttribute<string>(context, partName, "SecondLinkUrl");
            part.Record.ThirdLinkUrl = GetAttribute<string>(context, partName, "ThirdLinkUrl");
            part.Record.FirstLinkColor = GetAttribute<string>(context, partName, "FirstLinkColor");
            part.Record.SecondLinkColor = GetAttribute<string>(context, partName, "SecondLinkColor");
            part.Record.ThirdLinkColor = GetAttribute<string>(context, partName, "ThirdLinkColor");
        }

        protected override void Exporting(TrioPromoPart part, Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("FirstImage", part.FirstImage);
            element.SetAttributeValue("SecondImage", part.SecondImage);
            element.SetAttributeValue("ThirdImage", part.ThirdImage);
            element.SetAttributeValue("FirstTitle", part.FirstTitle);
            element.SetAttributeValue("SecondTitle", part.SecondTitle);
            element.SetAttributeValue("ThirdTitle", part.ThirdTitle);
            element.SetAttributeValue("FirstPromoText", part.FirstPromoText);
            element.SetAttributeValue("SecondPromoText", part.SecondPromoText);
            element.SetAttributeValue("ThirdPromoText", part.ThirdPromoText);
            element.SetAttributeValue("FirstLinkText", part.FirstLinkText);
            element.SetAttributeValue("SecondLinkText", part.SecondLinkText);
            element.SetAttributeValue("ThirdLinkText", part.ThirdLinkText);
            element.SetAttributeValue("FirstLinkUrl", part.FirstLinkUrl);
            element.SetAttributeValue("SecondLinkUrl", part.SecondLinkUrl);
            element.SetAttributeValue("ThirdLinkUrl", part.ThirdLinkUrl);
            element.SetAttributeValue("FirstLinkColor", part.FirstLinkColor);
            element.SetAttributeValue("SecondLinkColor", part.SecondLinkColor);
            element.SetAttributeValue("ThirdLinkColor", part.ThirdLinkColor);
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