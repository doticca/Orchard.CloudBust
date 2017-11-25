using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using System.Web.Http;

namespace CloudBust.Application
{
    public class ParametersRoutes : IRouteProvider
    {
        public ParametersRoutes()
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
                                        Name = "GetParameterCategoriesForApplication",
                                        RouteTemplate = "v1/applications({applicationId})/parameters/categories",
                                        Defaults = new { 
                                                action = "GetParameterCategoriesForApplication",
                                                area = "CloudBust.Application",
                                                controller = "oDataParameters"
                                            },
                                        },
                new HttpRouteDescriptor {
                                        Name = "GetParameterCategoriesForApplicationCount",
                                        RouteTemplate = "v1/applications({applicationId})/parameters/categories/$count",
                                        Defaults = new { 
                                                action="GetParameterCategoriesForApplicationCount", 
                                                area = "CloudBust.Application",
                                                controller = "oDataParameters"
                                            },
                                        },
                new HttpRouteDescriptor {
                                        Name = "GetParameterCategoryFieldsForApplication",
                                        RouteTemplate = "v1/applications({applicationId})/parameters/categories('{parameterCategoryName}')/{field}",
                                        Defaults = new {
                                                action = "GetParameterCategoryFieldsForApplication",
                                                area = "CloudBust.Application",
                                                controller = "oDataParameters"
                                            },
                                        },
                new HttpRouteDescriptor {
                                        Name = "GetParameterCategoryByNameForApplication",
                                        RouteTemplate = "v1/applications({applicationId})/parameters/categories('{parameterCategoryName}')",
                                        Defaults = new {
                                                action = "GetParameterCategoryByNameForApplication",
                                                area = "CloudBust.Application",
                                                controller = "oDataParameters"
                                            },
                                        },
                new HttpRouteDescriptor {
                                        Name = "GetParameterCategory",
                                        RouteTemplate = "v1/parameters/categories({parameterCategoryId})",
                                        Defaults = new { 
                                                action = "GetParameterCategory",
                                                area = "CloudBust.Application",
                                                controller = "oDataParameters"
                                            },
                                        },
                new HttpRouteDescriptor {
                                        Name = "GetParameterCategoryFields",
                                        RouteTemplate = "v1/parameters/categories({parameterCategoryId})/{field}",
                                        Defaults = new { 
                                                action = "Field",
                                                area = "CloudBust.Application",
                                                controller = "oDataFields"
                                            },
                                        },

                           

                         };
        }
    }
}