using Orchard.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudBust.Dashboard
{
    public class DashboardRoutes : IRouteProvider
    {
        public DashboardRoutes()
        {}

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {                   
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "Index"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "Applications"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/games",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "Games"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/datatables",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "Tables"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/games/create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "GameCreate"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/datatables/create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "TableCreate"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/games/{gameID}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "Game"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/games/{gameID}/keys",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "GameKeys"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/games/{gameID}/events",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "GameEvents"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },                     
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/games/{gameID}/events/create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "GameEventCreate"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },     
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/games/{gameID}/events/{eventID}/moveup",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "GameEventsUp"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },  
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/games/{gameID}/events/{eventID}/edit",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "GameEventEdit"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },  
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/games/{gameID}/events/{eventID}/delete",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "GameEventDelete"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/games/{gameID}/events/{eventID}/movedown",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "GameEventsDown"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },  
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationCreate"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "Application"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/keys",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationKeys"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/fb",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationFB"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/gc",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationGC"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/appstore",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationAppStore"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/smtp",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationSmtp"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/testmail",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationTestMail"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/sendmail",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationSendMail"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/userroles",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationUserRoles"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/userroles/create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "UserRoleCreate"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/userroles/{roleID}/edit",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "UserRoleEdit"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/userroles/{roleID}/delete",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "UserRoleDelete"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/users",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationUsers"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/users/{profileID}/edit",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationUserEdit"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/users/{profileID}/invites",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationUserInvites"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/users/{profileID}/friends",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationUserFriends"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/users/{profileID}/remove",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationUserRemove"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/delete",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationDelete"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/ast",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "AppSettings"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            //new RouteDescriptor {
                            //                         Route = new Route(
                            //                             "Dashboard/Applications/{appID}/tables",
                            //                             new RouteValueDictionary {
                            //                                                          {"area", "CloudBust.Dashboard"},
                            //                                                          {"controller", "Dashboard"},
                            //                                                          {"action", "ApplicationTables"}
                            //                                                      },
                            //                             new RouteValueDictionary(),
                            //                             new RouteValueDictionary {
                            //                                                          {"area", "CloudBust.Dashboard"}
                            //                                                      },
                            //                             new MvcRouteHandler())
                            //                     },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/senseapi",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "Senseapi"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/senseapi/games",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "SenseapiGames"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/senseapi/games/add",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "SenseapiGamesAdd"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/senseapi/games/{gameID}/add",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "SenseapiGamesAdd"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/senseapi/games/{gameID}/remove",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "SenseapiGamesRemove"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/senseapi/users",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "SenseapiUsers"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/senseapi/users/{userName}/sessions",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "SenseapiUsersSessions"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/senseapi/users/{userName}/{gameID}/sessions",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "SenseapiUsersSessionsGame"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },

                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/ast/create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "AppSettingsCreate"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/ast/categories",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "AppSettingsCategories"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/ast/categories/create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "AppSettingsCategoriesCreate"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },

                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/feeds",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "Feeds"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/blogs",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationBlogs"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/datatables",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationTables"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/datatables/add",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationTablesAdd"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/applications/{appID}/datatables/{datatableId}/add",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "ApplicationTablesAdd"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/datatables/{datatableID}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "Table"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/datatables/{datatableID}/fields",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "TableFields"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/datatables/{datatableID}/rows",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "TableRows"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/datatables/{datatableID}/rows/create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "TableRowCreate"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/datatables/{datatableID}/fields/create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "TableFieldCreate"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "dashboard/datatables/{datatableID}/fields/{fieldID}/edit",
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"},
                                                                                      {"controller", "Dashboard"},
                                                                                      {"action", "TableFieldEdit"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "CloudBust.Dashboard"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },

            };
        }
    }
}