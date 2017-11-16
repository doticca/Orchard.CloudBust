using System.Linq;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;
using Orchard.UI.Resources;
using CloudBust.Common.Services;

namespace CloudBust.Common.Shapes {
    [OrchardFeature("CloudBust.Common.FavIcon")]
    public class FaviconShapes : IShapeTableProvider {
        private readonly IWorkContextAccessor _wca;
        private readonly IFaviconService _faviconService;

        public FaviconShapes(IWorkContextAccessor wca, IFaviconService faviconService) {
            _wca = wca;
            _faviconService = faviconService;
        }

        public void Discover(ShapeTableBuilder builder) {

            builder.Describe("Metas")
                .OnDisplaying(shapeDisplayingContext =>
                {
                    string msApplicationConfigUrl = _faviconService.GetMSApplicationConfigUrl();
                    string themeColor = _faviconService.GetThemeColor();

                    bool runthisshape = !string.IsNullOrWhiteSpace(msApplicationConfigUrl) ||
                                        !string.IsNullOrWhiteSpace(themeColor);
                    if (runthisshape)
                    {
                        var resourceManager = _wca.GetContext().Resolve<IResourceManager>();
                        var metas = resourceManager.GetRegisteredMetas();

                        // msApplicationConfigUrl
                        if (!string.IsNullOrWhiteSpace(msApplicationConfigUrl))
                        {
                            var entryLink = metas.Where(l => l.Name == "msapplication-config").FirstOrDefault();

                            if (entryLink != default(MetaEntry))
                                entryLink.Content = msApplicationConfigUrl;
                            else
                                resourceManager.SetMeta(new MetaEntry { Name = "msapplication-config", Content = msApplicationConfigUrl });
                        }
                        // themeColor
                        if (!string.IsNullOrWhiteSpace(themeColor))
                        {
                            var entryLink = metas.Where(l => l.Name == "theme-color").FirstOrDefault();

                            if (entryLink != default(MetaEntry))
                                entryLink.Content = themeColor;
                            else
                                resourceManager.SetMeta(new MetaEntry { Name = "theme-color", Content = themeColor });
                        }
                    }
                });
            
            builder.Describe("HeadLinks")
                .OnDisplaying(shapeDisplayingContext => {
                    string faviconUrl = _faviconService.GetFaviconUrl();
                    string androidManifestUrl = _faviconService.GetAndroidManifestUrl();
                    string appleTouchIconUrl = _faviconService.GetAppleTouchIconUrl();                    
                    string pngImageUrl = _faviconService.GetPngImageUrl();
                    string safariPinnedMask = _faviconService.GetSafariPinnedMask();
                    string safariPinnedUrl = _faviconService.GetSafariPinnedUrl();


                    bool runthisshape = !string.IsNullOrWhiteSpace(faviconUrl) ||
                                        !string.IsNullOrWhiteSpace(androidManifestUrl) ||
                                        !string.IsNullOrWhiteSpace(appleTouchIconUrl) ||
                                        !string.IsNullOrWhiteSpace(pngImageUrl) ||
                                        !string.IsNullOrWhiteSpace(safariPinnedMask) ||
                                        !string.IsNullOrWhiteSpace(safariPinnedUrl);


                    if (runthisshape)
                    {
                        var resourceManager = _wca.GetContext().Resolve<IResourceManager>();
                        var links = resourceManager.GetRegisteredLinks();

                        // faviconUrl
                        if (!string.IsNullOrWhiteSpace(faviconUrl)) { 
                            var entryLink = links.Where(l => l.Rel == "shortcut icon").FirstOrDefault();
                            if (entryLink != default(LinkEntry))
                                entryLink.Href = faviconUrl;
                            else
                                resourceManager.RegisterLink(new LinkEntry { Type = "", Rel = "shortcut icon", Href = faviconUrl });
                        }
                        // appleTouchIconUrl
                        if (!string.IsNullOrWhiteSpace(appleTouchIconUrl))
                        {
                            var entryLink = links.Where(l => l.Rel == "apple-touch-icon").FirstOrDefault();

                            if (entryLink != default(LinkEntry))
                                entryLink.Href = appleTouchIconUrl;
                            else
                                resourceManager.RegisterLink(new LinkEntry { Rel = "apple-touch-icon", Href = appleTouchIconUrl, Sizes = "152x152" });
                        }
                        // pngImageUrl
                        if (!string.IsNullOrWhiteSpace(pngImageUrl))
                        {
                            var entryLink = links.Where(l => l.Rel == "icon" && l.Type == "image/png").FirstOrDefault();

                            if (entryLink != default(LinkEntry))
                                entryLink.Href = pngImageUrl;
                            else
                                resourceManager.RegisterLink(new LinkEntry { Type = "image/png", Rel = "icon", Href = pngImageUrl, Sizes = "32x32" });
                        }
                        // androidManifestUrl
                        if (!string.IsNullOrWhiteSpace(appleTouchIconUrl))
                        {
                            var entryLink = links.Where(l => l.Rel == "manifest").FirstOrDefault();

                            if (entryLink != default(LinkEntry))
                                entryLink.Href = androidManifestUrl;
                            else
                                resourceManager.RegisterLink(new LinkEntry { Rel = "manifest", Href = androidManifestUrl });
                        }
                        // safariPinnedUrl
                        if (!string.IsNullOrWhiteSpace(safariPinnedUrl))
                        {
                            var entryLink = links.Where(l => l.Rel == "mask-icon").FirstOrDefault();

                            if (entryLink != default(LinkEntry))
                            {
                                entryLink.Href = safariPinnedUrl;
                                if (!string.IsNullOrWhiteSpace(safariPinnedMask))
                                    entryLink.SetAttribute("color", safariPinnedMask);

                            }
                            else
                            {
                                resourceManager.RegisterLink(new LinkEntry { Rel = "mask-icon", Href = safariPinnedUrl });
                                if (!string.IsNullOrWhiteSpace(safariPinnedMask))
                                {
                                    entryLink = links.Where(l => l.Rel == "mask-icon").FirstOrDefault();
                                    entryLink.SetAttribute("color", safariPinnedMask);
                                }

                            }
                        }
                    }
                });
        }
    }
}