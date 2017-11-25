using CloudBust.Application.Models;
using Orchard;
using Orchard.Blogs.Models;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBust.Blogs.Services
{
    public interface IBlogsService : IDependency
    {
        IEnumerable<BlogPart> GetBlogsForUserInApplication(ApplicationRecord app, IUser user);
        IEnumerable<BlogPart> GetBlogsForUserInApplication(IUser user);
    }
}