using NetBlog.Web.Models.Domain;

namespace NetBlog.Web.Models.ViewModels
{
    public class ModalTagRequests
    {
        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
        public AddTagRequest AddTagRequest { get; set; }
    }
}
