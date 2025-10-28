namespace Ecommerce_API.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<(string Url, string PublicId)> UploadImageAsync(IFormFile file);
        Task<bool> DeleteImageAsync(string publicId);

    }

}
