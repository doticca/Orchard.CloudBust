using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using Orchard.Environment.Extensions;

namespace CloudBust.Application
{
    //[OrchardFeature("CloudBust.Application.WebApp")]
    //public class BlogsRoutes : IRouteProvider
    //{
    //    public BlogsRoutes()
    //    {
    //    }

    //    public void GetRoutes(ICollection<RouteDescriptor> routes)
    //    {
    //        foreach (var routeDescriptor in GetRoutes())
    //            routes.Add(routeDescriptor);
    //    }

    //    public IEnumerable<RouteDescriptor> GetRoutes()
    //    {
    //        return new[] {                            
    //                         new RouteDescriptor {
    //                                                Priority = 10,
    //                                                 Route = new Route(
    //                                                     "Blogs",
    //                                                     new RouteValueDictionary {
    //                                                                                  {"area", "CloudBust.Blogs"},
    //                                                                                  {"controller", "Blog"},
    //                                                                                  {"action", "List"}
    //                                                                              },
    //                                                     new RouteValueDictionary(),
    //                                                     new RouteValueDictionary {
    //                                                                                  {"area", "CloudBust.Blogs"}
    //                                                                              },
    //                                                     new MvcRouteHandler())
    //                                             }
    //                     };
    //    }
    //}
}