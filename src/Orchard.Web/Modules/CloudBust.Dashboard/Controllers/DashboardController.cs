using System;
using System.Linq;
using System.Web.Mvc;
using Orchard;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.DisplayManagement;
using System.Collections.Generic;
using Orchard.UI.Navigation;
using Orchard.Settings;
using Orchard.ContentManagement;
using Orchard.Security;
using CloudBust.Dashboard.ViewModels;
using CloudBust.Application.Services;
using Orchard.UI.Notify;
using Orchard.Messaging.Services;
using CloudBust.Application.Models;
using Orchard.Users.Models;
using Orchard.UI.Admin;

namespace CloudBust.Dashboard.Controllers
{

    [ValidateInput(false), Admin]
    public class DashboardController : Controller
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationsService _applicationsService;
        private readonly IGamesService _gamesService;
        private readonly IDataTablesService _datatablesService;
        private readonly IParametersService _parametersService;
        private readonly ISessionsService _sessionsService;
        private readonly ISiteService _siteService;
        private readonly IMessageService _messageService;
        private readonly IProfileService _profileService;
        private readonly ISettingsService _settingsService;

        public DashboardController(
            IOrchardServices services,
            IApplicationsService applicationsService,
            IGamesService gamesService,
            IDataTablesService datatablesService,
            IParametersService parametersService,
            ISessionsService sessionsService,
            IMessageService messageService,
            IShapeFactory shapeFactory,
            IProfileService profileService,
            ISettingsService settingsService,
            ISiteService siteService)
        {
            _orchardServices = services;
            _applicationsService = applicationsService;
            _gamesService = gamesService;
            _datatablesService = datatablesService;
            _parametersService = parametersService;
            _sessionsService = sessionsService;
            _siteService = siteService;
            _messageService = messageService;
            _profileService = profileService;
            _settingsService = settingsService;
            T = NullLocalizer.Instance;
            Shape = shapeFactory;
        }

        public Localizer T { get; set; }
        dynamic Shape { get; set; }

