using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using Orchard.Environment.Extensions;

namespace CloudBust.Common {

    [OrchardFeature("CloudBust.Common.SEO")]
    public class ApplinksRoutes : IRouteProvider {
		public void GetRoutes(ICollection<RouteDescriptor> routes) {
			foreach (var routeDescriptor in GetRoutes())
				routes.Add(routeDescriptor);
		}

		public IEnumerable<RouteDescriptor> GetRoutes() {
			return new[] {
								new RouteDescriptor {   Priority = 5,
														Route = new Route(
                                                            "apple-app-site-association",
															new RouteValueDictionary {
																						{"area", "CloudBust.Common"},
																						{"controller", "Applinks"},
																						{"action", "Index"}
															},
															new RouteValueDictionary(),
															new RouteValueDictionary {
																						{"area", "CloudBust.Common"}
															},
															new MvcRouteHandler())
								},
							};
		}
	}
}