namespace NetBlog.Web.Services
{
    public interface IImagesService
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
