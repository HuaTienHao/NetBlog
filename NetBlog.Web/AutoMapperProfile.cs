using AutoMapper;
using NetBlog.Web.Models.Domain;
using NetBlog.Web.Models.ViewModels;

namespace NetBlog.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Tag
            CreateMap<Tag, AddTagRequest>().ReverseMap();
            CreateMap<Tag, EditTagRequest>();
            CreateMap<EditTagRequest, Tag>().ReverseMap();

            // BlogPost
            CreateMap<AddBlogPostRequest, BlogPost>().ForMember(dest => dest.Tags, src => src.Ignore());
        }
    }
}
