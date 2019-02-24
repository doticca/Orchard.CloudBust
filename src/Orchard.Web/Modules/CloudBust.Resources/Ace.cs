using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    [OrchardFeature("CloudBust.Resources.Ace")]
    public class Ace : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("Ace_Base")
                .SetUrl("https://cloudbust.blob.core.windows.net/public/js/ace171017/ace.js", "ace171017/ace.js")
                .SetDependencies("jQuery")
                .SetVersion("1.2.9");

            manifest.DefineScript("Ace_Emmet").SetUrl("https://cloudbust.blob.core.windows.net/public/js/ace171017/ext-emmet.js", "ace171017/ext-emmet.js").SetDependencies("Emmet")
                .SetVersion("1.2.9");
            manifest.DefineScript("Ace_Elt").SetUrl("https://cloudbust.blob.core.windows.net/public/js/ace171017/ext-language_tools.js", "ace171017/ext-language_tools.js")
                .SetVersion("1.2.9");
            manifest.DefineScript("Ace_Spellcheck").SetUrl("https://cloudbust.blob.core.windows.net/public/js/ace171017/ext-spellcheck.js", "ace171017/ext-spellcheck.js")
                .SetVersion("1.2.9");
            manifest.DefineScript("Ace_Whitespace").SetUrl("https://cloudbust.blob.core.windows.net/public/js/ace171017/ext-whitespace.js", "ace171017/ext-whitespace.js")
                .SetVersion("1.2.9");
            manifest.DefineScript("Ace_Tabstops").SetUrl("https://cloudbust.blob.core.windows.net/public/js/ace171017/ext-elastic_tabstops_lite.js", "ace171017/ext-elastic_tabstops_lite.js")
                .SetVersion("1.2.9");


            manifest.DefineScript("Ace")
                .SetDependencies("Ace_Base", "Ace_Spellcheck", "Ace_Emmet", "Ace_Elt", "Ace_Whitespace", "Ace_Tabstops")
                .SetVersion("1.2.9");

            manifest.DefineScript("OrchardAce").SetUrl("https://cloudbust.blob.core.windows.net/public/js/orchard-ace.min.js", "orchard-ace.js").SetDependencies("Ace");
        }
    }
}
