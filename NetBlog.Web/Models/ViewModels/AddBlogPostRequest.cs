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
        [NotGreaterThanToday(ErrorMessage = "Date must not be greater than today.")]
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        [Required]
        public string Author { get; set; }
        public bool Visible { get; set; }  
        public IEnumerable<SelectListItem> Tags { get; set; } = Enumerable.Empty<SelectListItem>();
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }

    public class NotGreaterThanTodayAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                // Assuming null is not allowed. You can adjust this logic if null is valid.
                return false;
            }

            DateTime dateToCheck;
            if (DateTime.TryParse(value.ToString(), out dateToCheck))
            {
                return dateToCheck.Date <= DateTime.Now.Date;
            }

            return false;
        }
    }
}
