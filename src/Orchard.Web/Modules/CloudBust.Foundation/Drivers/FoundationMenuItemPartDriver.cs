using CloudBust.Foundation.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Security;
using System;

namespace CloudBust.Foundation.Drivers {
    public class FoundationMenuItemPartDriver : ContentPartDriver<FoundationMenuItemPart> {
        private readonly IAuthorizationService _authorizationService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public FoundationMenuItemPartDriver(IAuthorizationService authorizationService, IWorkContextAccessor workContextAccessor)
        {
            _authorizationService = authorizationService;
            _workContextAccessor = workContextAccessor;
        }

        protected override DriverResult Editor(FoundationMenuItemPart part, dynamic shapeHelper)
        {
            var currentUser = _workContextAccessor.GetContext().CurrentUser;
            //if (!_authorizationService.TryCheckAccess(Permissions.ManageMainMenu, currentUser, part))
            //    return null;

            return ContentShape("Parts_FoundationMenuItem_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts/FoundationMenuItem.Edit", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(FoundationMenuItemPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var currentUser = _workContextAccessor.GetContext().CurrentUser;
            //if (!_authorizationService.TryCheckAccess(Permissions.ManageMainMenu, currentUser, part))
            //    return null;

            if (updater != null) {
                updater.TryUpdateModel(part, Prefix, null, null);
            }

            return Editor(part, shapeHelper);
        }

        protected override void Importing(FoundationMenuItemPart part, Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.DisplayAsButton = GetAttribute<bool>(context, partName, "DisplayAsButton");
            part.Record.isRoot = GetAttribute<bool>(context, partName, "isRoot");
            part.Record.LeftSide = GetAttribute<bool>(context, partName, "LeftSide");
            part.Record.Divider = GetAttribute<bool>(context, partName, "Divider");
        }

        protected override void Exporting(FoundationMenuItemPart part, Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("DisplayAsButton", part.DisplayAsButton);
            element.SetAttributeValue("isRoot", part.isRoot);
            element.SetAttributeValue("LeftSide", part.LeftSide); 
            element.SetAttributeValue("Divider", part.Divider);
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