using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Resources {
    public class DefaultManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineStyle("Animate")
                .SetUrl("animate.min.css", "animate.css")
                .SetVersion("1.0.0");

            manifest.DefineStyle("jQuery_Scrollbar")
                .SetUrl("jquery.scrollbar.css", "jquery.scrollbar.css")
                .SetVersion("0.2.10");

            manifest.DefineStyle("jQuery_Slicknav")
                .SetUrl("slicknav.min.css", "slicknav.css")
                .SetVersion("1.0.10");

            manifest.DefineStyle("Elevator")
                .SetUrl("jquery.elevator.css", "jquery.elevator.css")
                .SetVersion("1.0.6");

            manifest.DefineScript("jQuery_Elevator")
                .SetUrl("jquery.elevator.min.js", "jquery.elevator.js")
                .SetDependencies("jQuery")
                .SetVersion("1.0.6");

            manifest.DefineScript("jQuery_QuickFit")
                .SetUrl("jquery.quickfit.js", "jquery.quickfit.js")
                .SetDependencies("jQuery")
                .SetVersion("1.0.0");

            manifest.DefineScript("jQuery_FitText")
                .SetUrl("jquery.fittext.js", "jquery.fittext.js")
                .SetDependencies("jQuery")
                .SetVersion("1.2.0");

            manifest.DefineScript("jQuery_BoxFit")
                .SetUrl("https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/js/jquery.boxfit.min.js", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/js/jquery.boxfit.js")
                .SetDependencies("jQuery")
                .SetVersion("1.2.4");

            manifest.DefineScript("Respond")
                .SetUrl("respond.min.js")
                .SetVersion("1.4.2");

            manifest.DefineScript("BigText")
                .SetUrl("bigtext.js")
                .SetDependencies("jQuery")
                .SetVersion("0.1.8");

            manifest.DefineScript("Isotope")
                .SetUrl("isotope.pkgd.min.js")
                .SetVersion("3.0.1");

            manifest.DefineScript("jQuery_Easing")
                .SetUrl("jquery.easing.min.js")               
                .SetDependencies("jQuery")
                .SetVersion("1.3.0");

            manifest.DefineScript("jQuery_Animate")
                .SetUrl("jquery.animate.min.js", "jquery.animate.js")
                .SetDependencies("jQuery")
                .SetVersion("1.11.0");

            manifest.DefineScript("jQuery_AnimateNumber")
                .SetUrl("jquery.animateNumber.min.js", "jquery.animateNumber.min.js")
                .SetDependencies("jQuery")
                .SetVersion("0.0.10");

            manifest.DefineScript("jQuery_Appear")
                .SetUrl("jquery.appear.min.js", "jquery.appear.js")
                .SetDependencies("jQuery")
                .SetVersion("1.0.0");

            manifest.DefineScript("jQuery_Superslides")
                .SetUrl("jquery.superslides.min.js", "jquery.superslides.js")
                .SetDependencies("jQuery", "jQuery_Animate")
                .SetVersion("0.6.3");

            manifest.DefineScript("jQuery_Quicksand")
                .SetUrl("jquery.quicksand.js", "jquery.quicksand.js")
                .SetDependencies("jQuery")
                .SetVersion("1.4.0");

            manifest.DefineScript("wow")
                .SetUrl("wow.min.js", "wow.js")
                .SetVersion("1.1.3");

            manifest.DefineScript("jQuery_NoHash")
                .SetUrl("jquery.nohash.js", "jquery.nohash.js")
                .SetDependencies("jQuery")
                .SetVersion("1.0.0");

            manifest.DefineScript("jQuery_Pseudo")
                .SetUrl("https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/js/jquery.pseudo.min.js", "https://doticcacdn.blob.core.windows.net/public/cloudbust/cloudbust.resources/js/jquery.pseudo.js")
                .SetDependencies("jQuery")
                .SetVersion("0.0.1");

            manifest.DefineScript("jQuery_Scrollbar")
                .SetUrl("jquery.scrollbar.min.js", "jquery.scrollbar.js")
                .SetDependencies("jQuery")
                .SetVersion("0.2.10");

            manifest.DefineScript("jQuery_Slicknav")
                .SetUrl("jquery.slicknav.min.js", "jquery.slicknav.js")
                .SetDependencies("jQuery")
                .SetVersion("1.0.10");
        }
    }
}
