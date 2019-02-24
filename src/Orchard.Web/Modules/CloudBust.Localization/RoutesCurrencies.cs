using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudBust.Localization
{
    public class RoutesCurrencies : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            const string areaName = "CloudBust.Localization";
            const string controllerName = "Currencies";
            return new[] {
                            new RouteDescriptor {
                                Route = new Route(
                                    "currencies",
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
                                    "currencies/add",
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
                                    "currencies/edit/{id}",
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
                                    "currencies/delete/{id}",
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