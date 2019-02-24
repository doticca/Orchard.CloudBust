using System.Collections.Generic;
using Orchard.Environment.Configuration;

namespace CloudBust.Tenants.ViewModels {
    public class TenantsIndexViewModel  {
        public IEnumerable<ShellSettings> TenantSettings { get; set; }
    }
}
