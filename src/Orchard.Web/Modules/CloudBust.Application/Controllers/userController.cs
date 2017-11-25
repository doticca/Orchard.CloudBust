using CloudBust.Application.Models;
using CloudBust.Application.Services;
using CloudBust.Common.Extensions;
using CloudBust.Common.OData;
using CloudBust.Application.OData;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Users.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloudBust.Application.OData.Profile;
using System.Text.RegularExpressions;

namespace CloudBust.Application.Controllers
{
    public class userController: ApiController
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IMembershipService _membershipService;
        private readonly IApplicationsService _applicationsService;
        private readonly IProfileService _profileService;
        private readonly ILoginsService _loginsService;
        public Localizer T { get; set; }
        public userController(
            IOrchardServices orchardServices,
            IMembershipService membershipService,
            IApplicationsService applicationsService,
            IProfileService profileService,
            ILoginsService loginsService
            )
        {
            _membershipService = membershipService;
            _orchardServices = orchardServices;
            _applicationsService = applicationsService;
            _profileService = profileService;
            _loginsService = loginsService;

            T = NullLocalizer.Instance;
        }

        private HttpResponseMessage GetSecuredProfile(string username, out IUser user, out ApplicationRecord applicationRecord, out Profile profile)
        {
            user = null;
            profile = null;
            IUser cUser = _orchardServices.WorkContext.CurrentUser;
            applicationRecord = null;
            if (string.IsNullOrWhiteSpace(username))
            {
                user = cUser;
            }
            else
            {
                user = _membershipService.GetUser(username);
            }
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            }
            int aid = _loginsService.GetSessionAppId(user);
            if (aid < 1) return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            applicationRecord = _applicationsService.GetApplication(aid);
            if (applicationRecord == null) return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            string h = _loginsService.GetHash(user.As<UserProfilePart>(), applicationRecord);

            bool publicu = !(cUser != null && (user.Id == cUser.Id || cUser.UserName.ToLowerInvariant() == "admin"));
            profile = new Profile(user, Request, h, false, publicu, true);

