﻿using System.ComponentModel.DataAnnotations;
using CloudBust.Tenants.Annotations;
using System.Collections.Generic;

namespace CloudBust.Tenants.ViewModels {
    public class TenantAddViewModel  {
        public TenantAddViewModel() {
            // define "Allow the tenant to set up the database" as default value 
            DataProvider = "";
            Themes = new List<ThemeEntry>();
            Modules = new List<ModuleEntry>();
        }

        [Required]
        public string Name { get; set; }
        public string RequestUrlHost { get; set; }
        public string RequestUrlPrefix { get; set; }
        public string DataProvider { get; set; }
        [SqlDatabaseConnectionString]
        public string DatabaseConnectionString { get; set; }
        public string DatabaseTablePrefix { get; set; }

        public List<ThemeEntry> Themes { get; set; }
        public List<ModuleEntry> Modules { get; set; }
    }
}

