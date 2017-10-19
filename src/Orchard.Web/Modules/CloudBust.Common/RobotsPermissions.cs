using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using Orchard.Environment.Extensions;

namespace CloudBust.Common {

    [OrchardFeature("CloudBust.Common.SEO")]
    public class RobotsPermissions : IPermissionProvider {
		public static readonly Permission ConfigureRobotsTextFile = new Permission { Description = "Configure Robots.txt", Name = "ConfigureRobotsTextFile" };

		public virtual Feature Feature { get; set; }

		public IEnumerable<Permission> GetPermissions() {
			return new[] { ConfigureRobotsTextFile };
		}

		public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
			return new[] { new PermissionStereotype { Name = "Administrator", Permissions = new[] { ConfigureRobotsTextFile } } };
		}
	}
}