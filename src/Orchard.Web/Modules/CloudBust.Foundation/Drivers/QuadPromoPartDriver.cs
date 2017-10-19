using CloudBust.Foundation.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using System;

namespace CloudBust.Foundation.Drivers {
    public class QuadPromoPartDriver : ContentPartDriver<QuadPromoPart>
    {
        public QuadPromoPartDriver()
        {

        }
        protected override DriverResult Display(QuadPromoPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_QuadPromo",
                            () => shapeHelper.Parts_QuadPromo());
        }
        protected override DriverResult Editor(QuadPromoPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_QuadPromo_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts/QuadPromo.Edit", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(QuadPromoPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater != null) {
                updater.TryUpdateModel(part, Prefix, null, null);
            }

            return Editor(part, shapeHelper);
        }

        protected override void Importing(QuadPromoPart part, Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.FirstImage = GetAttribute<string>(context, partName, "FirstImage");
            part.Record.SecondImage = GetAttribute<string>(context, partName, "SecondImage");
            part.Record.ThirdImage = GetAttribute<string>(context, partName, "ThirdImage");
            part.Record.FourthImage = GetAttribute<string>(context, partName, "FourthImage");
            part.Record.FirstTitle = GetAttribute<string>(context, partName, "FirstTitle");
            part.Record.SecondTitle = GetAttribute<string>(context, partName, "SecondTitle");
            part.Record.ThirdTitle = GetAttribute<string>(context, partName, "ThirdTitle");
            part.Record.FourthTitle = GetAttribute<string>(context, partName, "FourthTitle");
            part.Record.FirstPromoText = GetAttribute<string>(context, partName, "FirstPromoText");
            part.Record.SecondPromoText = GetAttribute<string>(context, partName, "SecondPromoText");
            part.Record.ThirdPromoText = GetAttribute<string>(context, partName, "ThirdPromoText");
            part.Record.FourthPromoText = GetAttribute<string>(context, partName, "FourthPromoText");
            part.Record.FirstLinkText = GetAttribute<string>(context, partName, "FirstLinkText");
            part.Record.SecondLinkText = GetAttribute<string>(context, partName, "SecondLinkText");
            part.Record.ThirdLinkText = GetAttribute<string>(context, partName, "ThirdLinkText");
            part.Record.FourthLinkText = GetAttribute<string>(context, partName, "FourthLinkText");
            part.Record.FirstLinkUrl = GetAttribute<string>(context, partName, "FirstLinkUrl");
            part.Record.SecondLinkUrl = GetAttribute<string>(context, partName, "SecondLinkUrl");
            part.Record.ThirdLinkUrl = GetAttribute<string>(context, partName, "ThirdLinkUrl");
            part.Record.FourthLinkUrl = GetAttribute<string>(context, partName, "FourthLinkUrl");
            part.Record.FirstLinkColor = GetAttribute<string>(context, partName, "FirstLinkColor");
            part.Record.SecondLinkColor = GetAttribute<string>(context, partName, "SecondLinkColor");
            part.Record.ThirdLinkColor = GetAttribute<string>(context, partName, "ThirdLinkColor");
            part.Record.FourthLinkColor = GetAttribute<string>(context, partName, "FourthLinkColor");
        }

        protected override void Exporting(QuadPromoPart part, Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("FirstImage", part.FirstImage);
            element.SetAttributeValue("SecondImage", part.SecondImage);
            element.SetAttributeValue("ThirdImage", part.ThirdImage);
            element.SetAttributeValue("FourthImage", part.FourthImage);
            element.SetAttributeValue("FirstTitle", part.FirstTitle);
            element.SetAttributeValue("SecondTitle", part.SecondTitle);
            element.SetAttributeValue("ThirdTitle", part.ThirdTitle);
            element.SetAttributeValue("FourthTitle", part.FourthTitle);
            element.SetAttributeValue("FirstPromoText", part.FirstPromoText);
            element.SetAttributeValue("SecondPromoText", part.SecondPromoText);
            element.SetAttributeValue("ThirdPromoText", part.ThirdPromoText);
            element.SetAttributeValue("FourthPromoText", part.FourthPromoText);
            element.SetAttributeValue("FirstLinkText", part.FirstLinkText);
            element.SetAttributeValue("SecondLinkText", part.SecondLinkText);
            element.SetAttributeValue("ThirdLinkText", part.ThirdLinkText);
            element.SetAttributeValue("FourthLinkText", part.FourthLinkText);
            element.SetAttributeValue("FirstLinkUrl", part.FirstLinkUrl);
            element.SetAttributeValue("SecondLinkUrl", part.SecondLinkUrl);
            element.SetAttributeValue("ThirdLinkUrl", part.ThirdLinkUrl);
            element.SetAttributeValue("FourthLinkUrl", part.FourthLinkUrl);
            element.SetAttributeValue("FirstLinkColor", part.FirstLinkColor);
            element.SetAttributeValue("SecondLinkColor", part.SecondLinkColor);
            element.SetAttributeValue("ThirdLinkColor", part.ThirdLinkColor);
            element.SetAttributeValue("FourthLinkColor", part.FourthLinkColor);        
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