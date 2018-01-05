﻿using CloudBust.Application.Models;
using Orchard;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Application.Services
{

    public interface IDataTablesService : IDependency
    {
        // Tables
        ApplicationDataTableRecord GetDataTable(int Id);
        IEnumerable<ApplicationDataTableRecord> GetDataTables();
        IEnumerable<ApplicationDataTableRecord> GetApplicationDataTables(ApplicationRecord applicationRecord);
        IEnumerable<ApplicationDataTableRecord> GetApplicationDataTables(int applicationId);
        ApplicationDataTableRecord GetDataTableByName(string datatableName);
        ApplicationDataTableRecord CreateDataTable(string datatableName, string datatableDescription);
        bool UpdateDataTable(int id, string datatableName, string datatableDescription);
        bool CreateDataTableForApplication(string applicationName, int datatableId);
        bool RemoveDataTableFromApplication(string applicationName, int datatableId);
        IEnumerable<ApplicationDataTableRecord> GetNonApplicationDataTables(IUser user, ApplicationRecord applicationRecord);
        IEnumerable<ApplicationRecord> GetDataTableApplications(int dataTableId);
        IEnumerable<ApplicationRecord> GetDataTableApplications(ApplicationDataTableRecord applicationDataTableRecord);
        // Fields
        FieldRecord GetField(int Id);
        IEnumerable<FieldRecord> GetFieldsForDataTable(int Id);
        FieldRecord CreateField(string fieldName, string fieldDescription, string fieldType);
        bool UpdateField(int id, string Name, string Description, string FieldType);
        FieldRecord SetFieldType(int fieldId, string fieldType);
        bool DeleteField(FieldRecord fieldRecord);
        bool DeleteField(int fieldId);
        FieldRecord CreateFieldForApplicationDataTable(int dataTableId, int fieldId);
        bool RemoveFieldFromApplicationDataTable(int dataTableId, int fieldId);
        //dynamic GetFieldValue(FieldRecord fieldRecord);
        //dynamic GetFieldValue(int fieldId);
        //void SetFieldValue(int fieldId, string fieldValue);
        //void SetFieldValue(int fieldId, int fieldValue);
        //void SetFieldValue(int fieldId, double fieldValue);
        //void SetFieldValue(int fieldId, bool fieldValue);
        //void SetFieldValue(int fieldId, DateTime fieldValue);    
    }
}