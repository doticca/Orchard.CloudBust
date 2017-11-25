using Orchard.Mvc.Routes;
using System.Collections.Generic;

namespace CloudBust.Application
{
    public class UserProfileRoutes : IRouteProvider
    {
        public UserProfileRoutes()
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
                                    Name = "pingtime",
                                    RouteTemplate = "v1/ping",
                                    Defaults = new {
                                            action = "Ping",
                                            area = "CloudBust.Application",
                                            controller = "login"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "securedping",
                                    RouteTemplate = "v1/user/ping",
                                    Defaults = new {
                                            action = "sPing",
                                            area = "CloudBust.Application",
                                            controller = "login"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "pingusertime",
                                    RouteTemplate = "v1/user/time",
                                    Defaults = new {
                                            action = "PingUser",
                                            area = "CloudBust.Application",
                                            controller = "login"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "profilelogin",
                                    RouteTemplate = "v1/user/login",
                                    Defaults = new { 
                                            action = "Login",
                                            area = "CloudBust.Application",
                                            controller = "login"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "facebooklogin",
                                    RouteTemplate = "v1/user/login/facebook",
                                    Defaults = new { 
                                            action = "LoginFacebook",
                                            area = "CloudBust.Application",
                                            controller = "login"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "gamecenterlogin",
                                    RouteTemplate = "v1/user/login/gamecenter",
                                    Defaults = new {
                                            action = "LoginGameCenter",
                                            area = "CloudBust.Application",
                                            controller = "login"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "profilelogout",
                                    RouteTemplate = "v1/user/logout",
                                    Defaults = new { 
                                            action = "LogOut",
                                            area = "CloudBust.Application",
                                            controller = "login"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "profileregister",
                                    RouteTemplate = "v1/user/register",
                                    Defaults = new { 
                                            action = "Register",
                                            area = "CloudBust.Application",
                                            controller = "login"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "profileregisterchallenge",
                                    RouteTemplate = "v1/user/register/challenge",
                                    Defaults = new {
                                            action = "RegisterChallenge",
                                            area = "CloudBust.Application",
                                            controller = "login"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "rolesbyuser",
                                    RouteTemplate = "v1/user/profile/roles",
                                    Defaults = new { 
                                            action = "Roles",
                                            area = "CloudBust.Application",
                                            controller = "user"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "profilebyusername",
                                    RouteTemplate = "v1/user/profile('{username}')",
                                    Defaults = new { 
                                            action = "Profile",
                                            area = "CloudBust.Application",
                                            controller = "user"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "rolesbyprofile",
                                    RouteTemplate = "v1/user/profile('{username}')/roles",
                                    Defaults = new { 
                                            action = "Roles",
                                            area = "CloudBust.Application",
                                            controller = "user"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "rolebyprofile",
                                    RouteTemplate = "v1/user/profile('{username}')/roles/('{rolename}')",
                                    Defaults = new { 
                                            action = "Roles",
                                            area = "CloudBust.Application",
                                            controller = "user"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "profilebyusernamebyfield",
                                    RouteTemplate = "v1/user/profile('{username}')/{field}",
                                    Defaults = new { 
                                            action = "ProfileField",
                                            area = "CloudBust.Application",
                                            controller = "user"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "profileinviteuserbyemail",
                                    RouteTemplate = "v1/user/invite/{email}",
                                    Defaults = new {
                                            action = "Invite",
                                            area = "CloudBust.Application",
                                            controller = "user"
                                        },
                                    },
                            new HttpRouteDescriptor {
                                    Name = "profileinviteuserbyemailwithmessage",
                                    RouteTemplate = "v1/user/invite/{email}/{message}",
                                    Defaults = new {
                                            action = "Invite",
                                            area = "CloudBust.Application",
                                            controller = "user"
                                        },
                                    },
                            // default action
                            new HttpRouteDescriptor {
                                    Name = "profiledefault",
                                    RouteTemplate = "v1/user/{action}",
                                    Defaults = new { 
                                            area = "CloudBust.Application",
                                            controller = "user"
                                        },
                                    }
            };
        }
    }
}