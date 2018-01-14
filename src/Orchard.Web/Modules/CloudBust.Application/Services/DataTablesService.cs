using System;
using System.Collections.Generic;
using System.Linq;
using CloudBust.Application.Models;
using Newtonsoft.Json.Linq;
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
        private readonly IRepository<RowRecord> _rowsRepository;
        private readonly IRepository<CellRecord> _cellsRepository;
        private readonly IRepository<ApplicationDataTableFieldsRecord> _appFieldsRepository;
        private readonly IRepository<ApplicationDataTableRowsRecord> _appRowsRepository;

        private readonly IRepository<StringFieldValueRecord> _valuesStringRepository;
        private readonly IRepository<BoolFieldValueRecord> _valuesBoolRepository;
        private readonly IRepository<IntegerFieldValueRecord> _valuesIntRepository;
        private readonly IRepository<DoubleFieldValueRecord> _valuesDoubleRepository;
        private readonly IRepository<DateTimeFieldValueRecord> _valuesDateTimeRepository;


        private readonly IApplicationsService _applicationsService;
        private readonly IDataNotificationService _dataNotificationService;

        public DataTablesService(
                                IContentManager contentManager
                                ,IOrchardServices orchardServices
                                ,IRepository<ApplicationDataTableRecord> datatablesRepository
                                ,IRepository<FieldRecord> fieldsRepository
                                ,IRepository<RowRecord> rowsRepository
                                ,IRepository<CellRecord> cellsRepository
                                ,IRepository<StringFieldValueRecord> valuesStringRepository
                                ,IRepository<BoolFieldValueRecord> valuesBoolRepository
                                ,IRepository<IntegerFieldValueRecord> valuesIntRepository
                                ,IRepository<DoubleFieldValueRecord> valuesDoubleRepository
                                ,IRepository<DateTimeFieldValueRecord> valuesDateTimeRepository
                                ,IRepository<ApplicationDataTableFieldsRecord> appFieldsRepository
                                ,IRepository<ApplicationDataTableRowsRecord> appRowsRepository
                                ,IApplicationsService applicationsService
                                ,IDataNotificationService datanotificationService
                                ,IClock clock
            )
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _datatablesRepository = datatablesRepository;
            _fieldsRepository = fieldsRepository;
            _rowsRepository = rowsRepository;
            _cellsRepository = cellsRepository;
            _appFieldsRepository = appFieldsRepository;
            _appRowsRepository = appRowsRepository;

            _valuesStringRepository = valuesStringRepository;
            _valuesBoolRepository = valuesBoolRepository;
            _valuesIntRepository = valuesIntRepository;
            _valuesDoubleRepository = valuesDoubleRepository;
            _valuesDateTimeRepository = valuesDateTimeRepository;

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
        public ApplicationDataTableRecord GetDataTable(RowRecord row)
        {
            try
            {
                return _appRowsRepository.Get(x => x.Row.Id == row.Id).ApplicationDataTable;
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
            if (applicationDataTableRecord == null) return fields;
            foreach (ApplicationDataTableFieldsRecord fieldPointer in applicationDataTableRecord.Fields)
            {
                var Field = GetField(fieldPointer.Field.Id);
                if(Field!=null)
                    fields.Add(Field);
            }
            return fields.OrderBy(x => x.Position).ToList();
        }

        public FieldRecord CreateField(string fieldName, string fieldDescription, string fieldType)
        {
            CBType p = CBDataTypes.TypeFromString(fieldType.ToLower());
            FieldRecord r = new FieldRecord();
            r.Name = fieldName;
            r.Description = fieldDescription;
            r.NormalizedName = fieldName.ToLowerInvariant();
            r.FieldType = CBDataTypes.StringFromType(p);

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

            fieldRecord.Position = GetFieldLastPosition(fieldRecord, dataTableId) + 1;

            applicationDatatTableRecord.Fields.Add(new ApplicationDataTableFieldsRecord { ApplicationDataTable = applicationDatatTableRecord, Field = fieldRecord });

            //_dataNotificationService.ApplicationUpdated(moduleRecord);
            return fieldRecord;
        }
        public int GetFieldLastPosition(FieldRecord fieldRecord, int dataTableId)
        {

            var fields = from field in _appFieldsRepository.Table where field.ApplicationDataTable.Id == dataTableId select field.Field;
            if (fields == null) return 0;

            try
            {
                var item = fields.Max<FieldRecord>(i => i.Position);
                return item;
            }
            catch
            {
                return 0;
            }
        }
        public void FieldPositionUp(int Id, int dataTableId)
        {
            FieldRecord r = GetField(Id);
            if (r == null) return;

            var fields = from field in _appFieldsRepository.Table where field.ApplicationDataTable.Id == dataTableId select field.Field;
            if (fields == null) return;

            try
            {
                FieldRecord s = (from previous in fields where previous.Position == r.Position - 1 select previous).FirstOrDefault();
                if (s != null)
                {
                    s.Position = r.Position;
                    r.Position = r.Position - 1;
                }
                //_dataNotificationService.GameUpdated(s.ApplicationGameRecord.NormalizedGameName, s.ApplicationGameRecord);
            }
            catch
            {
                return;
            }
        }
        public void FieldPositionDown(int Id, int dataTableId)
        {
            FieldRecord r = GetField(Id);
            if (r == null) return;

            var fields = from field in _appFieldsRepository.Table where field.ApplicationDataTable.Id == dataTableId select field.Field;
            if (fields == null) return;

            try
            {
                FieldRecord s = (from next in fields where next.Position == r.Position + 1 select next).FirstOrDefault();
                if (s != null)
                {
                    s.Position = r.Position;
                    r.Position = r.Position + 1;
                }
                //_dataNotificationService.GameUpdated(s.ApplicationGameRecord.NormalizedGameName, s.ApplicationGameRecord);
            }
            catch
            {
                return;
            }
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

        public RowRecord CreateRowForTable(int dataTableId)
        {
            ApplicationDataTableRecord applicationDatatTableRecord = GetDataTable(dataTableId);
            if (applicationDatatTableRecord == null) return null;


            RowRecord r = GetLastNewRow(dataTableId);
            if (r == null)
            {
                r = new RowRecord();

                _rowsRepository.Create(r);

                applicationDatatTableRecord.Rows.Add(new ApplicationDataTableRowsRecord() { ApplicationDataTable = applicationDatatTableRecord, Row = r, IsNew = true });
                CreateDefaultsForRow(r);
            }
            //TriggerSignal();

            return r;
        }
        public void DeleteRow(int datatableId, int rowId)
        {
            ApplicationDataTableRecord table = GetDataTable(datatableId);
            if (table == null) return;
            var existingrow = table.Rows.Where(tr => tr.Row.Id == rowId).FirstOrDefault();
            if (existingrow == null)
                return;
            DeleteCells(table, existingrow.Row);
            table.Rows.Remove(existingrow);
        }
        private void DeleteCells(ApplicationDataTableRecord table, RowRecord row)
        {
            var fields = GetFieldsForDataTable(table.Id);
            if (fields == null || fields.Count() < 1) return;
            foreach (var field in fields)
            {
                DeleteCell(row, field);
            }
        }
        private void DeleteCell(RowRecord row, FieldRecord col)
        {
            CellRecord cell = GetCell(row, col);
            if (cell == null) return;
            CBType p = CBDataTypes.TypeFromString(col.FieldType.ToLower());

            switch (p)
            {
                case CBType.intSetting:
                    var intval = cell.Field.IntegerFieldValueRecord.Where(v => v.Id == cell.ValueId).FirstOrDefault();
                    cell.Field.IntegerFieldValueRecord.Remove(intval);
                    break;
                case CBType.doubleSetting:
                    var doubleval = cell.Field.DoubleFieldValueRecord.Where(v => v.Id == cell.ValueId).FirstOrDefault();
                    cell.Field.DoubleFieldValueRecord.Remove(doubleval);
                    break;
                case CBType.boolSetting:
                    var boolval = cell.Field.BoolFieldValueRecord.Where(v => v.Id == cell.ValueId).FirstOrDefault();
                    cell.Field.BoolFieldValueRecord.Remove(boolval);
                    break;
                case CBType.datetimeSetting:
                    var datetimeval = cell.Field.DateTimeFieldValueRecord.Where(v => v.Id == cell.ValueId).FirstOrDefault();
                    cell.Field.DateTimeFieldValueRecord.Remove(datetimeval);
                    break;
                default:
                    var stringval = cell.Field.StringFieldValueRecord.Where(v => v.Id == cell.ValueId).FirstOrDefault();
                    cell.Field.StringFieldValueRecord.Remove(stringval);
                    break;
            }

            _cellsRepository.Delete(cell);
        }
        private RowRecord GetLastNewRow(int dataTableId)
        {
            var rows = from row in _appRowsRepository.Table where row.IsNew == true && row.ApplicationDataTable.Id == dataTableId select row;
            var newRow = rows.FirstOrDefault();
            if (newRow != null)
                return newRow.Row;

            return null;
        }
        private void CommitRow(ApplicationDataTableRecord table, RowRecord row)
        {
            var rows = from r in _appRowsRepository.Table where r.Row.Id == row.Id && r.ApplicationDataTable.Id == table.Id select r;
            var rowtocommit = rows.FirstOrDefault();
            if (rowtocommit != null)
                rowtocommit.IsNew = false;
        }
        public RowRecord GetRow(int RowId)
        {
            try
            {
                return _rowsRepository.Get(RowId);
            }
            catch
            {
                return null;
            }
        }

        public CellRecord GetCell(RowRecord row, FieldRecord col)
        {
            try
            {
                return _cellsRepository.Get(x => x.Row.Id == row.Id && x.Field.Id == col.Id);
            }
            catch
            {
                return null;
            }
        }
        public void CreateDefaultsForRow(RowRecord row)
        {
            ApplicationDataTableRecord table = GetDataTable(row);
            if (table == null) return;
            var fields = GetFieldsForDataTable(table.Id);
            if (fields == null || fields.Count() < 1) return;
            foreach (var field in fields)
            {
                CreateDefaultForCell(row, field);
            }
        }
        public void SetValuesForRow(RowRecord row, string jsonValues)
        {
            ApplicationDataTableRecord table = GetDataTable(row);
            if (table == null) return;
            var fields = GetFieldsForDataTable(table.Id);
            if (fields == null || fields.Count() < 1) return;
            dynamic values = JObject.Parse(jsonValues);

            foreach (var field in fields)
            {
                SetValueCell(row, field, values[field.Name]);
            }
            CommitRow(table, row);
        }
        public void SetValuesForRow(int dataTableId, RowRecord row, string jsonValues)
        {
            ApplicationDataTableRecord table = GetDataTable(dataTableId);
            if (table == null) return;
            var fields = GetFieldsForDataTable(table.Id);
            if (fields == null || fields.Count() < 1) return;
            dynamic values = JObject.Parse(jsonValues);

            foreach (var field in fields)
            {
                SetValueCell(row, field, values[field.Name]);
            }
            CommitRow(table, row);
        }
        public void SetValuesForRow(int dataTableId, RowRecord row, IEnumerable<FieldRecord> fields, string jsonValues)
        {
            ApplicationDataTableRecord table = GetDataTable(dataTableId);
            if (table == null) return;
            if (fields == null || fields.Count() < 1) return;
            dynamic values = JObject.Parse(jsonValues);

            foreach (var field in fields)
            {
                SetValueCell(row, field, values[field.Name]);
            }
            CommitRow(table, row);
        }
        public void SetValuesForRow(ApplicationDataTableRecord table, RowRecord row, IEnumerable<FieldRecord> fields, string jsonValues)
        {
            if (table == null) return;
            if (fields == null || fields.Count() < 1) return;
            dynamic values = JObject.Parse(jsonValues);

            foreach (var field in fields)
            {
                SetValueCell(row, field, values[field.Name]);
            }
            CommitRow(table, row);
        }
        public void CreateDefaultForCell(RowRecord row, FieldRecord col)
        {
            SetValueCell(row, col, null);
        }
        public void SetValueCell(RowRecord row, FieldRecord col, dynamic value)
        {
            if (row == null) return;
            if (col == null) return;

            var cell = GetCell(row, col);
            if (cell == null)
            {
                cell = new CellRecord() { Row = row, Field = col };
                row.Cells.Add(cell);
            }
            FieldValueRecord fieldvalue = null;
            CBType p = CBDataTypes.TypeFromString(col.FieldType.ToLower());

            if (value != null)
            {
                switch (p)
                {
                    case CBType.intSetting:
                        int valueInteger = value;
                        fieldvalue = SetValueCell(cell, valueInteger);
                        break;
                    case CBType.doubleSetting:
                        double valueDouble = value;
                        fieldvalue = SetValueCell(cell, valueDouble);
                        break;
                    case CBType.boolSetting:
                        bool valueBool = value;
                        fieldvalue = SetValueCell(cell, valueBool);
                        break;
                    case CBType.datetimeSetting:
                        DateTime valueDateTime = value;
                        fieldvalue = SetValueCell(cell, valueDateTime);
                        break;
                    default:
                        string valueString = value;
                        fieldvalue = SetValueCell(cell, valueString);
                        break;
                }
            }
            else
            {


                switch (p)
                {
                    case CBType.intSetting:
                        fieldvalue = SetValueCellNullInteger(cell);
                        break;
                    case CBType.doubleSetting:
                        fieldvalue = SetValueCellNullDouble(cell);
                        break;
                    case CBType.boolSetting:
                        fieldvalue = SetValueCellNullBool(cell);
                        break;
                    case CBType.datetimeSetting:
                        fieldvalue = SetValueCellNullDateTime(cell);
                        break;
                    default:
                        fieldvalue = SetValueCellNullString(cell);
                        break;
                }
            }

            cell.ValueId = fieldvalue.Id;

        }

        public FieldValueRecord SetValueCell(CellRecord cell, string value)
        {
            if (cell.ValueId > 0)
            {
                var r = cell.Field.StringFieldValueRecord.Where(v => v.Id == cell.ValueId).First();
                r.Value = value;
                return r;
            }
            StringFieldValueRecord fieldvaluerecord = new StringFieldValueRecord();
            fieldvaluerecord.Value = value;
            cell.Field.StringFieldValueRecord.Add(fieldvaluerecord);
            _valuesStringRepository.Flush();
            return fieldvaluerecord;
        }
        public FieldValueRecord SetValueCellNullString(CellRecord cell)
        {
            if (cell.ValueId > 0)
            {
                var r = cell.Field.StringFieldValueRecord.Where(v => v.Id == cell.ValueId).First();
                r.Value = null;
                return r;
            }
            StringFieldValueRecord fieldvaluerecord = new StringFieldValueRecord();
            fieldvaluerecord.Value = null;
            cell.Field.StringFieldValueRecord.Add(fieldvaluerecord);
            _valuesStringRepository.Flush();
            return fieldvaluerecord;
        }
        public FieldValueRecord SetValueCell(CellRecord cell, int value)
        {
            if (cell.ValueId > 0)
            {
                var r = cell.Field.IntegerFieldValueRecord.Where(v => v.Id == cell.ValueId).First();
                r.Value = value;
                return r;
            }
            IntegerFieldValueRecord fieldvaluerecord = new IntegerFieldValueRecord();
            fieldvaluerecord.Value = value;
            cell.Field.IntegerFieldValueRecord.Add(fieldvaluerecord);
            _valuesIntRepository.Flush();
            return fieldvaluerecord;
        }
        public FieldValueRecord SetValueCellNullInteger(CellRecord cell)
        {
            if (cell.ValueId > 0)
            {
                var r = cell.Field.IntegerFieldValueRecord.Where(v => v.Id == cell.ValueId).First();
                r.Value = null;
                return r;
            }
            IntegerFieldValueRecord fieldvaluerecord = new IntegerFieldValueRecord();
            fieldvaluerecord.Value = null;
            cell.Field.IntegerFieldValueRecord.Add(fieldvaluerecord);
            _valuesIntRepository.Flush();
            return fieldvaluerecord;
        }
        public FieldValueRecord SetValueCell(CellRecord cell, double value)
        {
            if (cell.ValueId > 0)
            {
                var r = cell.Field.DoubleFieldValueRecord.Where(v => v.Id == cell.ValueId).First();
                r.Value = value;
                return r;
            }
            DoubleFieldValueRecord fieldvaluerecord = new DoubleFieldValueRecord();
            fieldvaluerecord.Value = value;
            cell.Field.DoubleFieldValueRecord.Add(fieldvaluerecord);
            _valuesDoubleRepository.Flush();
            return fieldvaluerecord;
        }
        public FieldValueRecord SetValueCellNullDouble(CellRecord cell)
        {
            if (cell.ValueId > 0)
            {
                var r = cell.Field.DoubleFieldValueRecord.Where(v => v.Id == cell.ValueId).First();
                r.Value = null;
                return r;
            }
            DoubleFieldValueRecord fieldvaluerecord = new DoubleFieldValueRecord();
            fieldvaluerecord.Value = null;
            cell.Field.DoubleFieldValueRecord.Add(fieldvaluerecord);
            _valuesDoubleRepository.Flush();
            return fieldvaluerecord;
        }
        public FieldValueRecord SetValueCell(CellRecord cell, bool value)
        {
            if (cell.ValueId > 0)
            {
                var r = cell.Field.BoolFieldValueRecord.Where(v => v.Id == cell.ValueId).First();
                r.Value = value;
                return r;
            }

            BoolFieldValueRecord fieldvaluerecord = new BoolFieldValueRecord();
            fieldvaluerecord.Value = value;
            cell.Field.BoolFieldValueRecord.Add(fieldvaluerecord);
            _valuesBoolRepository.Flush();
            return fieldvaluerecord;
        }
        public FieldValueRecord SetValueCellNullBool(CellRecord cell)
        {
            if (cell.ValueId > 0)
            {
                var r = cell.Field.BoolFieldValueRecord.Where(v => v.Id == cell.ValueId).First();
                r.Value = null;
                return r;
            }
            BoolFieldValueRecord fieldvaluerecord = new BoolFieldValueRecord();
            fieldvaluerecord.Value = null;
            cell.Field.BoolFieldValueRecord.Add(fieldvaluerecord);
            _valuesBoolRepository.Flush();
            return fieldvaluerecord;
        }
        public FieldValueRecord SetValueCell(CellRecord cell, DateTime value)
        {
            if (cell.ValueId > 0)
            {
                var r = cell.Field.DateTimeFieldValueRecord.Where(v => v.Id == cell.ValueId).First();
                r.Value = value;
                return r;
            }
            DateTimeFieldValueRecord fieldvaluerecord = new DateTimeFieldValueRecord();
            fieldvaluerecord.Value = value;
            cell.Field.DateTimeFieldValueRecord.Add(fieldvaluerecord);
            _valuesDateTimeRepository.Flush();
            return fieldvaluerecord;
        }
        public FieldValueRecord SetValueCellNullDateTime(CellRecord cell)
        {
            if (cell.ValueId > 0)
            {
                var r = cell.Field.DateTimeFieldValueRecord.Where(v => v.Id == cell.ValueId).First();
                r.Value = null;
                return r;
            }
            DateTimeFieldValueRecord fieldvaluerecord = new DateTimeFieldValueRecord();
            fieldvaluerecord.Value = null;
            cell.Field.DateTimeFieldValueRecord.Add(fieldvaluerecord);
            _valuesDateTimeRepository.Flush();
            return fieldvaluerecord;
        }
        #endregion

    }
}