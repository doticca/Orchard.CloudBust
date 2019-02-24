using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Q42.DbTranslations
{
    public class Routes : IRouteProvider {
        public IEnumerable<RouteDescriptor> GetRoutes() {
            const string areaName = "Q42.DbTranslations";
            const string controllerName = "Dashboard";
            return new[] {
                new RouteDescriptor
                {
                    Route = new Route(
                        "system/statictranslations",
                        new RouteValueDictionary
                        {
                            {"area", areaName},
                            {"controller", controllerName},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", areaName}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Route = new Route(
                        "system/statictranslations/import",
                        new RouteValueDictionary
                        {
                            {"area", areaName},
                            {"controller", controllerName},
                            {"action", "Import"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", areaName}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Route = new Route(
                        "system/statictranslations/search",
                        new RouteValueDictionary
                        {
                            {"area", areaName},
                            {"controller", controllerName},
                            {"action", "Search"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", areaName}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Route = new Route(
                        "system/statictranslations/culture/{culture}",
                        new RouteValueDictionary {
                                                    {"area", areaName},
                                                    {"controller", controllerName},
                                                    {"action", "Culture"}
                                                },
                        new RouteValueDictionary {
                                                    {"culture", ".+"}
                                                },
                        new RouteValueDictionary {
                                                    {"area", areaName}
                                                },
                        new MvcRouteHandler())
                },       
                new RouteDescriptor {
                    Route = new Route(
                        "system/statictranslations/culture/{culture}/details",
                        new RouteValueDictionary {
                            {"area", areaName},
                            {"controller", controllerName},
                            {"action", "Details"}
                        },
                        new RouteValueDictionary {
                            {"culture", ".+"}
                        },
                        new RouteValueDictionary {
                            {"area", areaName}
                        },
                        new MvcRouteHandler())
                },                
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (RouteDescriptor routeDescriptor in GetRoutes()) {
                routes.Add(routeDescriptor);
            }
        }
    }
}