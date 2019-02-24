using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace CloudBust.Tenants {
    public class Routes : IRouteProvider {
        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                            new RouteDescriptor {
                                Route = new Route(
                                    "cb/system/tenants",
                                    new RouteValueDictionary {
                                                                {"area", "CloudBust.Tenants"},
                                                                {"controller", "Dashboard"},
                                                                {"action", "Index"}
                                                            },
                                    new RouteValueDictionary(),
                                    new RouteValueDictionary {
                                                                {"area", "CloudBust.Tenants"}
                                                            },
                                    new MvcRouteHandler())
                                },
                            new RouteDescriptor {
                                Route = new Route(
                                    "cb/system/tenants/add",
                                    new RouteValueDictionary {
                                                                {"area", "CloudBust.Tenants"},
                                                                {"controller", "Dashboard"},
                                                                {"action", "Add"}
                                                            },
                                    new RouteValueDictionary(),
                                    new RouteValueDictionary {
                                                                {"area", "CloudBust.Tenants"}
                                                            },
                                    new MvcRouteHandler())
                                },
                            new RouteDescriptor {
                                Route = new Route(
                                    "cb/system/tenants/edit/{name}",
                                    new RouteValueDictionary {
                                                                {"area", "CloudBust.Tenants"},
                                                                {"controller", "Dashboard"},
                                                                {"action", "Edit"}
                                                            },
                                    new RouteValueDictionary {
                                                                {"name", ".+"}
                                                            },
                                    new RouteValueDictionary {
                                                                {"area", "CloudBust.Tenants"}
                                                            },
                                    new MvcRouteHandler())
                                },
                            new RouteDescriptor {
                                Route = new Route(
                                    "cb/system/tenants/reset/{name}",
                                    new RouteValueDictionary {
                                                                {"area", "CloudBust.Tenants"},
                                                                {"controller", "Dashboard"},
                                                                {"action", "Reset"}
                                                            },
                                    new RouteValueDictionary {
                                                                {"name", ".+"}
                                                            },
                                    new RouteValueDictionary {
                                                                {"area", "CloudBust.Tenants"}
                                                            },
                                    new MvcRouteHandler())
                                }
                         };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (RouteDescriptor routeDescriptor in GetRoutes()) {
                routes.Add(routeDescriptor);
            }
        }
    }
}