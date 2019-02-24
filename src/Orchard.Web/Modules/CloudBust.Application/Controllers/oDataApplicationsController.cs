using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloudBust.Application.OData.Category;
using CloudBust.Application.Services;
using CloudBust.Common.Extensions;
using CloudBust.Common.OData;
using Orchard;
using Orchard.Caching;
using Orchard.Core.XmlRpc.Controllers;

namespace CloudBust.Application.Controllers
{
    public class oDataApplicationsController : ApiController
    {
        private readonly IApplicationsService _applicationsService;
        private readonly ICacheManager _cacheManager;
        private readonly IOrchardServices _orchardServices;
        private readonly IParametersService _parametersService;
        private readonly ISignals _signals;

        public oDataApplicationsController(IOrchardServices orchardServices, IApplicationsService applicationsService, IParametersService parametersService, ICacheManager cacheManager, ISignals signals)
        {
            _orchardServices = orchardServices;
            _applicationsService = applicationsService;
            _parametersService = parametersService;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        private string HostUrl()
        {
            return _orchardServices.WorkContext.CurrentSite.BaseUrl;
        }

        [ExtendedQueryable]
        [LiveWriterController.NoCache]
        public IQueryable<Category> GetCategories()
        {
            var key = CBSignals.SignalApplications;
            var cats = _cacheManager.Get(key, ctx =>
                                              {
                                                  ctx.Monitor(_signals.When(key));
                                                  return _applicationsService.GetCategories().Select(c => new Category(c, Request));
                                              });

            var categories = new List<Category> {new Category(Request)};
            categories.AddRange(cats);

            return categories.AsQueryable();
        }

        [ExtendedQueryable]
        [LiveWriterController.NoCache]
        public IQueryable<OData.Application.Application> GetApplications()
        {
            var key = CBSignals.SignalApplications;
            var mods = _cacheManager.Get(key, ctx =>
                                              {
                                                  ctx.Monitor(_signals.When(key));
                                                  return _applicationsService.GetApplications().Select(c => new OData.Application.Application(c, HostUrl()));
                                              });

            return mods.AsQueryable();
        }

        [HttpGet]
        public HttpResponseMessage GetCategory(int id)
        {
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.OK, new Category(Request));

            var key = CBSignals.SignalApplications;
            var cat = _cacheManager.Get(key, ctx =>
                                             {
                                                 ctx.Monitor(_signals.When(key));
                                                 var category = _applicationsService.GetCategory(id);
                                                 if (category != null)
                                                     return new Category(category, Request);
                                                 return null;
                                             });

            if (cat != null) return Request.CreateResponse(HttpStatusCode.OK, cat);
            return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
        }


        [HttpGet]
        public HttpResponseMessage GetApplication(int id = 0)
        {
            if (id <= 0)
            {
                if (_orchardServices.WorkContext.CurrentUser == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));

                try
                {
                    var appid = _orchardServices.WorkContext.HttpContext.Session["doticca_aid"].ToString();

                    if (string.IsNullOrWhiteSpace(appid))
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));

                    if (!int.TryParse(appid, out var aid))
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));

                    id = aid;
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new uError("User not authorized", 401));
                }
            }

            var key = CBSignals.SignalApplications;

            var mod = _cacheManager.Get(key, ctx =>
                                             {
                                                 ctx.Monitor(_signals.When(key));
                                                 var application = _applicationsService.GetApplication(id);
                                                 if (application != null)
                                                     return new OData.Application.Application(_applicationsService.GetApplication(id), HostUrl());
                                                 return null;
                                             });

            if (mod != null) return Request.CreateResponse(HttpStatusCode.OK, mod);
            return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Not Found", 404));
        }

        [HttpGet]
        public HttpResponseMessage GetCategoriesCount()
        {
            var cats = _applicationsService.GetCategories().Count();

            return Request.CreateResponse(HttpStatusCode.OK, new Totals(cats, "Category"));
        }

        [HttpGet]
        public HttpResponseMessage GeApplicationsCount()
        {
            var mods = _applicationsService.GetApplications().Count();

            return Request.CreateResponse(HttpStatusCode.OK, new Totals(mods, "Application"));
        }
    }
}