        // dashboard main screen
        public ActionResult Index()
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }
           
            var model = new DashboardViewModel();
            model.User = user;

            return View(model);
        }

        #region Games

        // App Information
        public ActionResult Game(string gameID)
        {
            return GamePage(gameID, 0);
        }

        [HttpPost, ActionName("Game")]
        [FormValueRequired("submit.Save")]
        public ActionResult GamePOST(string gameID)
        {
            var game = _gamesService.GetGameByKey(gameID);

            if (game == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            var viewModel = new GameViewModel();
            TryUpdateModel(viewModel);

            _gamesService.UpdateGame(game.Id, viewModel.Game.Name, viewModel.Game.Description);
            _gamesService.UpdateGameLocation(game.Id, viewModel.Game.Latitude, viewModel.Game.Longitude);

            return RedirectToAction("Game", "Dashboard", new { area = "CloudBust.Dashboard" });
        }

        public ActionResult Games()
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new GamesViewModel();
            model.User = user;


            var games = _gamesService.GetUserGames(user).OrderBy(r => r.Name).ToList();
            model.Games = games;

            return View(model);
        }        

        public ActionResult GameCreate(string appID)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }



            var model = new GameCreateViewModel();
            if (!string.IsNullOrWhiteSpace(appID))
            {
                var app = _applicationsService.GetApplicationByKey(appID);
                if (app != null)
                {
                    model.ApplicationName = app.Name;
                    model.ApplicationKey = app.AppKey;
                }
            }
            model.User = user;
            model.owner = user.UserName;

            return View(model);
        }

        [HttpPost, ActionName("GameCreate")]
        [FormValueRequired("submit.Save")]
        public ActionResult GameCreatePOST()
        {
            var viewModel = new GameCreateViewModel();
            TryUpdateModel(viewModel);

            if (string.IsNullOrWhiteSpace(viewModel.owner))
            {
                viewModel.owner = "admin";
            }
            if (String.IsNullOrEmpty(viewModel.Name))
            {
                ModelState.AddModelError("Name", T("Game name can't be empty"));
            }

            var group = _gamesService.GetGameByName(viewModel.Name);
            if (group != null)
            {
                ModelState.AddModelError("Name", T("Game with same name already exists"));
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }


            var newGame = _gamesService.CreateGame(viewModel.Name, viewModel.Description, viewModel.owner);
            if (newGame == null)
            {
                _orchardServices.Notifier.Information(T("An error occured while trying to create the Game."));
            }
            else
            {
                //_applicationsService.CreateKeysForApplication(newModule.Id);
                newGame = _gamesService.GetGame(newGame.Id);

                if (!string.IsNullOrWhiteSpace(viewModel.ApplicationName))
                {
                    var module = _applicationsService.GetApplicationByName(viewModel.ApplicationName);
                    if (module != null)
                    {
                        _gamesService.CreateGameForApplication(viewModel.ApplicationName, newGame.Id);

                        return RedirectToAction("SenseapiGames", "Dashboard", new { area = "CloudBust.Dashboard", appID = module.AppKey });
                    }
                }
            }

            return RedirectToAction("Games", "Dashboard", new { area = "CloudBust.Dashboard" });
        }

        // App Tokens
        public ActionResult GameKeys(string gameID)
        {

            return GamePage(gameID, 1);
        }

        // Events
        public ActionResult GameEvents(string gameID)
        {
            return GamePage(gameID, 2);
        }

        public ActionResult GameEventCreate(string gameID)
        {
            var game = _gamesService.GetGameByKey(gameID);

            if (game == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (game.owner != user.UserName)
            {
                return GameEvents(gameID);
            }

            var model = new GameEventCreateViewModel();
            model.GameName = game.Name;
            model.GameKey = game.AppKey;
            model.User = user;
            model.EventName = string.Empty;
            model.EventDescription = string.Empty;
            model.EventBusinessProcess = 0;
            model.GameEventType = GameEventType.instruction;

            return View(model);
        }

        [HttpPost, ActionName("GameEventCreate")]
        [FormValueRequired("submit.Save")]
        public ActionResult GameEventCreatePOST(string gameID)
        {

            var viewModel = new GameEventCreateViewModel();
            TryUpdateModel(viewModel);

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                viewModel.User = user;
            }

            var game = _gamesService.GetGameByKey(gameID);

            if (game == null)
            {
                ModelState.AddModelError("Game", T("Game can't be empty"));
            }
            else
            {
                // in case we have to return to this view
                viewModel.GameName = game.Name;
                viewModel.GameKey = game.AppKey;
            }

            if (String.IsNullOrEmpty(viewModel.EventName))
            {
                ModelState.AddModelError("Name", T("Event name can't be empty"));
            }

            var gameevent = _gamesService.GetGameEventByName(game, viewModel.EventName);
            if (gameevent != null)
            {
                ModelState.AddModelError("Name", T("Event with same name already exists"));
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var newGameEvent = _gamesService.CreateGameEvent(game, viewModel.EventName, viewModel.EventDescription, viewModel.GameEventType);
            if (newGameEvent == null)
            {
                _orchardServices.Notifier.Information(T("An error occured while trying to create the game event."));
            }


            return RedirectToAction("GameEvents", "Dashboard", new { area = "CloudBust.Dashboard", gameID = gameID });
        }

        public ActionResult GameEventEdit(string gameID, string eventID)
        {
            var game = _gamesService.GetGameByKey(gameID);

            if (game == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (game.owner != user.UserName)
            {
                return GameEvents(gameID);
            }

            int eventid;
            if (!Int32.TryParse(eventID, out eventid))
            {
                return HttpNotFound();
            }
            var ev = _gamesService.GetGameEvent(eventid);

            if (ev == null)
                return HttpNotFound();
            // double check to see if this event id belongs to this game
            ev = _gamesService.GetGameEventByName(game, ev.Name);
            if (ev == null)
                return HttpNotFound();

            var model = new GameEventEditViewModel();
            model.GameName = game.Name;
            model.GameKey = game.AppKey;
            model.User = user;
            model.EventName = ev.Name;
            model.EventDescription = ev.Description;
            model.GameEventType = (GameEventType)ev.GameEventType;

            return View(model);
        }

        [HttpPost, ActionName("GameEventEdit")]
        [FormValueRequired("submit.Save")]
        public ActionResult GameEventEditPOST(string gameID, string eventID)
        {
            var viewModel = new GameEventEditViewModel();
            TryUpdateModel(viewModel);

            var game = _gamesService.GetGameByKey(gameID);

            if (game == null)
            {
                ModelState.AddModelError("Game", T("Game can't be empty"));
            }
            else
            {
                // in case we have to reutnr to this view
                viewModel.GameName = game.Name;
                viewModel.GameKey = game.AppKey;
            }
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                viewModel.User = user;
            }

            int eventid;
            if (!Int32.TryParse(eventID, out eventid))
            {
                ModelState.AddModelError("Event", T("Event Id can't be empty"));
            }
            var ev = _gamesService.GetGameEvent(eventid);

            if (ev == null)
                ModelState.AddModelError("Event", T("Event can't be empty"));
            // double check to see if this role id belongs to this application
            ev = _gamesService.GetGameEventByName(game, ev.Name);
            if (ev == null)
                return HttpNotFound();

            if (String.IsNullOrEmpty(viewModel.EventName))
            {
                ModelState.AddModelError("Name", T("Event name can't be empty"));
            }

            if (viewModel.EventName != ev.Name)
            {
                var group = _gamesService.GetGameEventByName(game, viewModel.EventName);
                if (group != null)
                {
                    ModelState.AddModelError("Name", T("Event with same name already exists"));
                }
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            bool updated = _gamesService.UpdateGameEvent(ev.Id, viewModel.EventName, viewModel.EventDescription, viewModel.GameEventType);
            if (!updated)
            {
                _orchardServices.Notifier.Information(T("An error occured while trying to update the event."));
            }

            return RedirectToAction("GameEvents", "Dashboard", new { area = "CloudBust.Dashboard", gameID = gameID });
        }
        public ActionResult GameEventDelete(string gameID, string eventID)
        {
            var game = _gamesService.GetGameByKey(gameID);

            if (game == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (game.owner != user.UserName)
            {
                return GameEvents(gameID);
            }

            int eventid;
            if (!Int32.TryParse(eventID, out eventid))
            {
                return HttpNotFound();
            }
            var ev = _gamesService.GetGameEvent(eventid);

            if (ev == null)
                return HttpNotFound();
            // double check to see if this event id belongs to this game
            ev = _gamesService.GetGameEventByName(game, ev.Name);
            if (ev == null)
                return HttpNotFound();

            var model = new GameEventDeleteViewModel();
            model.User = user;
            model.GameName = game.Name;
            model.GameKey = game.AppKey;
            model.EventName = ev.Name;
            model.EventDescription = ev.Description;

            return View(model);
        }

        [HttpPost, ActionName("GameEventDelete")]
        [FormValueRequired("submit.Delete")]
        public ActionResult GameEventDeletePOST(string gameID, string eventID, string returnUrl)
        {
            var game = _gamesService.GetGameByKey(gameID);

            if (game == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (game.owner != user.UserName)
            {
                return GameEvents(gameID);
            }

            int eventid;
            if (!Int32.TryParse(eventID, out eventid))
            {
                return HttpNotFound();
            }
            var ev = _gamesService.GetGameEvent(eventid);

            if (ev == null)
                return HttpNotFound();
            // double check to see if this event id belongs to this game
            ev = _gamesService.GetGameEventByName(game, ev.Name);
            if (ev == null)
                return HttpNotFound();

            _gamesService.DeleteGameEvent(ev.Id);

            return this.RedirectLocal(returnUrl, () =>
                RedirectToAction("GameEvents", "Dashboard", new { area = "CloudBust.Dashboard", gameID = gameID }));
        }

        public ActionResult GameEventsUp(string gameID, string eventID)
        {
            var game = _gamesService.GetGameByKey(gameID);

            if (game == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (game.owner != user.UserName)
            {
                return GameEvents(gameID);
            }
            int eventid;
            if (!Int32.TryParse(eventID, out eventid))
            {
                return HttpNotFound();
            }
            var ev = _gamesService.GetGameEvent(eventid);

            if (ev == null)
                return HttpNotFound();
            // double check to see if this event id belongs to this game
            ev = _gamesService.GetGameEventByName(game, ev.Name);
            if (ev == null)
                return HttpNotFound();

            _gamesService.GameEventBusinessProcessUp(ev.Id);

            return GameEvents(gameID);
        }

        public ActionResult GameEventsDown(string gameID, string eventID)
        {
            var game = _gamesService.GetGameByKey(gameID);

            if (game == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (game.owner != user.UserName)
            {
                return GameEvents(gameID);
            }
            int eventid;
            if (!Int32.TryParse(eventID, out eventid))
            {
                return HttpNotFound();
            }
            var ev = _gamesService.GetGameEvent(eventid);

            if (ev == null)
                return HttpNotFound();
            // double check to see if this event id belongs to this game
            ev = _gamesService.GetGameEventByName(game, ev.Name);
            if (ev == null)
                return HttpNotFound();

            _gamesService.GameEventBusinessProcessDown(ev.Id);

            return GameEvents(gameID);
        }

        #endregion

        #region Applications
        // list of applications
        public ActionResult Applications()
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new ApplicationsViewModel();
            model.User = user;


            var apps = _applicationsService.GetUserApplications(user).OrderBy(r => r.Name).ToList();
            model.Applications = apps;

            return View(model);
        }

        public ActionResult ApplicationCreate()
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new ApplicationCreateViewModel();
            model.User = user;
            model.owner = user.UserName;

            return View(model);
        }

        [HttpPost, ActionName("ApplicationCreate")]
        [FormValueRequired("submit.Save")]
        public ActionResult ApplicationCreatePOST()
        {
            var viewModel = new ApplicationCreateViewModel();
            TryUpdateModel(viewModel);

            if (string.IsNullOrWhiteSpace(viewModel.owner))
            {
                viewModel.owner = "admin";
            }
            if (String.IsNullOrEmpty(viewModel.Name))
            {
                ModelState.AddModelError("Name", T("Application name can't be empty"));
            }

            var group = _applicationsService.GetApplicationByName(viewModel.Name);
            if (group != null)
            {
                ModelState.AddModelError("Name", T("Application with same name already exists"));
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }


            var newModule = _applicationsService.CreateApplication(viewModel.Name, viewModel.Description, viewModel.owner);
            if (newModule == null)
            {
                _orchardServices.Notifier.Information(T("An error occured while trying to create the Application."));
            }
            else
            {
                _applicationsService.CreateKeysForApplication(newModule.Id);
                newModule = _applicationsService.GetApplication(newModule.Id);
            }

            return RedirectToAction("ApplicationKeys", "Dashboard", new { area = "CloudBust.Dashboard", appID = newModule.AppKey });
        }

        public ActionResult ApplicationDelete(string appID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var model = new ApplicationDeleteViewModel();
            model.User = user;
            model.Application = app;

            return View(model);
        }

        // this method is ommited for security reasons
        [HttpPost, ActionName("ApplicationDelete")]
        [FormValueRequired("submit.Delete")]
        public ActionResult ApplicationDeletePOST(string appID, string returnUrl)
        {
            var app = _applicationsService.GetApplicationByKey(appID);
            if (app == null)
                return HttpNotFound();

            //_applicationsService.DeleteApplication(app.Id);

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        // App Information
        public ActionResult Application(string appID)
        {
            return ApplicationPage(appID, 0);
        }

        [HttpPost, ActionName("Application")]
        [FormValueRequired("submit.Save")]
        public ActionResult ApplicationPOST(string appID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new ApplicationEditInfoViewModel();
            TryUpdateModel(viewModel);

            _applicationsService.UpdateApplication(app.Id, viewModel.Name, viewModel.Description);

            return RedirectToAction("Application", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }

        // App Tokens
        public ActionResult ApplicationKeys(string appID)
        {

            return ApplicationPage(appID, 1);
        }

        // Facebook Token
        public ActionResult ApplicationFB(string appID)
        {

            return ApplicationPage(appID, 2);
        }

        [HttpPost, ActionName("ApplicationFB")]
        [FormValueRequired("submit.Save")]
        public ActionResult ApplicationFBPOST(string appID)
        {

            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new ApplicationEditFBViewModel();
            TryUpdateModel(viewModel);

            _applicationsService.UpdateApplicationFacebook(app.Id, viewModel.fbAppKey, viewModel.fbAppSecret);
            return RedirectToAction("ApplicationFB", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }

        // Game Center Bundle Identifier
        public ActionResult ApplicationGC(string appID)
        {

            return ApplicationPage(appID, 3);
        }
        [HttpPost, ActionName("ApplicationGC")]
        [FormValueRequired("submit.Save")]
        public ActionResult ApplicationGCPOST(string appID)
        {

            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new ApplicationEditGCViewModel();
            TryUpdateModel(viewModel);

            _applicationsService.UpdateApplicationGameCenter(app.Id, viewModel.BundleIdentifier, viewModel.BundleIdentifierOSX, viewModel.BundleIdentifierTvOS, viewModel.BundleIdentifierWatch);
            return RedirectToAction("ApplicationGC", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }
        // App Store
        public ActionResult ApplicationAppStore(string appID)
        {

            return ApplicationPage(appID, 4);
        }
        [HttpPost, ActionName("ApplicationAppStore")]
        [FormValueRequired("submit.Save")]
        public ActionResult ApplicationAppStorePOST(string appID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new ApplicationEditAppStoreViewModel();
            TryUpdateModel(viewModel);

            _applicationsService.UpdateApplicationAppStore(app.Id, viewModel.ServerBuild, viewModel.MinimumClientBuild, viewModel.UpdateUrl, viewModel.UpdateUrlOSX, viewModel.UpdateUrlTvOS, viewModel.UpdateUrlWatch, viewModel.UpdateUrlDeveloper);
            return RedirectToAction("ApplicationAppStore", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }
        #endregion

        #region SMTP
        // SMTP Settings
        public ActionResult ApplicationSmtp(string appID, bool afterPost=false)
        {
            return ApplicationPage(appID, 5, afterPost);
        }
        [HttpPost, ActionName("ApplicationSmtp")]
        [FormValueRequired("submit.Save")]
        public ActionResult ApplicationSmtpPOST(string appID)
        {

            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new ApplicationEditSmtpViewModel();
            TryUpdateModel(viewModel);

            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("internalEmail"))// && Request.Form[key] == "true")
                {
                    // the user checked the internal server
                    viewModel.internalEmail = true;
                }
            }

            _applicationsService.UpdateApplicationSmtp(app.Id, viewModel.internalEmail, viewModel.senderEmail, viewModel.mailServer,viewModel.mailPort,viewModel.mailUsername, viewModel.mailPassword);
            return RedirectToAction("ApplicationSmtp", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID, afterPost = true });
            //return ApplicationSmtp(appID, true);
        }

        public ActionResult ApplicationTestMail(string appID)
        {
            return ApplicationPage(appID, 6);
        }

        public ActionResult ApplicationSendMail(string appID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var model = new ApplicationViewModel();
            model.User = user;
            model.Application = app;
            SendTestMail(app);
            return View(model);
        }

        private bool SendTestMail(ApplicationRecord app)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;
            var parameters = new Dictionary<string, object> {
                        {"Application", app.AppKey},
                        {"Subject", T("CloudBust Test mail").Text},
                        {"Body", "You received this email because you requested a Test from your CloudBust application " + app.Name},
                        {"Recipients", user.Email }
                    };

            _messageService.Send("Email", parameters);
            return true;
        }

        #endregion

        #region Blogs

        public ActionResult ApplicationBlogs(string appID, bool afterPost = false)
        {
            return ApplicationPage(appID, 15, afterPost);
        }
        [HttpPost, ActionName("ApplicationBlogs")]
        [FormValueRequired("submit.Save")]
        public ActionResult ApplicationBlogsPOST(string appID)
        {

            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new ApplicationEditBlogsViewModel();
            TryUpdateModel(viewModel);

            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("blogPerUser"))
                {
                    viewModel.blogPerUser = true;
                }
                if (key.StartsWith("blogSecurity"))
                {
                    viewModel.blogSecurity = true;
                }
            }

            _applicationsService.UpdateApplicationBlogs(app.Id, viewModel.blogPerUser, viewModel.blogSecurity);
            return RedirectToAction("ApplicationBlogs", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID, afterPost = true });
        }
        #endregion

        #region Users
        // Users
        public ActionResult ApplicationUserRoles(string appID, bool afterPost = false)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var model = new ApplicationViewModel();
            model.User = user;

            model.Application = app;
            model.DefaultRole = _applicationsService.GetDefaultRole(app);
            model.Roles = _applicationsService.GetUserRoles(app);
            model.Page = 7;
            model.afterPost = afterPost;
            model.Uri = Request.Url;
            return View("Application", model);

        }

        public ActionResult ApplicationUsers(string appID, PagerParameters pagerParameters, bool afterPost = false)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);


            IEnumerable<int> profilesIDs = _profileService.GetUserIDsForApplication(app);

            var users = _orchardServices.ContentManager
                .Query<UserProfilePart, UserProfilePartRecord>().Where(x => profilesIDs.Contains(x.Id));

            var pagerShape = Shape.Pager(pager).TotalItemCount(users.Count());

            var results = users
                .Slice(pager.GetStartIndex(), pager.PageSize)
                .ToList();


            var model = new ApplicationViewModel
            {
                Users = results
                    .Select(x => new UserProfileEntry { User = x.As<UserPart>().Record, Profile = x.As<UserProfilePart>().Record })
                    .ToList(),
                Pager = pagerShape
            };

            model.Application = app;
            model.DefaultRole = _applicationsService.GetDefaultRole(app);
            model.Roles = _applicationsService.GetUserRoles(app);
            model.Page = 8;
            model.afterPost = afterPost;
            model.Uri = Request.Url;
            return View("Application", model);


            //return ApplicationPage(appID, 6, afterPost);
        }
        public ActionResult ApplicationUserEdit(string appID, int profileID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;


            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return ApplicationUserRoles(appID);
            }

            UserProfilePart profilePart = _profileService.Get(profileID);
            if (profilePart == null)
            {
                return HttpNotFound();
            }


            

            var model = new UserEditViewModel(app,profilePart);

            return View(model);
        }
        public ActionResult ApplicationUserInvites(string appID, int profileID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;


            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return ApplicationUsers(appID, null);
            }

            UserProfilePart profilePart = _profileService.Get(profileID);
            if (profilePart == null)
            {
                return HttpNotFound();
            }

            var model = new UserInvitesViewModel(app, profilePart);
            model.Invitations = _profileService.GetPendingInvitations(profilePart, null);
            return View(model);
        }

        public ActionResult ApplicationUserFriends(string appID, int profileID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;


            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return ApplicationUsers(appID, null);
            }

            UserProfilePart profilePart = _profileService.Get(profileID);
            if (profilePart == null)
            {
                return HttpNotFound();
            }

            var model = new UserFriendsViewModel(app, profilePart);
            model.Friends = profilePart.Friends; //_profileService.GetFriendsOfUser(profilePart,app);
            return View(model);
        }

        public ActionResult UserRoleCreate(string appID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return ApplicationUserRoles(appID);
            }

            var model = new UserRoleCreateViewModel();
            model.ApplicationName = app.Name;
            model.AppKey = app.AppKey;
            model.User = user;
            model.Name = string.Empty;
            model.Description = string.Empty;
            model.isDefault = false;

            return View(model);
        }

        [HttpPost, ActionName("UserRoleCreate")]
        [FormValueRequired("submit.Save")]
        public ActionResult UserRoleCreatePOST(string appID)
        {

            var viewModel = new UserRoleCreateViewModel();
            TryUpdateModel(viewModel);

            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("isDefault"))// && Request.Form[key] == "true")
                {
                    // the user checked the internal server
                    viewModel.isDefault = true;
                }
            }

            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
            {
                ModelState.AddModelError("Application", T("Application can't be empty"));
            }
            else
            {
                // in case we have to reutnr to this view
                viewModel.ApplicationName = app.Name;
                viewModel.AppKey = app.AppKey;
            }

            if (String.IsNullOrEmpty(viewModel.Name))
            {
                ModelState.AddModelError("Name", T("User role name can't be empty"));
            }

            var group = _applicationsService.GetUserRoleByName(app,viewModel.Name);
            if (group != null)
            {
                ModelState.AddModelError("Name", T("User role with same name already exists"));
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }


            var newUserRole = _applicationsService.CreateUserRole(app, viewModel.Name, viewModel.Description);
            if (newUserRole == null)
            {
                _orchardServices.Notifier.Information(T("An error occured while trying to create the user role."));
            }
            else
            {
                if (viewModel.isDefault)
                {
                    _applicationsService.SetDefaultRole(app, newUserRole.Id);
                }
                newUserRole = _applicationsService.GetUserRole(newUserRole.Id);
            }

            return RedirectToAction("ApplicationUserRoles", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }

        public ActionResult UserRoleEdit(string appID, string roleID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;


            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return ApplicationUserRoles(appID);
            }
            int roleid;
            if(!Int32.TryParse(roleID, out roleid))
            {
                return HttpNotFound();
            }
            var role = _applicationsService.GetUserRole(roleid);

            if (role == null)
                return HttpNotFound();
            // double check to see if this role id belongs to this application
            role = _applicationsService.GetUserRoleByName(app, role.Name);
            if (role == null)
                return HttpNotFound();

            var model = new UserRoleEditViewModel();
            model.ApplicationName = app.Name;
            model.AppKey = app.AppKey;
            model.User = user;
            model.Name = role.Name;
            model.Description = role.Description;
            model.isDefault = role.IsDefaultRole;
            model.isDashboard = role.IsDashboardRole;
            model.isData = role.IsData;
            model.isSecurity = role.IsSecurity;
            model.isSettings = role.IsSettings;

            return View(model);
        }

        [HttpPost, ActionName("UserRoleEdit")]
        [FormValueRequired("submit.Save")]
        public ActionResult UserRoleEditPOST(string appID, string roleID)
        {
            var viewModel = new UserRoleEditViewModel();
            TryUpdateModel(viewModel);

            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("isDefault"))// && Request.Form[key] == "true")
                {
                    // the user checked the internal server
                    viewModel.isDefault = true;
                }
            }

            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
            {
                ModelState.AddModelError("Application", T("Application can't be empty"));
            }
            else
            {
                // in case we have to reutnr to this view
                viewModel.ApplicationName = app.Name;
                viewModel.AppKey = app.AppKey;
            }
            int roleid;
            if (!Int32.TryParse(roleID, out roleid))
            {
                ModelState.AddModelError("Role", T("Role Id can't be empty"));
            }
            var role = _applicationsService.GetUserRole(roleid);

            if (role == null)
                ModelState.AddModelError("Role", T("Role can't be empty"));
            // double check to see if this role id belongs to this application
            role = _applicationsService.GetUserRoleByName(app, role.Name);
            if (role == null)
                return HttpNotFound();

            if (String.IsNullOrEmpty(viewModel.Name))
            {
                ModelState.AddModelError("Name", T("User role name can't be empty"));
            }

            if (viewModel.Name != role.Name)
            {
                var group = _applicationsService.GetUserRoleByName(app, viewModel.Name);
                if (group != null)
                {
                    ModelState.AddModelError("Name", T("User role with same name already exists"));
                }
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            bool updated = _applicationsService.UpdateUserRole(role.Id, viewModel.Name, viewModel.Description);
            if (!updated)
            {
                _orchardServices.Notifier.Information(T("An error occured while trying to update the user role."));
            }
            else
            {
                if (viewModel.isDefault)
                {
                    _applicationsService.SetDefaultRole(app, role.Id);
                }
            }

            return RedirectToAction("ApplicationUserRoles", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }

        public ActionResult UserRoleDelete(string appID, string roleID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;


            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return ApplicationUserRoles(appID);
            }
            int roleid;
            if (!Int32.TryParse(roleID, out roleid))
            {
                return HttpNotFound();
            }
            var role = _applicationsService.GetUserRole(roleid);

            if (role == null)
                return HttpNotFound();
            // double check to see if this role id belongs to this application
            role = _applicationsService.GetUserRoleByName(app, role.Name);
            if (role == null)
                return HttpNotFound();

            var model = new UserRoleDeleteViewModel();
            model.ApplicationName = app.Name;
            model.AppKey = app.AppKey;
            model.User = user;
            model.Name = role.Name;
            model.Description = role.Description;
            model.isDefault = role.IsDefaultRole;

            return View(model);
        }

        [HttpPost, ActionName("UserRoleDelete")]
        [FormValueRequired("submit.Delete")]
        public ActionResult UserRoleDeletePOST(string appID, string roleID)
        {

            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
            {
                ModelState.AddModelError("Application", T("Application can't be empty"));
            }

            int roleid;
            if (!Int32.TryParse(roleID, out roleid))
            {
                ModelState.AddModelError("Role", T("Role Id can't be empty"));
            }
            var role = _applicationsService.GetUserRole(roleid);

            if (role == null)
                ModelState.AddModelError("Role", T("Role can't be empty"));

            if (!ModelState.IsValid)
            {
                return HttpNotFound();
            }
            // double check to see if this role id belongs to this application
            role = _applicationsService.GetUserRoleByName(app, role.Name);
            if (role == null)
                return HttpNotFound();

            bool updated = _applicationsService.DeleteUserRole(role.Id);
            if (!updated)
            {
                _orchardServices.Notifier.Information(T("An error occured while trying to update the user role."));
            }

            return RedirectToAction("ApplicationUserRoles", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }

        #endregion

        #region Data
        // Data
        public ActionResult Table(string datatableID)
        {
            return TablePage(datatableID, 0);
        }

        public ActionResult TableFields(string datatableId)
        {
            return TablePage(datatableId, 1);
        }
        public ActionResult TableRows(string datatableId)
        {
            return TablePage(datatableId, 2);
        }

        public ActionResult Tables()
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new TablesViewModel();
            model.User = user;


            var datatables = _datatablesService.GetDataTables().OrderBy(r => r.Name).ToList();
            model.DataTables = datatables;

            return View(model);
        }

        public ActionResult TableCreate(string appID)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }



            var model = new TableCreateViewModel();
            if (!string.IsNullOrWhiteSpace(appID))
            {
                var app = _applicationsService.GetApplicationByKey(appID);
                if (app != null)
                {
                    model.ApplicationName = app.Name;
                    model.ApplicationKey = app.AppKey;
                }
            }
            model.User = user;

            return View(model);
        }

        [HttpPost, ActionName("TableCreate")]
        [FormValueRequired("submit.Save")]
        public ActionResult TableCreatePOST()
        {
            var viewModel = new TableCreateViewModel();
            TryUpdateModel(viewModel);

            if (String.IsNullOrEmpty(viewModel.Name))
            {
                ModelState.AddModelError("Name", T("Table name can't be empty"));
            }

            var group = _datatablesService.GetDataTableByName(viewModel.Name);
            if (group != null)
            {
                ModelState.AddModelError("Name", T("Table with same name already exists"));
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }


            var newTable = _datatablesService.CreateDataTable(viewModel.Name, viewModel.Description);
            if (newTable == null)
            {
                _orchardServices.Notifier.Information(T("An error occured while trying to create the Table."));
            }
            else
            {
                //_applicationsService.CreateKeysForApplication(newModule.Id);
                newTable = _datatablesService.GetDataTable(newTable.Id);

                if (!string.IsNullOrWhiteSpace(viewModel.ApplicationName))
                {
                    var module = _applicationsService.GetApplicationByName(viewModel.ApplicationName);
                    if (module != null)
                    {
                        _datatablesService.CreateDataTableForApplication(viewModel.ApplicationName, newTable.Id);

                        return RedirectToAction("Tables", "Dashboard", new { area = "CloudBust.Dashboard", appID = module.AppKey });
                    }
                }
            }
            return RedirectToAction("Tables", "Dashboard", new { area = "CloudBust.Dashboard" });

        }

        public ActionResult ApplicationTables(string appID, bool afterPost = false)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var model = new ApplicationViewModel();

            model.Application = app;
            model.DataTables = _datatablesService.GetApplicationDataTables(app);
            model.Page = 11;
            model.afterPost = afterPost;
            model.Uri = Request.Url;

            return View("Application", model);
        }

        public ActionResult ApplicationTablesAdd(string appID, int datatableID = 0)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return new HttpUnauthorizedResult();
            }

            if (datatableID == 0)
            {
                var model = new DataTablesViewModel();
                model.User = user;
                model.Application = app;
                var datatables = _datatablesService.GetNonApplicationDataTables(user, app).OrderBy(r => r.Name).ToList();
                model.DataTables = datatables;

                return View(model);
            }
            else
            {
                var datatable = _datatablesService.GetDataTable(datatableID);

                if (datatable != null)
                {
                    _datatablesService.CreateDataTableForApplication(app.Name, datatable.Id);
                }
            }
            return RedirectToAction("Tables", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }
        public ActionResult TableFieldCreate(string datatableId)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            //if (app.owner != user.UserName)
            //{
            //    return Application(appID);
            //}
            int datatableid;
            if (!Int32.TryParse(datatableId, out datatableid))
            {
                return HttpNotFound();
            }

            ApplicationDataTableRecord table = _datatablesService.GetDataTable(datatableid);
            if (table == null)
            {
                return HttpNotFound();
            }

            var model = new FieldCreateViewModel();
            model.ApplicationDataTableName = table.Name;
            model.ApplicationDataTableID = datatableId;
            model.User = user;
            model.Name = string.Empty;
            model.Description = string.Empty;
            model.FieldType = "string";
            return View(model);
        }
        [HttpPost, ActionName("TableFieldCreate")]
        [FormValueRequired("submit.Save")]
        public ActionResult TableFieldCreatePOST(string datatableId)
        {

            var viewModel = new FieldCreateViewModel();
            TryUpdateModel(viewModel);

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                viewModel.User = user;
            }
            int datatableid;
            if (!Int32.TryParse(datatableId, out datatableid))
            {
                return HttpNotFound();
            }

            ApplicationDataTableRecord table = _datatablesService.GetDataTable(datatableid);
            if (table == null)
            {
                return HttpNotFound();
            }
            viewModel.ApplicationDataTableName = table.Name;
            viewModel.ApplicationDataTableID = datatableId;
            viewModel.User = user;

            if (string.IsNullOrWhiteSpace(viewModel.Name))
            {
                ModelState.AddModelError("Name", "You have to enter a name for the new field");
            }
            if (string.IsNullOrWhiteSpace(viewModel.Description))
            {
                ModelState.AddModelError("Name", "You have to enter a description for the new field");
            }
            if (string.IsNullOrWhiteSpace(viewModel.FieldType))
            {
                ModelState.AddModelError("Name", "You have to set the field type");
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var field = _datatablesService.CreateField(viewModel.Name, viewModel.Description, viewModel.FieldType);
            if (field == null)
            {
                ModelState.AddModelError("_FORM","Error while creating field");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            _datatablesService.CreateFieldForApplicationDataTable(datatableid, field.Id);

            //return RedirectToAction("AppSettings", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
            return TablePage(datatableId, 1, true);
        }

        public ActionResult TableFieldEdit(string datatableId, int fieldId)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            //if (app.owner != user.UserName)
            //{
            //    return Application(appID);
            //}
            int datatableid;
            if (!Int32.TryParse(datatableId, out datatableid))
            {
                return HttpNotFound();
            }

            ApplicationDataTableRecord table = _datatablesService.GetDataTable(datatableid);
            if (table == null)
            {
                return HttpNotFound();
            }
            FieldRecord field = _datatablesService.GetField(fieldId);
            if (field == null)
            {
                return HttpNotFound();
            }

            var model = new FieldEditViewModel();
            model.ApplicationDataTableName = table.Name;
            model.ApplicationDataTableID = datatableId;

            model.Name = field.Name;
            model.Description = field.Description;
            model.FieldType = field.FieldType;
            return View(model);
        }

        [HttpPost, ActionName("TableFieldEdit")]
        [FormValueRequired("submit.Save")]
        public ActionResult TableFieldEditPOST(string datatableId, int fieldId)
        {

            var viewModel = new FieldEditViewModel();
            TryUpdateModel(viewModel);

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            int datatableid;
            if (!Int32.TryParse(datatableId, out datatableid))
            {
                return HttpNotFound();
            }

            ApplicationDataTableRecord table = _datatablesService.GetDataTable(datatableid);
            if (table == null)
            {
                return HttpNotFound();
            }
            FieldRecord field = _datatablesService.GetField(fieldId);
            if (field == null)
            {
                return HttpNotFound();
            }
            viewModel.ApplicationDataTableName = table.Name;
            viewModel.ApplicationDataTableID = datatableId;

            if (string.IsNullOrWhiteSpace(viewModel.Name))
            {
                ModelState.AddModelError("Name", "You have to enter a name for the new field");
            }
            if (string.IsNullOrWhiteSpace(viewModel.Description))
            {
                ModelState.AddModelError("Name", "You have to enter a description for the new field");
            }
            if (string.IsNullOrWhiteSpace(viewModel.FieldType))
            {
                ModelState.AddModelError("Name", "You have to set the field type");
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            _datatablesService.UpdateField(field.Id, viewModel.Name, viewModel.Description, viewModel.FieldType);

            return TablePage(datatableId, 1, true);
        }
        public ActionResult TableRowCreate(string datatableId)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            //if (app.owner != user.UserName)
            //{
            //    return Application(appID);
            //}
            int datatableid;
            if (!Int32.TryParse(datatableId, out datatableid))
            {
                return HttpNotFound();
            }

            ApplicationDataTableRecord table = _datatablesService.GetDataTable(datatableid);
            if (table == null)
            {
                return HttpNotFound();
            }

            var model = new FieldCreateViewModel();
            model.ApplicationDataTableName = table.Name;
            model.ApplicationDataTableID = datatableId;
            model.User = user;
            model.Name = string.Empty;
            model.Description = string.Empty;
            model.FieldType = "string";
            return View(model);
        }
        public ActionResult AppSettings(string appID, PagerParameters pagerParameters, bool afterPost = false)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }



            var settings = _parametersService.GetParametersForApplication(app.Id).OrderBy(r => r.Name).ToList();

            var model = new ApplicationViewModel
            {
                Settings = settings
            };

            model.Application = app;
            model.Page = 9;
            model.afterPost = afterPost;
            model.Uri = Request.Url;
            return View("Application", model);
        }

        public ActionResult AppSettingsCategories(string appID, bool afterPost = false)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var model = new ApplicationViewModel();
            model.User = user;

            model.Application = app;
            model.Page = 10;
            model.afterPost = afterPost;
            model.Uri = Request.Url;
            model.SettingsCategories = _parametersService.GetParameterCategoriesForApplication(app.Id).OrderBy(r => r.Name).ToList();
            return View("Application", model);

        }

        public ActionResult AppSettingsCategoriesCreate(string appID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var model = new AppSettingsCategoriesCreateViewModel();
            model.ApplicationName = app.Name;
            model.ApplicationKey = app.AppKey;
            model.User = user;
            model.Name = string.Empty;
            model.Description = string.Empty;

            return View(model);
        }

        [HttpPost, ActionName("AppSettingsCategoriesCreate")]
        [FormValueRequired("submit.Save")]
        public ActionResult AppSettingsCategoriesCreatePOST(string appID)
        {

            var viewModel = new AppSettingsCategoriesCreateViewModel();
            TryUpdateModel(viewModel);

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                viewModel.User = user;
            }

            var app = _applicationsService.GetApplicationByKey(appID);
            if (app == null)
                return HttpNotFound();

            viewModel.ApplicationKey = app.AppKey;
            viewModel.ApplicationName = app.Name;

            var parameterCategory = _parametersService.CreateParameterCategoryForApplication(app.Id, viewModel.Name, viewModel.Description);

            return RedirectToAction("AppSettingsCategories", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }

        public ActionResult AppSettingsCreate(string appID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var model = new AppSettingsCreateViewModel();
            model.ApplicationName = app.Name;
            model.ApplicationKey = app.AppKey;
            model.User = user;
            model.Name = string.Empty;
            model.Description = string.Empty;
            model.ParameterType = "string";
            model.ParameterTypeI = CBType.stringSetting;
            return View(model);
        }

        [HttpPost, ActionName("AppSettingsCreate")]
        [FormValueRequired("submit.Save")]
        public ActionResult AppSettingsCreatePOST(string appID)
        {

            var viewModel = new AppSettingsCreateViewModel();
            TryUpdateModel(viewModel);

            viewModel.ParameterTypeI = CBDataTypes.TypeFromString(viewModel.ParameterType);

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                viewModel.User = user;
            }

            var app = _applicationsService.GetApplicationByKey(appID);
            if (app == null)
                return HttpNotFound();

            viewModel.ApplicationKey = app.AppKey;
            viewModel.ApplicationName = app.Name;
            var parameter = _parametersService.CreateParameterForApplication(app.Id, viewModel.Name, viewModel.Description);
            _parametersService.SetParameterType(parameter.Id, viewModel.ParameterType);

            switch (viewModel.ParameterTypeI)
            {
                case CBType.boolSetting:
                    _parametersService.SetParameterValue(parameter.Id, viewModel.ParameterValueBool);
                    break;
                case CBType.datetimeSetting:
                    _parametersService.SetParameterValue(parameter.Id, viewModel.ParameterValueDateTime);
                    break;
                case CBType.doubleSetting:
                    _parametersService.SetParameterValue(parameter.Id, viewModel.ParameterValueDouble);
                    break;
                case CBType.intSetting:
                    _parametersService.SetParameterValue(parameter.Id, viewModel.ParameterValueInt);

                    break;
                case CBType.stringSetting:
                    _parametersService.SetParameterValue(parameter.Id, viewModel.ParameterValueString);
                    break;
            }

            return RedirectToAction("AppSettings", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }

        #endregion

        #region Senseapi
        public ActionResult Senseapi(string appID, bool afterPost = false)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var model = new ApplicationViewModel();
            model.User = user;

            model.Application = app;
            model.Page = 12;
            model.afterPost = afterPost;
            model.Uri = Request.Url;
            return View("Application", model);
        }

        public ActionResult SenseapiGames(string appID, bool afterPost = false)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var model = new ApplicationViewModel();
            model.User = user;

            model.Application = app;
            model.Games = _gamesService.GetApplicationGames(app);
            model.Page = 13;
            model.afterPost = afterPost;
            model.Uri = Request.Url;
            return View("Application", model);
        }

        public ActionResult SenseapiGamesAdd(string appID, string gameID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return SenseapiGames(appID);
            }

            if(string.IsNullOrWhiteSpace(gameID))
            {
                var model = new GamesViewModel();
                model.User = user;
                model.Application = app;

                var games = _gamesService.GetNonApplicationGames(user, app).OrderBy(r => r.Name).ToList();
                model.Games = games;

                return View(model);
            }
            else
            {
                var game = _gamesService.GetGameByKey(gameID);

                if (game != null)
                {
                    _gamesService.CreateGameForApplication(app.Name, game.Id);
                }
            }
            return RedirectToAction("SenseapiGames", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });            
        }

        public ActionResult SenseapiGamesRemove(string appID, string gameID)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return SenseapiGames(appID);
            }

            if (!string.IsNullOrWhiteSpace(gameID))
            {
                var game = _gamesService.GetGameByKey(gameID);

                if (game != null)
                {
                    _gamesService.RemoveGameFromApplication(app.Name, game.Id);
                }
            }
            return RedirectToAction("SenseapiGames", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID });
        }

        public ActionResult SenseapiUsers(string appID, PagerParameters pagerParameters, bool afterPost = false)
        {
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            if (app.owner != user.UserName)
            {
                return Application(appID);
            }

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);


            IEnumerable<int> profilesIDs = _profileService.GetUserIDsForApplication(app);

            var users = _orchardServices.ContentManager
                .Query<UserProfilePart, UserProfilePartRecord>().Where(x => profilesIDs.Contains(x.Id));

            var pagerShape = Shape.Pager(pager).TotalItemCount(users.Count());

            var results = users
                .Slice(pager.GetStartIndex(), pager.PageSize)
                .ToList();


            var model = new ApplicationViewModel
            {
                Users = results
                    .Select(x => new UserProfileEntry { User = x.As<UserPart>().Record, Profile = x.As<UserProfilePart>().Record })
                    .ToList(),
                Pager = pagerShape
            };

            model.Application = app;
            model.DefaultRole = _applicationsService.GetDefaultRole(app);
            model.Roles = _applicationsService.GetUserRoles(app);
            model.Page = 14;
            model.afterPost = afterPost;
            model.Uri = Request.Url;
            return View("Application", model);
        }

        public ActionResult SenseapiUsersSessions(string appID, string UserName)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            if (app.owner != user.UserName)
            {
                return SenseapiGames(appID);
            }

            var model = new SessionsViewModel();
            model.User = user;
            model.Application = app;
            model.Page = 14;

            var games = _gamesService.GetApplicationGames(app).OrderBy(r => r.Name).ToList();
            if (games.Count() > 1)
            {
                model.Games = games;

                return View(model);
            }
            else
            {
                try
                {
                    var game = games[0];
                    return RedirectToAction("SenseapiUsersSessionsGame", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID, gameID = game.AppKey, UserName = UserName });
                }
                catch { }
                return Application(appID);
            }
        }

        public ActionResult SenseapiUsersSessionsGame(string appID, string gameID, string UserName)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }
            var app = _applicationsService.GetApplicationByKey(appID);

            if (app == null)
                return HttpNotFound();

            if (app.owner != user.UserName)
            {
                return SenseapiGames(appID);
            }

            if (!string.IsNullOrWhiteSpace(gameID))
            {
                var game = _gamesService.GetGameByKey(gameID);

                if (game != null)
                {
                    var model = new SessionsViewModel();
                    model.User = user;
                    model.Application = app;
                    model.Game = game;
                    model.UserName = UserName;
                    model.Page = 14;
                    var sessions = _sessionsService.GetSessionsForUserInApplication(app.Name, game.Name, UserName).OrderByDescending(r => r.EndDate).ToList();
                    model.Sessions = sessions;

                    return View(model);
                }
            }

            return SenseapiGames(appID);
        }

        //public ActionResult SenseapiUsersResults(string appID, string UserName)
        //{
        //    IUser user = _orchardServices.WorkContext.CurrentUser;

        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var app = _applicationsService.GetApplicationByKey(appID);

        //    if (app == null)
        //        return HttpNotFound();

        //    if (app.owner != user.UserName)
        //    {
        //        return SenseapiGames(appID);
        //    }

        //    var model = new ScoresViewModel();
        //    model.User = user;
        //    model.Application = app;
        //    model.Page = 12;

        //    var games = _applicationsService.GetApplicationGames(app).OrderBy(r => r.Name).ToList();
        //    if (games.Count() > 1)
        //    {
        //        model.Games = games;

        //        return View(model);
        //    }
        //    else
        //    {
        //        var game = games[0];
                
        //        return RedirectToAction("SenseapiUsersResultsGame", "Dashboard", new { area = "CloudBust.Dashboard", appID = appID, gameID = game.AppKey, UserName = UserName });
        //    }
        //}

        //public ActionResult SenseapiUsersResultsGame(string appID, string gameID, string UserName)
        //{
        //    IUser user = _orchardServices.WorkContext.CurrentUser;

        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var app = _applicationsService.GetApplicationByKey(appID);

        //    if (app == null)
        //        return HttpNotFound();

        //    if (app.owner != user.UserName)
        //    {
        //        return SenseapiGames(appID);
        //    }

        //    if (!string.IsNullOrWhiteSpace(gameID))
        //    {
        //        var game = _applicationsService.GetGameByKey(gameID);

        //        if (game != null)
        //        {
        //            var model = new ScoresViewModel();
        //            model.User = user;
        //            model.Application = app;
        //            model.Game = game;
        //            model.UserName = UserName;
        //            model.Page = 12;
        //            var scores = _applicationsService.GetScoreForUserInApplication(app.Name, game.Name, UserName).OrderByDescending(r => r.EndDate).ToList();
        //            model.Scores = scores;

        //            return View(model);            
        //        }
        //    }

        //    return SenseapiGames(appID);
        //}

        #endregion

        #region Feeds
        public ActionResult Feeds()
        {
            return View();
        }

        #endregion

        private ActionResult ApplicationPage(string appID, int page, bool afterPost = false)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new ApplicationViewModel();
            model.User = user;


            var app = _applicationsService.GetApplicationByKey(appID);
            model.Application = app;
            model.Page = page;
            model.afterPost = afterPost;

            if (_settingsService.IsWebApplication())
            {
                if(_settingsService.GetWebApplicationKey() == app.AppKey)
                {
                    model.isWebApp = true;
                }
            }

            string appPath = Request.ApplicationPath;
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority) + appPath;
            UriBuilder b = new UriBuilder(baseUrl);

            model.Uri = b.Uri;
            return View("Application", model);
        }
        private ActionResult GamePage(string gameID, int page, bool afterPost = false)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new GameViewModel();
            model.User = user;


            var game = _gamesService.GetGameByKey(gameID);
            model.Game = game;

            model.Page = page;
            model.afterPost = afterPost;
            model.Uri = Request.Url;
            model.Events = _gamesService.GetGameEvents(game);
            return View("Game", model);
        }
        private ActionResult TablePage(string datatableID, int page, bool afterPost = false)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new TableViewModel();
            model.User = user;

            int datatableid;
            if (!Int32.TryParse(datatableID, out datatableid))
            {
                return HttpNotFound();
            }
            var datatable = _datatablesService.GetDataTable(datatableid);
            if(datatable == null)
                return HttpNotFound();

            model.DataTable = datatable;

            model.Page = page;
            model.afterPost = afterPost;
            model.Uri = Request.Url;
            model.Fields = _datatablesService.GetFieldsForDataTable(datatableid);

            model.Applications = _datatablesService.GetDataTableApplications(datatable);

            return View("Table", model);
        }
    }
}
