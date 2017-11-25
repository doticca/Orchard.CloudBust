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
    public class oDataGameController: ApiController
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationsService _applicationsService;
        private readonly IGamesService _gamesService;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public oDataGameController(
                               IOrchardServices orchardServices
                               , IApplicationsService applicationsService
                               , IGamesService gamesService
                               , ICacheManager cacheManager
                               , ISignals signals 
            )
        {
            _orchardServices = orchardServices;
            _applicationsService = applicationsService;
            _gamesService = gamesService;
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

        [HttpGet]
        public HttpResponseMessage GetGame(string key = null)
        {
            var cachekey = CBSignals.SignalApplications;
            if (!string.IsNullOrWhiteSpace(key)) { 
                var cat = _cacheManager.Get(cachekey, ctx =>
                            {
                                ctx.Monitor(_signals.When(cachekey));
                                return new ApplicationGame(_gamesService.GetGameByKey(key), Request);
                            }
                        );
                return Request.CreateResponse(HttpStatusCode.OK, cat);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [ExtendedQueryable]
        [Orchard.Core.XmlRpc.Controllers.LiveWriterController.NoCache]
        public IQueryable<CloudBust.Application.OData.ApplicationGame> GetGames()
        {
            int id = GetAppId();
            if (id <= 0)
            {
                return null;
            }

            var key = CBSignals.SignalApplications;
            var mods = _cacheManager.Get(key, ctx =>
            {
                ctx.Monitor(_signals.When(key));
                return _gamesService.GetApplicationGames(id)
                    .Select(c => new CloudBust.Application.OData.ApplicationGame(c, Request));
            }
            );

            return mods.AsQueryable();
        }
            
        [ActionName("Score")]
        [HttpPut]
        public HttpResponseMessage PutScore(string key, uGameScore gameScore)
        {
            if (gameScore == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
            }

            IUser user = _orchardServices.WorkContext.CurrentUser;
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
            }
            int id = GetAppId();
            if(id==0)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));

            var app = _applicationsService.GetApplication(id);
            if(app == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            var game = _gamesService.GetGameByKey(key);
            if(game == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));

            GameScoreRecord r = new GameScoreRecord();

            r.Agility = gameScore.Agility;
            r.AgilityScore = gameScore.AgilityScore;
            r.AgilityText = gameScore.AgilityText;
            r.Accuracy = gameScore.Accuracy;
            r.AccuracyScore = gameScore.AccuracyScore;
            r.AccuracyText = gameScore.AccuracyText;
            r.ApplicationName = app.Name;
            r.Attention = gameScore.Attention;
            r.EndDate = gameScore.EndDate;
            r.Executive = gameScore.Executive;
            r.GameName = game.Name;
            r.Score = gameScore.Score;
            r.Smoothness = gameScore.Smoothness;
            r.SmoothnessScore = gameScore.SmoothnessScore;
            r.SmoothnessText = gameScore.SmoothnessText;
            r.Spatial = gameScore.Spatial;
            r.Stability = gameScore.Stability;
            r.StabilityScore = gameScore.StabilityScore;
            r.StabilityText = gameScore.StabilityText;
            r.StartDate = gameScore.StartDate;
            r.UserName = user.UserName;

            _gamesService.StoreScore(r);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
       
    }
}