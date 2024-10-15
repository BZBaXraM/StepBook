using CloudinaryDotNet.Actions;

namespace Account.API.Services;

public interface IPhotoService
{
    /// <returns></returns>
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

    /// <summary>
    /// Delete a photo from the cloudinary service.
    /// </summary>
    /// <param name="publicId"></param>
    /// <returns></returns>
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}