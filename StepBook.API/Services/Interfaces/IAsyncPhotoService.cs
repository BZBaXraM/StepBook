namespace StepBook.API.Services.Interfaces;

/// <summary>
/// Interface for the photo service.
/// </summary>
public interface IAsyncPhotoService
{
    /// <summary>
    /// Add a photo to the cloudinary service.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

    /// <summary>
    /// Delete a photo from the cloudinary service.
    /// </summary>
    /// <param name="publicId"></param>
    /// <returns></returns>
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}