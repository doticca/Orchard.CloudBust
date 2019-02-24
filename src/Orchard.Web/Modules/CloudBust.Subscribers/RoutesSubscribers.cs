using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudBust.Subscribers
{
    public class RoutesSubscribers : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            const string areaName = "CloudBust.Subscribers";
            const string controllerName = "Admin";
            return new[] {
                            new RouteDescriptor {
                                Route = new Route(
                                    "dashboard/subscribers",
                                    new RouteValueDictionary {
                                                                {"area", areaName},
                                                                {"controller", controllerName},
                                                                {"action", "Index"}
                                                            },
                                    new RouteValueDictionary(),
                                    new RouteValueDictionary {
                                                                {"area", areaName}
                                                            },
                                    new MvcRouteHandler())
                                },
                            new RouteDescriptor {
                                Route = new Route(
                                    "dashboard/subscribers/add",
                                    new RouteValueDictionary {
                                                                {"area", areaName},
                                                                {"controller", controllerName},
                                                                {"action", "Add"}
                                                            },
                                    new RouteValueDictionary(),
                                    new RouteValueDictionary {
                                                                {"area", areaName}
                                                            },
                                    new MvcRouteHandler())
                                },
                            new RouteDescriptor {
                                Route = new Route(
                                    "forms/subscribers/add",
                                    new RouteValueDictionary {
                                        {"area", areaName},
                                        {"controller", "Subscribers"},
                                        {"action", "Add"}
                                    },
                                    new RouteValueDictionary(),
                                    new RouteValueDictionary {
                                        {"area", areaName}
                                    },
                                    new MvcRouteHandler())
                            },
                            new RouteDescriptor {
                                Route = new Route(
                                    "dashboard/subscribers/edit/{id}",
                                    new RouteValueDictionary {
                                                                {"area", areaName},
                                                                {"controller", controllerName},
                                                                {"action", "Edit"}
                                                            },
                                    new RouteValueDictionary {
                                                                {"id", ".+"}
                                                            },
                                    new RouteValueDictionary {
                                                                {"area", areaName}
                                                            },
                                    new MvcRouteHandler())
                                },
                            new RouteDescriptor {
                                Route = new Route(
                                    "dashboard/subscribers/delete/{id}",
                                    new RouteValueDictionary {
                                                                {"area", areaName},
                                                                {"controller", controllerName},
                                                                {"action", "Delete"}
                                                            },
                                    new RouteValueDictionary {
                                                                {"id", ".+"}
                                                            },
                                    new RouteValueDictionary {
                                                                {"area", areaName}
                                                            },
                                    new MvcRouteHandler())
                                },
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (RouteDescriptor routeDescriptor in GetRoutes())
            {
                routes.Add(routeDescriptor);
            }
        }
    }
}