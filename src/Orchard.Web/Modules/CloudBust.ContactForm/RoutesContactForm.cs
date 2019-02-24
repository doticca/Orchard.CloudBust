using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudBust.ContactForm
{
    public class RoutesContactForm : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            const string areaName = "CloudBust.ContactForm";
            const string controllerName = "Admin";
            return new[] {
                            new RouteDescriptor {
                                Route = new Route(
                                    "dashboard/contactform",
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
                                    "dashboard/contactform/add",
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
                                    "forms/contactform/add",
                                    new RouteValueDictionary {
                                        {"area", areaName},
                                        {"controller", "ContactForms"},
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
                                    "dashboard/contactform/edit/{id}",
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
                                    "dashboard/contactform/delete/{id}",
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