using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudBust.Localization
{
    public class RoutesTranslations : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            const string areaName = "CloudBust.Localization";
            const string controllerName = "Translations";
            return new[] {
                            new RouteDescriptor {
                                Route = new Route(
                                    "translations",
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
                                    "translations/add",
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
                                    "translations/edit/{id}",
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
                                    "translations/delete/{id}",
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