namespace NetBlog.Web.Services.Interfaces
{
    public interface IImagesService
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
