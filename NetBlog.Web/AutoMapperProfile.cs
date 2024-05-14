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
            CreateMap<Tag, EditTagRequest>().ReverseMap();
            //CreateMap<EditTagRequest, Tag>().ReverseMap();

            // BlogPost
            CreateMap<AddBlogPostRequest, BlogPost>().ForMember(dest => dest.Tags, src => src.Ignore());
            CreateMap<BlogPost, EditBlogPostRequest>().ForMember(dest => dest.Tags, src => src.Ignore());
            CreateMap<EditBlogPostRequest, BlogPost>().ForMember(dest => dest.Tags, src => src.Ignore());
            CreateMap<BlogPost, BlogPost>();
            CreateMap<BlogPost, BlogDetailViewModel>().ForMember(dest => dest.Comments, src => src.Ignore());

            // BlogPostLike
            CreateMap<AddLikeRequest, BlogPostLike>();
        }
    }
}
