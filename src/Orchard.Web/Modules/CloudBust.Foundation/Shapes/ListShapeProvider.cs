using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement.Descriptors;
using System.Collections.Generic;
using Orchard.UI.Admin;
using System.Linq;
using Orchard.Environment;

namespace Themes.Plaisio.Blogs
{    
    public class ListShapeProvider : IShapeTableProvider
    {
        private readonly Work<WorkContext> _workContext;

        public ListShapeProvider(
          Work<WorkContext> workContext)
        {

            _workContext = workContext;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("List")
                    .OnDisplaying(displaying =>
                    {
                        var request = _workContext.Value.HttpContext.Request;
                        if (AdminFilter.IsApplied(request.RequestContext)) return;

                        IList<dynamic> Items =
                            displaying.Shape.Items;

                        if(Items!=null && Items.Count > 0)
                        {
                            int found = 0;
                            foreach(dynamic item in Items)
                            {
                                ContentItem cItem = item.ContentItem;
                                if (cItem != null)
                                    if (cItem.ContentType == "BlogPost")
                                        found++;
                            }
                            if(found>0 && found == Items.Count)
                                displaying.ShapeMetadata.Alternates.Add("Parts_Blogs_BlogPost_List");
                        }
                    });           
        }
    }
}