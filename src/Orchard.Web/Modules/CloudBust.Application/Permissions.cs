using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace CloudBust.Application {
    public class Permissions : IPermissionProvider {
        public static readonly Permission ManageApps = new Permission { Description = "Manage apps for others", Name = "ManageApps" };
        public static readonly Permission ManageOwnApps = new Permission { Description = "Manage own blogs", Name = "ManageOwnApps", ImpliedBy = new[] { ManageApps } };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                ManageOwnApps,
                ManageApps
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] { ManageApps }
                },
                new PermissionStereotype {
                    Name = "Author",
                    Permissions = new[] { ManageOwnApps }
                }
            };
        }

    }
}


