using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using Orchard.Environment.Extensions;

namespace CloudBust.Application
{
    [OrchardFeature("CloudBust.Application.WebApp")]
    public class EmailRoutes : IRouteProvider {

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                            new RouteDescriptor {
                                                    Priority = 100,
                                                    Route = new Route(
                                                        "users/challengesent",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Email"},
                                                                                    {"action", "ChallengeEmailSent"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    },
                            new RouteDescriptor {
                                                    Priority = 100,
                                                    Route = new Route(
                                                        "users/challengesuccess",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Email"},
                                                                                    {"action", "ChallengeEmailSuccess"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    },
                            new RouteDescriptor {
                                                    Priority = 100,
                                                    Route = new Route(
                                                        "users/challengeemail",
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"},
                                                                                    {"controller", "Email"},
                                                                                    {"action", "ChallengeEmail"}
                                                                                },
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary {
                                                                                    {"area", "CloudBust.Application"}
                                                                                },
                                                        new MvcRouteHandler())
                                                    }
                         };
        }
    }
}