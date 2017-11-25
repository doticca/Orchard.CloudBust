using CloudBust.Application.Models;
using CloudBust.Application.OData;
using CloudBust.Application.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.Records;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Orchard.Caching;
using CloudBust.Common.Extensions;
using CloudBust.Common.OData;

namespace CloudBust.Application.Controllers
{
    public class oDataFieldsController : ApiController
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IApplicationsService _applicationsService;
        private readonly IParametersService _parametersService;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public oDataFieldsController(
                               IOrchardServices orchardServices
                               ,IApplicationsService applicationsService
                               ,IParametersService parametersService
                               , ICacheManager cacheManager
                               ,ISignals signals 
            )
        {
            _orchardServices = orchardServices;
            _applicationsService = applicationsService;
            _parametersService = parametersService;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        [HttpGet]
        public HttpResponseMessage Field(int id)
        {
            int requestobject = 0;  // this flag is for direct calls to do an error handling
            if (Request.RequestUri.AbsolutePath.ToLower().Contains("/v1/categories"))
                requestobject = 1;
            if (Request.RequestUri.AbsolutePath.ToLower().Contains("/v1/applications"))
                requestobject = 2;
            if (Request.RequestUri.AbsolutePath.ToLower().Contains("/v1/parameters/categories"))
                requestobject = 3;
            if (requestobject <= 0)
                return Request.CreateResponse(HttpStatusCode.Conflict, new uError("Wrong URL formating",409));

            switch (requestobject)
            {
                case 1:
                    return CategoryField(id);
                case 2:
                    return ModuleField(id);
                case 3:
                    return ParameterCategoryField(id);

            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        private HttpResponseMessage CategoryField(int id)
        {
            ApplicationCategoryRecord cat = null;
            if(id==0)
            {
                cat = new ApplicationCategoryRecord
                {
                    Id = 0,
                    Description = "Browse all Applications",
                    Name = "All Applications"
                };
            }
            else
                cat = _applicationsService.GetCategory(id);

            if (cat == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Category not found", 404));
            }

            string r = Request.RequestUri.AbsolutePath.ToLower();

            if (r.EndsWith("name"))// && r.Count(f => f == '/') == 4)
            {
                OData.Category.fieldName category = new OData.Category.fieldName(cat, Request);
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
            if (r.EndsWith("description"))// && r.Count(f => f == '/') == 4)
            {
                OData.Category.fieldDescription category = new OData.Category.fieldDescription(cat, Request);
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        private HttpResponseMessage ParameterCategoryField(int id)
        {
            var cat = _parametersService.GetParameterCategory(id);
            if (cat == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Parameter Category not found", 404));
            }

            string r = Request.RequestUri.AbsolutePath.ToLower();

            if (r.EndsWith("name"))// && r.Count(f => f == '/') == 4)
            {
                OData.ParameterCategory.fieldName category = new OData.ParameterCategory.fieldName(cat, Request);
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
            if (r.EndsWith("description"))// && r.Count(f => f == '/') == 4)
            {
                OData.ParameterCategory.fieldDescription category = new OData.ParameterCategory.fieldDescription(cat, Request);
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        private HttpResponseMessage ModuleField(int id)
        {
            var mod = _applicationsService.GetApplication(id);
            if (mod == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new uError("Application not found", 404));
            }

            string r = Request.RequestUri.AbsolutePath.ToLower();

            if (r.EndsWith("name"))// && r.Count(f => f == '/') == 4)
            {
                OData.Application.fieldName module = new OData.Application.fieldName(mod, Request);
                return Request.CreateResponse(HttpStatusCode.OK, module);
            }
            if (r.EndsWith("description"))// && r.Count(f => f == '/') == 4)
            {
                OData.Application.fieldDescription module = new OData.Application.fieldDescription(mod, Request);
                return Request.CreateResponse(HttpStatusCode.OK, module);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}