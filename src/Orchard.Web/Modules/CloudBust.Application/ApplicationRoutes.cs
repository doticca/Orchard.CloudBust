using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using System.Web.Http;

namespace CloudBust.Application
{
    public class ApplicationRoutes : IRouteProvider
    {
        public ApplicationRoutes()
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
                                                    Name = "GetApplications",
                                                    RouteTemplate = "v1/applications",
                                                    Defaults = new {
                                                            action = "GetApplications",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataApplications"
                                                        },
                                                    },

                            new HttpRouteDescriptor {
                                                    Name = "GetApplication",
                                                    RouteTemplate = "v1/applications({id})",
                                                    Defaults = new {
                                                            action = "GetApplication",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataApplications"
                                                        },
                                                    },
                            new HttpRouteDescriptor {
                                                    Name = "GetApplicationForUser",
                                                    RouteTemplate = "v1/application",
                                                    Defaults = new {
                                                            action = "GetApplication",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataApplications"
                                                        },
                                                    },

                            new HttpRouteDescriptor {
                                                    Name = "GetCategories",
                                                    RouteTemplate = "v1/categories",
                                                    Defaults = new { 
                                                            action = "GetCategories",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataApplications"
                                                        },
                                                    },
                            
                            new HttpRouteDescriptor {
                                                    Name = "GetCategory",
                                                    RouteTemplate = "v1/categories({id})",
                                                    Defaults = new { 
                                                            action = "GetCategory",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataApplications"
                                                        },
                                                    },                           

                            new HttpRouteDescriptor {
                                                    Name = "GeApplicationsCount",
                                                    RouteTemplate = "v1/applications/$count",
                                                    Defaults = new { 
                                                            action="GeApplicationsCount", 
                                                            area = "CloudBust.Application",
                                                            controller = "oDataApplications"
                                                        },
                                                    },
                            new HttpRouteDescriptor {
                                                    Name = "GetCategoriesCount",
                                                    RouteTemplate = "v1/categories/$count",
                                                    Defaults = new {
                                                            action="GetCategoriesCount",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataApplications"
                                                        },
                                                    },
                            new HttpRouteDescriptor {
                                                    Name = "FieldsForApplication",
                                                    RouteTemplate = "v1/applications({id})/{field}",
                                                    Defaults = new { 
                                                            action = "Field",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataFields"
                                                        },
                                                    },
                            new HttpRouteDescriptor {
                                                    Name = "FieldsForApplicationCategories",
                                                    RouteTemplate = "v1/categories({id})/{field}",
                                                    Defaults = new {
                                                            action = "Field",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataFields"
                                                        },
                                                    },
                            new HttpRouteDescriptor {
                                                    Name = "gamebykey",
                                                    RouteTemplate = "v1/games('{key}')",
                                                    Defaults = new { 
                                                            action = "GetGame",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataGame"
                                                        },
                                                    },
                            new HttpRouteDescriptor {
                                                    Name = "currentapplicationgames",
                                                    RouteTemplate = "v1/games",
                                                    Defaults = new { 
                                                            action = "GetGames",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataGame"
                                                        },
                                                    },
                            new HttpRouteDescriptor {
                                                    Name = "scorebygame",
                                                    RouteTemplate = "v1/games('{key}')/score",
                                                    Defaults = new { 
                                                            action = "Score",
                                                            area = "CloudBust.Application",
                                                            controller = "oDataGame"
                                                        },
                                                    },

                         };
        }
    }
}