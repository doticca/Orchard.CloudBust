using System.Collections.Generic;
using System.Linq;
using CloudBust.Application.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Security;
using Orchard.Services;


namespace CloudBust.Application.Services
{    
    public class DataTablesService : IDataTablesService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IContentManager _contentManager;
        private readonly IClock _clock;

        private readonly IRepository<ApplicationDataTableRecord> _datatablesRepository;
        private readonly IApplicationsService _applicationsService;
        private readonly IDataNotificationService _dataNotificationService;

        public DataTablesService(
                                IContentManager contentManager
                                ,IOrchardServices orchardServices
                                ,IRepository<ApplicationDataTableRecord> datatablesRepository
                                ,IApplicationsService applicationsService
                                ,IDataNotificationService datanotificationService
                                ,IClock clock
            )
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _datatablesRepository = datatablesRepository;
            _applicationsService = applicationsService;
            _dataNotificationService = datanotificationService;
            _clock = clock;
        }
        // APPLICATION GAMES
        #region Application Games

        public ApplicationDataTableRecord GetDataTable(int Id)
        {
            try
            {
                var datatable = _datatablesRepository.Get(Id);
                return datatable;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<ApplicationDataTableRecord> GetDataTables()
        {
            try
            {
                var datatables = from datatable in _datatablesRepository.Table select datatable;

                return datatables.ToList();
            }
            catch
            {
                return new List<ApplicationDataTableRecord>();
            }
        }
        public IEnumerable<ApplicationDataTableRecord> GetApplicationDataTables(ApplicationRecord applicationRecord)
        {
            var datatables = new List<ApplicationDataTableRecord>();

            if (applicationRecord == null)
                return datatables;

            foreach (ApplicationApplicationDataTablesRecord datatable in applicationRecord.DataTables)
            {
                datatables.Add(GetDataTable(datatable.ApplicationDataTable.Id));
            }
            return datatables;
        }
        public IEnumerable<ApplicationDataTableRecord> GetApplicationDataTables(int applicationId)
        {
            ApplicationRecord applicationRecord =  _applicationsService.GetApplication(applicationId);
            return GetApplicationDataTables(applicationRecord);
        }
        public ApplicationDataTableRecord GetDataTableByName(string datatableName)
        {
            try
            {
                return _datatablesRepository.Get(x => x.Name == datatableName);
            }
            catch
            {
                return null;
            }
        }
        public ApplicationDataTableRecord CreateDataTable(string datatableName, string datatableDescription)
        {
            var utcNow = _clock.UtcNow;
            ApplicationDataTableRecord r = new ApplicationDataTableRecord();
            r.Name = datatableName;
            r.Description = datatableDescription;
            r.NormalizedTableName = datatableName.ToLowerInvariant();
            r.CreatedUtc = utcNow;
            r.ModifiedUtc = utcNow;

            _datatablesRepository.Create(r);

            r = GetDataTableByName(datatableName);

            return r;
        }
        private bool DeleteDataTable(int Id)
        {
            ApplicationDataTableRecord datatable = GetDataTable(Id);
            if (datatable == null) return false;
            string datatableName = datatable.NormalizedTableName;
            _datatablesRepository.Delete(datatable);
            //_dataNotificationService.GameUpdated(gameName);
            return true;
        }
        public bool UpdateDataTable(int id, string datatableName, string datatableDescription)
        {
            ApplicationDataTableRecord datatableRecord = GetDataTable(id);

            datatableRecord.Name = datatableName;
            datatableRecord.Description = datatableDescription;
            datatableRecord.NormalizedTableName = datatableName.ToLowerInvariant();

            //_dataNotificationService.GameUpdated(datatableName, gameRecord);
            return true;
        }
        public bool CreateDataTableForApplication(string applicationName, int datatableId)
        {
            ApplicationDataTableRecord datatableRecord = GetDataTable(datatableId);
            if (datatableRecord == null) return false;

            ApplicationRecord moduleRecord = _applicationsService.GetApplicationByName(applicationName);
            if (moduleRecord == null) return false;

            moduleRecord.DataTables.Add(new ApplicationApplicationDataTablesRecord { Application = moduleRecord, ApplicationDataTable = datatableRecord });

            _dataNotificationService.ApplicationUpdated(moduleRecord);
            return true;
        }
        public bool RemoveDataTableFromApplication(string applicationName, int datatableId)
        {
            ApplicationRecord moduleRecord = _applicationsService.GetApplicationByName(applicationName);
            if (moduleRecord == null) return false;

            foreach (ApplicationApplicationDataTablesRecord datatableRecord in moduleRecord.DataTables)
            {
                if (datatableRecord.ApplicationDataTable.Id == datatableId)
                {
                    moduleRecord.DataTables.Remove(datatableRecord);
                    break;
                }
            }

            _dataNotificationService.ApplicationUpdated(moduleRecord);
            return true;
        }

        public IEnumerable<ApplicationDataTableRecord> GetNonApplicationDataTables(IUser user, ApplicationRecord applicationRecord)
        {
            if (applicationRecord == null)
                return null;

            // user games
            var datatables = from datatable in _datatablesRepository.Table select datatable;
            // app games
            //var appgames = GetApplicationGames(applicationRecord);

            var newdatatables = new List<ApplicationDataTableRecord>();
            foreach (ApplicationDataTableRecord datatable in datatables)
            {
                bool found = false;
                foreach (ApplicationApplicationDataTablesRecord datatableref in applicationRecord.DataTables)
                {
                    if (datatableref.ApplicationDataTable.Id == datatable.Id)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    newdatatables.Add(datatable);
                }
            }
            return newdatatables;
        }
        #endregion
    }
}