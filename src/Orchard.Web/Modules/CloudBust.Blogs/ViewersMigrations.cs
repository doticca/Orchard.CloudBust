using CloudBust.Blogs.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs
{
    [OrchardFeature("CloudBust.Blogs.Stats")]
    public class ViewersMigrations : DataMigrationImpl
    {
        
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition("ViewersPart", builder => builder.Attachable());
            return 1;
        }
        public int UpdateFrom1()
        {
            // attach viewers part to Blogs
            ContentDefinitionManager.AlterTypeDefinition("BlogPost",
            cfg => cfg
                .WithPart(typeof(ViewersPart).Name)
                    .WithSetting("ViewersTypePartSettings.AllowAnonymousRatings", "True")
                    .WithSetting("ViewersTypePartSettings.ShowVoter", "True")
            );
            return 2;
        }
        public int UpdateFrom2()
        {
            SchemaBuilder.CreateTable("PopularBlogPostsPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<int>("BlogId")
                );

            ContentDefinitionManager.AlterPartDefinition("PopularBlogPostsPart",
                part => part
                    .WithDescription("Renders a list of the most popular blog posts."));

            ContentDefinitionManager.AlterTypeDefinition("PopularBlogPosts",
                cfg => cfg
                    .WithPart("PopularBlogPostsPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 3;
        }
    }
}