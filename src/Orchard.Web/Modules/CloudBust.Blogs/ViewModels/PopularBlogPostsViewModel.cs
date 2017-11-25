using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Orchard.Blogs.Models;

namespace CloudBust.Blogs.ViewModels {
    public class PopularBlogPostsViewModel {
       
        [Required]
        public int BlogId { get; set; }

        public IEnumerable<BlogPart> Blogs { get; set; }

    }
}