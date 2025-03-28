using CloudinaryDotNet.Actions;

namespace NL_THUD.Services.ServiceImpl
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> uploadImageAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
