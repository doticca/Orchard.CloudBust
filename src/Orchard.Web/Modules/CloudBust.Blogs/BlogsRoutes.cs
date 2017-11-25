using Orchard.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Blogs
{
    public class BlogsRoutes : IRouteProvider
    {
        public BlogsRoutes()
        {}

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {                   
                            new HttpRouteDescriptor {
                                    Name = "blogbyname",
                                    RouteTemplate = "v1/blogs('{blogname}')",
                                    Defaults = new { 
                                            action = "Info",
                                            area = "CloudBust.Blogs",
                                            controller = "blogs"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "blogslist",
                                    RouteTemplate = "v1/blogs",
                                    Defaults = new { 
                                            action = "Blogs",
                                            area = "CloudBust.Blogs",
                                            controller = "blogs"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "postsbyblog",
                                    RouteTemplate = "v1/blogs('{blogname}')/posts",
                                    Defaults = new { 
                                            action = "Posts",
                                            area = "CloudBust.Blogs",
                                            controller = "blogs"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "viewrsbypost",
                                    RouteTemplate = "v1/blogpost/{id}/views",
                                    Defaults = new {
                                            action = "Posts",
                                            area = "CloudBust.Blogs",
                                            controller = "blogs"
                                        },
                                    },
            };
        }
    }
}