            return Request.CreateResponse(HttpStatusCode.OK, profile);
        }

        [ActionName("Profile")]
        public HttpResponseMessage GetProfile(string username = null)
        {
            IUser user = null;
            ApplicationRecord app = null;
            Profile profile = null;
            return GetSecuredProfile(username, out user, out app, out profile);                        
        }

        [ActionName("Profile")]
        public HttpResponseMessage PutProfile(uProfile profile)
        {
            if (profile == null || string.IsNullOrWhiteSpace(profile.Username) || profile.Id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            }

            IUser user = _orchardServices.WorkContext.CurrentUser;
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            }
            if (user.UserName != profile.Username || user.Id != profile.Id)
            {
                if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users")))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
                }
            }

            try
            {
                var userp = _orchardServices.ContentManager.Get<UserPart>(profile.Id);
                if(userp.UserName == profile.Username)
                {
                    profile.updateProfile(userp.As<UserProfilePart>());
                }
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            }           
        }

        [ActionName("Profile")]
        public HttpResponseMessage PatchProfile(uProfile profile)
        {
            if (profile == null || string.IsNullOrWhiteSpace(profile.Username) || profile.Id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            }

            IUser user = _orchardServices.WorkContext.CurrentUser;
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            }
            if (user.UserName != profile.Username || user.Id != profile.Id)
            {
                if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users")))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
                }
            }
            try
            {
                var userp = _orchardServices.ContentManager.Get<UserPart>(profile.Id);
                if (userp.UserName == profile.Username)
                {
                    profile.patchProfile(userp.As<UserProfilePart>());
                }
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            }
        }

        [ActionName("ProfileField")]
        [HttpGet]
        public HttpResponseMessage GetProfileField(string username, string field)
        {
            IUser user = null;
            ApplicationRecord app = null;
            Profile profile = null;
            HttpResponseMessage msg = GetSecuredProfile(username, out user, out app, out profile);
            if (msg.StatusCode == HttpStatusCode.OK)
            {
                switch (field)
                {
                    case "FirstName":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.FirstName);
                    case "Email":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.Email);
                    case "Username":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.Username);
                    case "LastName":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.LastName);
                    case "WebSite":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.WebSite);
                    case "Bio":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.Bio);
                    case "Location":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.Location);
                    case "link":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.link);
                    case "Type":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.Type);
                    case "Id":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.Id);
                    case "Hash":
                        return Request.CreateResponse(HttpStatusCode.OK, profile.Hash);
                    default:
                        return Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            else
                return msg;            
        }

        [ExtendedQueryable]
        [Orchard.Core.XmlRpc.Controllers.LiveWriterController.NoCache]
        [ActionName("Roles")]
        public IQueryable<UserRole> GetRoles(string username = null, string Hash = null)
        {
            IUser user = null;
            ApplicationRecord app = null;

            if (string.IsNullOrWhiteSpace(username))
            {
                if (_orchardServices.WorkContext.CurrentUser == null)
                    return null;
                else
                    username = _orchardServices.WorkContext.CurrentUser.UserName;
                Hash = null;
            }

            // if hash is null then we can only return data for current user
            if (Hash == null)
            {
                try
                {
                    user = _orchardServices.WorkContext.CurrentUser;
                    if (user.UserName.ToLower() != username.ToLower()) return null;

                    int aid = _loginsService.GetSessionAppId(user);
                    if (aid <1) return null;
                    
                    app = _applicationsService.GetApplication(aid);
                    if (app == null) return null;
                }
                catch
                {
                    return null;
                }
            }

            // get roles from service
            IEnumerable<UserRoleRecord> roles = _profileService.GetUserRoles(user.As<UserProfilePart>(), app);
            // create a new list
            List<UserRole> Roles = new List<UserRole>();
            foreach (UserRoleRecord role in roles)
            {
                Roles.Add(new UserRole(user, role, Request));
            }
            return Roles.AsQueryable();
        }

        [ActionName("Invite")]
        [HttpGet]
        public HttpResponseMessage Invite(string email, string message=null)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new uError("Bad Request", 400));
            }
            if (!Regex.IsMatch(email, UserPart.EmailPattern, RegexOptions.IgnoreCase))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new uError("Bad Request", 400));
            }
            _profileService.InviteWithEmail(null,user.As<UserProfilePart>(), email, message);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //[ExtendedQueryable]
        //[Orchard.Core.XmlRpc.Controllers.LiveWriterController.NoCache]
        //[ActionName("Users")]
        //public IQueryable<UserRole> GetUsers(string username = null, string Hash = null)
        //{
        //    IUser user = null;
        //    ApplicationRecord app = null;

        //    if (string.IsNullOrWhiteSpace(username))
        //    {
        //        if (_orchardServices.WorkContext.CurrentUser == null)
        //            return null;
        //        else
        //            username = _orchardServices.WorkContext.CurrentUser.UserName;
        //        Hash = null;
        //    }

        //    // if hash is null then we can only return data for current user
        //    if (Hash == null)
        //    {
        //        try
        //        {
        //            string appid = _orchardServices.WorkContext.HttpContext.Session["doticca_aid"].ToString();

        //            if (string.IsNullOrWhiteSpace(appid)) return null;
        //            int aid;
        //            if (!Int32.TryParse(appid, out aid)) return null;
        //            user = _orchardServices.WorkContext.CurrentUser;
        //            if (user.UserName.ToLower() != username.ToLower()) return null;
        //            app = _applicationsService.GetApplication(aid);
        //            if (app == null) return null;
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }


        //    //else
        //    //{
        //    //    user = _membershipService.GetUser(username);
        //    //    if(user == null) return null;
        //    //    app = _applicationsService.GetApplicationByKey(appID);
        //    //    if(app == null) return null;
        //    //}

        //    // get roles from service
        //    IEnumerable<UserRoleRecord> roles = _profileService.GetUserRoles(user.As<UserProfilePart>(), app);
        //    // create a new list
        //    List<UserRole> Roles = new List<UserRole>();
        //    foreach (UserRoleRecord role in roles)
        //    {
        //        Roles.Add(new UserRole(user, role, Request));
        //    }
        //    return Roles.AsQueryable();
        //}
    }
}