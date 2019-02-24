using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using CloudBust.LogViewer.CustomUtils;
using Orchard.Mvc.Routes;

namespace CloudBust.LogViewer
{
    public class Routes : IRouteProvider
    {
        #region Attributes
        private const string AdminController = "Admin";

        private const string AdminPrefix = "Admin/Log";
        #endregion

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                GetRoute(AdminPrefix, null, LogConstants.ModulePath, AdminController, "Index"),
                GetRoute(AdminPrefix, "Delete/{File}", LogConstants.ModulePath, AdminController, "Delete"),
                GetRoute(AdminPrefix, "Save/{File}", LogConstants.ModulePath, AdminController, "Save")
            };
        }

        private static RouteDescriptor GetRoute(string aliasPrefix, string aliasPostfix, string modulePath, string controller, string action)
        {
            var defaults = new Dictionary<string, object>
            {
                {"area", modulePath},
                {"controller", controller},
                {"action", action},
            };

            return new RouteDescriptor
            {
                Priority = -5,
                Route = new Route(
                    String.IsNullOrEmpty(aliasPostfix) ? aliasPrefix : String.Format("{0}/{1}", aliasPrefix, aliasPostfix),
                    new RouteValueDictionary(defaults),
                    new RouteValueDictionary(),
                    new RouteValueDictionary
                    {
                        {"area", modulePath}
                    },
                    new MvcRouteHandler())
            };
        }
    }
}