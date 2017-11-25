using CloudBust.Application.Models;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Dashboard.ViewModels
{
    public class TablesViewModel
    {
        public ApplicationRecord Application { get; set; }
        public IUser User { get; set; }
        public IList<ApplicationDataTableRecord> DataTables { get; set; }
    }
}