using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudBust.Localization
{
    public class RoutesCountries : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            const string controllerName = "Countries";
            const string areaName = "CloudBust.Localization";
            return new[] {
                            new RouteDescriptor {
                                Route = new Route(
                                    "countries",
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
                                    "countries/add",
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
                                    "countries/edit/{id}",
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
                                    "countries/delete/{id}",
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