using Orchard.ContentManagement;
using Orchard.DisplayManagement.Descriptors;

namespace CloudBust.Foundation.Shapes
{
    public class MenuItemLinkShapes : IShapeTableProvider
    {
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("MenuItemLink")
                .OnDisplaying(displaying =>
                {
                    var menuItem = displaying.Shape;
                    string menuName = menuItem.Menu.MenuName;
                    string contentType = null;
                    if (menuItem.Content != null)
                    {
                        contentType = ((IContent)menuItem.Content).ContentItem.ContentType;
                    }

                    if (contentType != null)
                    {
                        menuItem.Metadata.Alternates.Add("FoundationMenuItemLink__" + EncodeAlternateElement(contentType));
                    }

                    menuItem.Metadata.Alternates.Add("FoundationMenuItemLink__" + EncodeAlternateElement(menuName));

                    if (contentType != null)
                    {
                        menuItem.Metadata.Alternates.Add("FoundationMenuItemLink__" + EncodeAlternateElement(menuName) + "__" + EncodeAlternateElement(contentType));
                    }

                });
        }


        /// <summary>
        /// Encodes dashed and dots so that they don't conflict in filenames 
        /// </summary>
        /// <param name="alternateElement"></param>
        /// <returns></returns>
        private string EncodeAlternateElement(string alternateElement)
        {
            return alternateElement.Replace("-", "__").Replace(".", "_");
        }
    }
}