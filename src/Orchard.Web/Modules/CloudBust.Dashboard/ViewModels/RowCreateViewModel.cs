using Orchard.Security;
using CloudBust.Application.Models;
using CloudBust.Dashboard.Models;
using System.Collections.Generic;

namespace CloudBust.Dashboard.ViewModels
{
    public class RowCreateViewModel  {
        public IUser User { get; set; }
        public string ApplicationDataTableName { get; set; }
        public string ApplicationDataTableID { get; set; }
        public RowRecord Row { get; set; }
        public IEnumerable<FieldRecord> Fields { get; set; }
        public int RowID { get; set; }
        public string jsonResults { get; set; }
    }    
}
