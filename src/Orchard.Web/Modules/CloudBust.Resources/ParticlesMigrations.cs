using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace CloudBust.Resources
{
    [OrchardFeature("CloudBust.Resources.Particles")]
    public class ParticlesMigrations : DataMigrationImpl {    
        public int Create()
        {
            SchemaBuilder.CreateTable(
                "ParticlesSettingsPartRecord",
                table => table
                             .ContentPartRecord()
                             .Column<string>("JsonUrl", c => c.WithDefault("particles.json"))
                             .Column<bool>("AutoEnable", c => c.WithDefault(true))
                             .Column<bool>("AutoEnableAdmin", c => c.WithDefault(false))
                );
            return 1;
        }
        public int UpdateFrom1()
        {
            SchemaBuilder.CreateTable("ParticlesPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("JsonUrl", c => c.WithDefault("particles.json"))
                );

            ContentDefinitionManager.AlterPartDefinition("ParticlesPart",
                builder => builder
                    .Attachable()
                    .WithDescription("Creates a canvas to display particles.js.")
                );

            ContentDefinitionManager.AlterTypeDefinition("ParticlesWidget",
                cfg => cfg
                    .WithPart("ParticlesPart")
                    .WithPart("CommonPart")
                    .WithPart("BodyPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 2;
        }

    }
}