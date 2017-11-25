using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using Orchard.Environment.Extensions;

namespace CloudBust.Application
{
    [OrchardFeature("CloudBust.Application.WebApp")]
    public class AccountRoutes : IRouteProvider
    {
        public AccountRoutes()
        {
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {

                             new RouteDescriptor {
                                                     Priority = -4,
                                                     Route = new Route(
                                                         "orchard_admin",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Dashboard"},
                                                                                      {"controller", "admin"},
                                                                                      {"action", "index"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                    Priority = 9,
                                                    Route = new Route(
                                                        "users/accessdenied",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Account"},
                                                                                    {"action", "AccessDenied"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    },
                            new RouteDescriptor {
                                                    Priority = 9,
                                                    Route = new Route(
                                                        "users/logoff",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Account"},
                                                                                    {"action", "LogOff"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    },
                            new RouteDescriptor {
                                                    Priority = 9,
                                                    Route = new Route(
                                                        "users/logon",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Account"},
                                                                                    {"action", "LogOn"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    },
                            new RouteDescriptor {
                                                    Priority = 9,
                                                    Route = new Route(
                                                        "users/register",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Account"},
                                                                                    {"action", "Register"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    },
                            new RouteDescriptor {
                                                    Priority = 9,
                                                    Route = new Route(
                                                        "Users/i",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Account"},
                                                                                    {"action", "Invitation"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    },
                            new RouteDescriptor {
                                                    Priority = 9,
                                                    Route = new Route(
                                                        "users/requestlostpassword",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Account"},
                                                                                    {"action", "RequestLostPassword"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    },
                            new RouteDescriptor {
                                                    Priority = 9,
                                                    Route = new Route(
                                                        "users/lostpassword",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Account"},
                                                                                    {"action", "LostPassword"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    },
                            new RouteDescriptor {
                                                    Priority = 9,
                                                    Route = new Route(
                                                        "users/changepasswordsuccess",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Account"},
                                                                                    {"action", "ChangePasswordSuccess"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    },
                         };
        }
    }
}