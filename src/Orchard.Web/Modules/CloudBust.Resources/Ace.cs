using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class Ace : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("Ace_Base")
                .SetUrl("ace171017/ace.js", "ace171017/ace.js")
                .SetDependencies("jQuery")
                .SetVersion("1.2.9");

            manifest.DefineScript("Ace_Emmet").SetUrl("ace171017/ext-emmet.js", "ace171017/ext-emmet.js").SetDependencies("Emmet")
                .SetVersion("1.2.9");
            manifest.DefineScript("Ace_Elt").SetUrl("ace171017/ext-language_tools.js", "ace171017/ext-language_tools.js")
                .SetVersion("1.2.9");
            manifest.DefineScript("Ace_Spellcheck").SetUrl("ace171017/ext-spellcheck.js", "ace171017/ext-spellcheck.js")
                .SetVersion("1.2.9");
            manifest.DefineScript("Ace_Whitespace").SetUrl("ace171017/ext-whitespace.js", "ace171017/ext-whitespace.js")
                .SetVersion("1.2.9");
            manifest.DefineScript("Ace_Tabstops").SetUrl("ace171017/ext-elastic_tabstops_lite.js", "ace171017/ext-elastic_tabstops_lite.js")
                .SetVersion("1.2.9");


            manifest.DefineScript("Ace")
                .SetDependencies("Ace_Base", "Ace_Spellcheck", "Ace_Emmet", "Ace_Elt", "Ace_Whitespace", "Ace_Tabstops")
                .SetVersion("1.2.9");

            manifest.DefineScript("OrchardAce").SetUrl("orchard-ace.js", "orchard-ace.js").SetDependencies("Ace");
        }
    }
}
