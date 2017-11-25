using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Localization;
using Orchard.Core.Contents.Extensions;
using CloudBust.Blogs.Models;

namespace CloudBust.Blogs
{
    public class BlogsMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition("SharePart", builder => builder.Attachable());
            return 1;
        }
        public int UpdateFrom1()
        {
            // attach viewers part to Blogs
            ContentDefinitionManager.AlterTypeDefinition("BlogPost",
            cfg => cfg
                .WithPart(typeof(SharePart).Name)
                    .WithSetting("ShareTypePartSettings.ShowFacebook", "True")
                    .WithSetting("ShareTypePartSettings.ShowTwitter", "True")
                    .WithSetting("ShareTypePartSettings.ShowMail", "True")
            );
            return 2;
        }
    }
}