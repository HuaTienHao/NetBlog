using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NetBlog.Web.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        [Required]
        public string Heading { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        [Url]
        public string FeaturedImageUrl { get; set; }
        [Required]
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        [Required]
        public string Author { get; set; }
        public bool Visible { get; set; }  
        public IEnumerable<SelectListItem> Tags { get; set; } = Enumerable.Empty<SelectListItem>();
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
