using System.Linq;
using CloudBust.Common.Services;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace CloudBust.Common.Shapes {
    [OrchardFeature("CloudBust.Common.FavIcon")]
    public class FaviconShapes : IShapeTableProvider {
        private readonly IFaviconService _faviconService;
        private readonly IWorkContextAccessor _wca;

        public FaviconShapes(IWorkContextAccessor wca, IFaviconService faviconService) {
            _wca = wca;
            _faviconService = faviconService;
        }

        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Metas")
                   .OnDisplaying(shapeDisplayingContext => {
                        var msApplicationConfigUrl = _faviconService.GetMSApplicationConfigUrl();
                        var themeColor = _faviconService.GetThemeColor();

                        var runthisshape = !string.IsNullOrWhiteSpace(msApplicationConfigUrl) ||
                            !string.IsNullOrWhiteSpace(themeColor);
                        if (runthisshape) {
                            var resourceManager = _wca.GetContext().Resolve<IResourceManager>();
                            var metas = resourceManager.GetRegisteredMetas();

                            // msApplicationConfigUrl
                            if (!string.IsNullOrWhiteSpace(msApplicationConfigUrl)) {
                                var entryLink = metas.FirstOrDefault(l => l.Name == "msapplication-config");

                                if (entryLink != default(MetaEntry)) {
                                    entryLink.Content = msApplicationConfigUrl;
                                }
                                else {
                                    resourceManager.SetMeta(new MetaEntry
                                        {Name = "msapplication-config", Content = msApplicationConfigUrl});
                                }
                            }

                            // themeColor
                            if (!string.IsNullOrWhiteSpace(themeColor)) {
                                var entryLink = metas.FirstOrDefault(l => l.Name == "theme-color");

                                if (entryLink != default(MetaEntry)) {
                                    entryLink.Content = themeColor;
                                }
                                else {
                                    resourceManager.SetMeta(new MetaEntry {Name = "theme-color", Content = themeColor});
                                }
                            }
                        }
                    });

            builder.Describe("HeadLinks")
                   .OnDisplaying(shapeDisplayingContext => {
                        var faviconUrl = _faviconService.GetFaviconUrl();
                        var androidManifestUrl = _faviconService.GetAndroidManifestUrl();
                        var appleTouchIconUrl = _faviconService.GetAppleTouchIconUrl();
                        var pngImageUrl = _faviconService.GetPngImageUrl();
                        var safariPinnedMask = _faviconService.GetSafariPinnedMask();
                        var safariPinnedUrl = _faviconService.GetSafariPinnedUrl();


                        var runthisshape = !string.IsNullOrWhiteSpace(faviconUrl) ||
                            !string.IsNullOrWhiteSpace(androidManifestUrl) ||
                            !string.IsNullOrWhiteSpace(appleTouchIconUrl) ||
                            !string.IsNullOrWhiteSpace(pngImageUrl) ||
                            !string.IsNullOrWhiteSpace(safariPinnedMask) ||
                            !string.IsNullOrWhiteSpace(safariPinnedUrl);


                        if (runthisshape) {
                            var resourceManager = _wca.GetContext().Resolve<IResourceManager>();
                            var links = resourceManager.GetRegisteredLinks();

                            // faviconUrl
                            if (!string.IsNullOrWhiteSpace(faviconUrl)) {
                                var entryLink = links.FirstOrDefault(l => l.Rel == "shortcut icon");
                                if (entryLink != default(LinkEntry)) {
                                    entryLink.Href = faviconUrl;
                                }
                                else {
                                    resourceManager.RegisterLink(new LinkEntry
                                        {Type = "", Rel = "shortcut icon", Href = faviconUrl});
                                }
                            }

                            // appleTouchIconUrl
                            if (!string.IsNullOrWhiteSpace(appleTouchIconUrl)) {
                                var entryLink = links.FirstOrDefault(l => l.Rel == "apple-touch-icon");

                                if (entryLink != default(LinkEntry)) {
                                    entryLink.Href = appleTouchIconUrl;
                                }
                                else {
                                    resourceManager.RegisterLink(new LinkEntry
                                        {Rel = "apple-touch-icon", Href = appleTouchIconUrl, Sizes = "152x152"});
                                }
                            }

                            // pngImageUrl
                            if (!string.IsNullOrWhiteSpace(pngImageUrl)) {
                                var entryLink = links.FirstOrDefault(l => l.Rel == "icon" && l.Type == "image/png");

                                if (entryLink != default(LinkEntry)) {
                                    entryLink.Href = pngImageUrl;
                                }
                                else {
                                    resourceManager.RegisterLink(new LinkEntry
                                        {Type = "image/png", Rel = "icon", Href = pngImageUrl, Sizes = "32x32"});
                                }
                            }

                            // androidManifestUrl
                            if (!string.IsNullOrWhiteSpace(appleTouchIconUrl)) {
                                var entryLink = links.FirstOrDefault(l => l.Rel == "manifest");

                                if (entryLink != default(LinkEntry)) {
                                    entryLink.Href = androidManifestUrl;
                                }
                                else {
                                    resourceManager.RegisterLink(new LinkEntry
                                        {Rel = "manifest", Href = androidManifestUrl});
                                }
                            }

                            // safariPinnedUrl
                            if (!string.IsNullOrWhiteSpace(safariPinnedUrl)) {
                                var entryLink = links.FirstOrDefault(l => l.Rel == "mask-icon");

                                if (entryLink != default(LinkEntry)) {
                                    entryLink.Href = safariPinnedUrl;
                                    if (!string.IsNullOrWhiteSpace(safariPinnedMask)) {
                                        entryLink.SetAttribute("color", safariPinnedMask);
                                    }
                                }
                                else {
                                    resourceManager.RegisterLink(new LinkEntry
                                        {Rel = "mask-icon", Href = safariPinnedUrl});
                                    if (!string.IsNullOrWhiteSpace(safariPinnedMask)) {
                                        entryLink = links.FirstOrDefault(l => l.Rel == "mask-icon");
                                        entryLink.SetAttribute("color", safariPinnedMask);
                                    }
                                }
                            }
                        }
                    });
        }
    }
}