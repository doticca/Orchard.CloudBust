using System.Linq;
using Orchard.Blogs.Models;
using Orchard.Blogs.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment.Extensions;
using CloudBust.Blogs.Models;
using CloudBust.Blogs.ViewModels;
using Contrib.Voting.Services;
using System.Collections.Generic;
using Orchard.Taxonomies.Services;
using Orchard.Taxonomies.Models;

namespace CloudBust.Blogs.Drivers
{
    [OrchardFeature("CloudBust.Blogs.Stats")]
    public class PopularBlogPostsPartDriver : ContentPartDriver<PopularBlogPostsPart> {
        private readonly IBlogService _blogService;
        private readonly IBlogPostService _blogpostService;
        private readonly IContentManager _contentManager;
        private readonly IVotingService _votingService;
        private readonly ITaxonomyService _taxonomyService;

        public PopularBlogPostsPartDriver(
            IBlogService blogService,
            IBlogPostService blogpostService,
            IVotingService votingService,
            IContentManager contentManager,
            ITaxonomyService taxonomyService) {
            _blogService = blogService;
            _blogpostService = blogpostService;
            _votingService = votingService;
            _contentManager = contentManager;
            _taxonomyService = taxonomyService;
        }

        protected override DriverResult Display(PopularBlogPostsPart part, string displayType, dynamic shapeHelper)
        {
            var result = makeResult(part, displayType, shapeHelper);

            return ContentShape("Parts_Blogs_PopularBlogPosts", () => {
                return result;
            });
        }

        private Shape makeResult(PopularBlogPostsPart part, string displayType, dynamic shapeHelper){

            var blog = _contentManager.Get<BlogPart>(part.BlogId);

            if (blog == null)
            {
                return null;
            }


            int ammount = 8;

            var voteRecords = _votingService.GetResults(ammount, "sum", Constants.Dimension_View);
            int[] voteIds = voteRecords.Select(s => s.ContentItemRecord.Id).ToArray();

            IList<BlogPostPart> blogPosts = new List<BlogPostPart>();
            IList<TermPart> blogTerms = new List<TermPart>();

            foreach(int id in voteIds)
            {
               var p = _blogpostService.Get(id);
                blogPosts.Add(p);

                var terms = _taxonomyService.GetTermsForContentItem(p.Id);
                foreach(var term in terms)
                {
                    if (!blogTerms.Any(x => x.Id == term.Id))
                        blogTerms.Add(term);
                }
            }

            var list = shapeHelper.List();
            list.AddRange(blogPosts.Select(bp => _contentManager.BuildDisplay(bp, "Summary")));

            var blogPostList = shapeHelper.Parts_Blogs_BlogPost_List(ContentItems: list);

            return shapeHelper.Parts_Blogs_PopularBlogPosts(ContentItems: blogPostList, Blog: blog, Terms: blogTerms);
        }

        protected override DriverResult Editor(PopularBlogPostsPart part, dynamic shapeHelper)
        {
            var viewModel = new PopularBlogPostsViewModel {
                BlogId = part.BlogId,
                Blogs = _blogService.Get().ToList().OrderBy(b => _contentManager.GetItemMetadata(b).DisplayText)
            };

            return ContentShape("Parts_Blogs_PopularBlogPosts_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts.Blogs.PopularBlogPosts", Model: viewModel, Prefix: Prefix));
        }

        protected override DriverResult Editor(PopularBlogPostsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new PopularBlogPostsViewModel();

            if (updater.TryUpdateModel(viewModel, Prefix, null, null)) {
                part.BlogId = viewModel.BlogId;
            }

            return Editor(part, shapeHelper);
        }

        protected override void Importing(PopularBlogPostsPart part, ImportContentContext context)
        {
            var blog = context.Attribute(part.PartDefinition.Name, "Blog");
            if (blog != null) {
                part.BlogId = context.GetItemFromSession(blog).Id;
            }
        }

        protected override void Exporting(PopularBlogPostsPart part, ExportContentContext context)
        {
            var blog = _contentManager.Get(part.BlogId);
            var blogIdentity = _contentManager.GetItemMetadata(blog).Identity;
            context.Element(part.PartDefinition.Name).SetAttributeValue("Blog", blogIdentity);
        } 
    }
}