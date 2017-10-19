using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using Orchard.Environment.Extensions;

namespace CloudBust.Common {

    [OrchardFeature("CloudBust.Common.SEO")]
    public class ApplinksPermissions : IPermissionProvider {
		public static readonly Permission ConfigureApplinksTextFile = new Permission { Description = "Configure apple-app-site-association", Name = "ConfigureApplinksTextFile" };

		public virtual Feature Feature { get; set; }

		public IEnumerable<Permission> GetPermissions() {
			return new[] { ConfigureApplinksTextFile };
		}

		public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
			return new[] { new PermissionStereotype { Name = "Administrator", Permissions = new[] { ConfigureApplinksTextFile } } };
		}
	}
}