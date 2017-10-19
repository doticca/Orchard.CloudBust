using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class Ace : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("Ace_Base")
                .SetUrl("ace2810/ace.js", "ace2810/ace.js")
                .SetDependencies("jQuery")
                .SetVersion("28.10.13");

            manifest.DefineScript("Ace_Emmet").SetUrl("ace2810/ext-emmet.js", "ace2810/ext-emmet.js").SetDependencies("Emmet")
                .SetVersion("28.10.13");
            manifest.DefineScript("Ace_Elt").SetUrl("ace2810/ext-language_tools.js", "ace2810/ext-language_tools.js")
                .SetVersion("28.10.13");
            manifest.DefineScript("Ace_Spellcheck").SetUrl("ace2810/ext-spellcheck.js", "ace2810/ext-spellcheck.js")
                .SetVersion("28.10.13");
            manifest.DefineScript("Ace_Whitespace").SetUrl("ace2810/ext-whitespace.js", "ace2810/ext-whitespace.js")
                .SetVersion("28.10.13");
            manifest.DefineScript("Ace_Tabstops").SetUrl("ace2810/ext-elastic_tabstops_lite.js", "ace2810/ext-elastic_tabstops_lite.js")
                .SetVersion("28.10.13");


            manifest.DefineScript("Ace")
                .SetDependencies("Ace_Base", "Ace_Spellcheck", "Ace_Emmet", "Ace_Elt", "Ace_Whitespace", "Ace_Tabstops")
                .SetVersion("28.10.13");

            manifest.DefineScript("OrchardAce").SetUrl("orchard-ace.js", "orchard-ace.js").SetDependencies("Ace");
        }
    }
}
