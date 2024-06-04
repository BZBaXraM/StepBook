using Microsoft.Extensions.Options;

namespace StepBook.API.Services.Classes;

/// <summary>
/// Photo service.
/// </summary>
public class PhotoService : IAsyncPhotoService
{
    private readonly Cloudinary _cloudinary;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="helper"></param>
    public PhotoService(IOptions<CloudinaryHelper> helper)
    {
        var account = new Account
        {
            Cloud = helper.Value.CloudName,
            ApiKey = helper.Value.ApiKey,
            ApiSecret = helper.Value.ApiSecret
        };

        _cloudinary = new Cloudinary(account);
    }

    /// <summary>
    /// Add photo to cloudinary.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length <= 0) return uploadResult;
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation()
                .Height(500)
                .Width(500)
                .Crop("fill")
                .Gravity("face")
        };

        uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult;
    }

    /// <summary>
    /// Delete photo from cloudinary.
    /// </summary>
    /// <param name="publicId"></param>
    /// <returns></returns>
    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        => await _cloudinary.DestroyAsync(new DeletionParams(publicId));
}