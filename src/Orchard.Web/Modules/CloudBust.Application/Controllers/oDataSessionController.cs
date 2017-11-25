using CloudBust.Application.Models;
using CloudBust.Application.OData;
using CloudBust.Application.Services;
using CloudBust.Common.Extensions;
using CloudBust.Common.OData;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CloudBust.Application.Controllers
{
    public class oDataSessionController: ApiController
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationsService _applicationsService;
        private readonly IGamesService _gamesService;
        private readonly ISessionsService _sessionsService;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public oDataSessionController(
                                 IOrchardServices orchardServices
                                ,IApplicationsService applicationsService
                                , IGamesService gamesService
                                , ISessionsService sessionsService
                                ,ICacheManager cacheManager
                                ,ISignals signals 
            )
        {
            _orchardServices = orchardServices;
            _applicationsService = applicationsService;
            _gamesService = gamesService;
            _sessionsService = sessionsService;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        private int GetAppId()
        {
            if(_orchardServices.WorkContext.CurrentUser == null)
                return 0;

            try
            {
                string appid = _orchardServices.WorkContext.HttpContext.Session["doticca_aid"].ToString();

                if (string.IsNullOrWhiteSpace(appid))
                    return 0;

                int aid;
                if (!Int32.TryParse(appid, out aid))
                    return 0;

                return aid;
            }
            catch
            {
                return 0;
            }
        }

        private ApplicationRecord GetApplication(int id)
        {
            return _applicationsService.GetApplication(id);
        }

        private ApplicationGameRecord GetGame(string key = null)
        {
            //var cachekey = ApplicationsService.SignalName;
            //if (!string.IsNullOrWhiteSpace(key))
            //{
            //    var game = _cacheManager.Get(cachekey, ctx =>
            //                    {
            //                        ctx.Monitor(_signals.When(cachekey));
            //                        return _applicationsService.GetGameByKey(key);
            //                    }
            //                );
            //    return game;
            //}
            return _gamesService.GetGameByKey(key);
            //return null;
        }



        [ActionName("SessionStart")]
        [HttpGet]
        public HttpResponseMessage SessionStart(string key)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            }
            int id = GetAppId();
            if(id==0)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));

            var app = GetApplication(id);
            if(app == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            var game = GetGame(key);
            if(game == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            int sId = _sessionsService.StartSession(app.Name, game.Name, user.UserName);
            if(sId > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, sId);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
        }

        private HttpResponseMessage CheckSession(string key, int id, out SessionRecord session)
        {
            IUser user = _orchardServices.WorkContext.CurrentUser;
            if (user == null)
            {
                session = null;
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            }
            int aid = GetAppId();
            if (aid == 0)
            {
                session = null;
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            }
            var app = GetApplication(aid);
            if (app == null)
            {
                session = null;
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            }
            var game = GetGame(key);
            if (game == null)
            {
                session = null;
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            }
            session = _sessionsService.GetSession(id);
            if (session.ApplicationName != app.Name || session.GameName != game.Name)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            }
            return Request.CreateResponse(HttpStatusCode.OK, id);
        }

        [ActionName("SessionEnd")]
        [HttpGet]
        public HttpResponseMessage SessionEnd(string key, int id)
        {
            SessionRecord session;
            HttpResponseMessage response = CheckSession(key, id, out session);
            if (session == null) return response;

            _sessionsService.EndSession(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [ActionName("SessionEventStart")]
        [HttpPut]
        public HttpResponseMessage PutSessionEventStart(string key, int id, uSessionEvent sessionEvent)
        {
            SessionRecord session;
            HttpResponseMessage response = CheckSession(key, id, out session);
            if (session == null) return response;

            _sessionsService.StartSessionEvent(id, sessionEvent.Name, sessionEvent.StimulaeType, sessionEvent.ObjectType);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [ActionName("SessionEventEnd")]
        [HttpPut]
        public HttpResponseMessage PutSessionEventEnd(string key, int id, uSessionEvent sessionEvent)
        {
            SessionRecord session;
            HttpResponseMessage response = CheckSession(key, id, out session);
            if (session == null) return response;

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        //[ActionName("SessionScore")]
        //[HttpPut]
        //public HttpResponseMessage PutSessionScore(string key, uGameScore gameScore)
        //{
        //    if (gameScore == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
        //    }

        //    IUser user = _orchardServices.WorkContext.CurrentUser;
        //    if (user == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
        //    }
        //    int id = GetAppId();
        //    if(id==0)
        //        return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));

        //    var app = _applicationsService.GetApplication(id);
        //    if(app == null)
        //        return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

        //    var game = _applicationsService.GetGameByKey(key);
        //    if(game == null)
        //        return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

        //    GameScoreRecord r = new GameScoreRecord();

        //    r.Agility = gameScore.Agility;
        //    r.AgilityScore = gameScore.AgilityScore;
        //    r.AgilityText = gameScore.AgilityText;
        //    r.Accuracy = gameScore.Accuracy;
        //    r.AccuracyScore = gameScore.AccuracyScore;
        //    r.AccuracyText = gameScore.AccuracyText;
        //    r.ApplicationName = app.Name;
        //    r.Attention = gameScore.Attention;
        //    r.EndDate = gameScore.EndDate;
        //    r.Executive = gameScore.Executive;
        //    r.GameName = game.Name;
        //    r.Score = gameScore.Score;
        //    r.Smoothness = gameScore.Smoothness;
        //    r.SmoothnessScore = gameScore.SmoothnessScore;
        //    r.SmoothnessText = gameScore.SmoothnessText;
        //    r.Spatial = gameScore.Spatial;
        //    r.Stability = gameScore.Stability;
        //    r.StabilityScore = gameScore.StabilityScore;
        //    r.StabilityText = gameScore.StabilityText;
        //    r.StartDate = gameScore.StartDate;
        //    r.UserName = user.UserName;

        //    _applicationsService.StoreScore(r);

        //    return Request.CreateResponse(HttpStatusCode.NoContent);
        //}
       
    }
}