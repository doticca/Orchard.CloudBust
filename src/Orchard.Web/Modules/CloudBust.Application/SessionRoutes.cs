using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using System.Web.Http;

namespace CloudBust.Application
{
    public class SessionRoutes : IRouteProvider
    {
        public SessionRoutes()
        { }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                            new HttpRouteDescriptor {
                                                    Name = "startsession",
                                                    RouteTemplate = "v1/games('{key}')/session/start",
                                                    Defaults = new { 
                                                            action = "SessionStart",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataSession"
                                                        },
                                                    },
                            new HttpRouteDescriptor {
                                                    Name = "endsession",
                                                    RouteTemplate = "v1/games('{key}')/session({id})/end",
                                                    Defaults = new { 
                                                            action = "SessionEnd",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataSession"
                                                        },
                                                    },
                            new HttpRouteDescriptor {
                                                    Name = "startsessionevent",
                                                    RouteTemplate = "v1/games('{key}')/session({id})/startevent",
                                                    Defaults = new { 
                                                            action = "SessionEventStart",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataSession"
                                                        },
                                                    },
                            new HttpRouteDescriptor {
                                                    Name = "endsessionevent",
                                                    RouteTemplate = "v1/games('{key}')/session({id})/endevent",
                                                    Defaults = new { 
                                                            action = "SessionEventEnd",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataSession"
                                                        },
                                                    },                                                                      
                         };
        }
    }
}