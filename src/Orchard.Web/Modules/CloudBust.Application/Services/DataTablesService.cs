using System;
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
        private readonly IRepository<FieldRecord> _fieldsRepository;
        private readonly IApplicationsService _applicationsService;
        private readonly IDataNotificationService _dataNotificationService;

        public DataTablesService(
                                IContentManager contentManager
                                ,IOrchardServices orchardServices
                                ,IRepository<ApplicationDataTableRecord> datatablesRepository
                                ,IRepository<FieldRecord> fieldsRepository
                                ,IApplicationsService applicationsService
                                ,IDataNotificationService datanotificationService
                                ,IClock clock
            )
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _datatablesRepository = datatablesRepository;
            _fieldsRepository = fieldsRepository;
            _applicationsService = applicationsService;
            _dataNotificationService = datanotificationService;
            _clock = clock;
        }
        // Data Table
        #region Data Table

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
        public IEnumerable<ApplicationRecord> GetDataTableApplications(int dataTableId)
        {
            ApplicationDataTableRecord applicationDataTableRecord = GetDataTable(dataTableId);
            return GetDataTableApplications(applicationDataTableRecord);
        }
        public IEnumerable<ApplicationRecord> GetDataTableApplications(ApplicationDataTableRecord applicationDataTableRecord)
        {
            var applications = new List<ApplicationRecord>();
            if (applicationDataTableRecord == null) return applications;

            var totalapplications = _applicationsService.GetApplications();
            if (totalapplications == null) return null;

            foreach(var app in totalapplications)
            {
                if (app.DataTables.FirstOrDefault(d => d.ApplicationDataTable.Id == applicationDataTableRecord.Id) != null)
                    applications.Add(app);
            }

            return applications;
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

        // Fields
        #region Fields

        public FieldRecord GetField(int Id)
        {
            try
            {
                var fieldRecord = _fieldsRepository.Get(Id);
                return fieldRecord;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<FieldRecord> GetFieldsForDataTable(int Id)
        {
            var fields = new List<FieldRecord>();
            ApplicationDataTableRecord applicationDataTableRecord = GetDataTable(Id);
            foreach (ApplicationDataTableFieldsRecord field in applicationDataTableRecord.Fields)
            {
                fields.Add(GetField(field.Field.Id));
            }
            return fields;
        }

        public FieldRecord CreateField(string fieldName, string fieldDescription, string fieldType)
        {
            FieldRecord r = new FieldRecord();
            r.Name = fieldName;
            r.Description = fieldDescription;
            r.NormalizedName = fieldName.ToLowerInvariant();
            r.FieldType = fieldType;

            _fieldsRepository.Create(r);

            //TriggerSignal();

            return r;
        }
        public bool UpdateField(int id, string Name, string Description, string FieldType)
        {
            FieldRecord r = GetField(id);
            if (r == null) return false;

            r.Name = Name;
            r.Description = Description;
            r.NormalizedName = Name.ToLowerInvariant();
            r.FieldType = FieldType;

            //_dataNotificationService.GameUpdated(datatableName, gameRecord);
            return true;
        }
        public FieldRecord SetFieldType(int fieldId, string fieldType)
        {
            FieldRecord fieldRecord = GetField(fieldId);
            if (fieldRecord == null) return null;

            fieldRecord.FieldType = fieldType;
            //TriggerSignal();

            return fieldRecord;
        }

        public bool DeleteField(FieldRecord fieldRecord)
        {
            if (fieldRecord == null) return false;

            _fieldsRepository.Delete(fieldRecord);

            //TriggerSignal();
            return true;
        }
        public bool DeleteField(int fieldId)
        {
            FieldRecord fieldRecord = GetField(fieldId);
            return DeleteField(fieldRecord);
        }
        public FieldRecord CreateFieldForApplicationDataTable(int dataTableId, int fieldId)
        {
            ApplicationDataTableRecord applicationDatatTableRecord = GetDataTable(dataTableId);
            if (applicationDatatTableRecord == null) return null;

            FieldRecord fieldRecord = GetField(fieldId);
            if (fieldRecord == null) return null;

            applicationDatatTableRecord.Fields.Add(new ApplicationDataTableFieldsRecord { ApplicationDataTable = applicationDatatTableRecord, Field = fieldRecord });

            //_dataNotificationService.ApplicationUpdated(moduleRecord);
            return fieldRecord;
        }
        public bool RemoveFieldFromApplicationDataTable(int dataTableId, int fieldId)
        {
            ApplicationDataTableRecord applicationDatatTableRecord = GetDataTable(dataTableId);
            if (applicationDatatTableRecord == null) return false;

            foreach (ApplicationDataTableFieldsRecord fieldLinkRecord in applicationDatatTableRecord.Fields)
            {
                if (fieldLinkRecord.Field.Id == fieldId)
                {
                    applicationDatatTableRecord.Fields.Remove(fieldLinkRecord);
                    break;
                }
            }

            //_dataNotificationService.ApplicationUpdated(moduleRecord);
            return true;
        }

        #endregion



        //public dynamic GetFieldValue(FieldRecord fieldRecord)
        //{
        //    if (fieldRecord == null) return null;

        //    CBType p = CBDataTypes.TypeFromString(fieldRecord.FieldType.ToLower());

        //    switch (p)
        //    {
        //        case CBType.intSetting:
        //            return fieldRecord.FieldValueInt;
        //        case CBType.doubleSetting:
        //            return fieldRecord.FieldValueDouble;
        //        case CBType.boolSetting:
        //            return fieldRecord.FieldValueBool;
        //        case CBType.datetimeSetting:
        //            return fieldRecord.FieldValueDateTime;
        //        default:
        //            return fieldRecord.FieldValueString;
        //    }
        //}
        //public dynamic GetFieldValue(int fieldId)
        //{
        //    FieldRecord fieldRecord = GetField(fieldId);
        //    return GetFieldValue(fieldRecord);
        //}
        //public void SetFieldValue(int fieldId, string fieldValue)
        //{
        //    FieldRecord fieldRecord = GetField(fieldId);
        //    if (fieldRecord == null) return;

        //    fieldRecord.FieldValueString = fieldValue;
        //    //TriggerSignal();
        //}
        //public void SetFieldValue(int fieldId, int fieldValue)
        //{
        //    FieldRecord fieldRecord = GetField(fieldId);
        //    if (fieldRecord == null) return;

        //    fieldRecord.FieldValueInt = fieldValue;
        //    //TriggerSignal();
        //}
        //public void SetFieldValue(int fieldId, double fieldValue)
        //{
        //    FieldRecord fieldRecord = GetField(fieldId);
        //    if (fieldRecord == null) return;

        //    fieldRecord.FieldValueDouble = fieldValue;
        //    //TriggerSignal();
        //}
        //public void SetFieldValue(int fieldId, bool fieldValue)
        //{
        //    FieldRecord fieldRecord = GetField(fieldId);
        //    if (fieldRecord == null) return;

        //    fieldRecord.FieldValueBool = fieldValue;
        //    //TriggerSignal();
        //}
        //public void SetFieldValue(int fieldId, DateTime fieldValue)
        //{
        //    FieldRecord fieldRecord = GetField(fieldId);
        //    if (fieldRecord == null) return;

        //    fieldRecord.FieldValueDateTime = fieldValue;
        //    //TriggerSignal();
        //}
    }
}