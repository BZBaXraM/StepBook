namespace StepBook.API.Services;

/// <summary>
/// Bucket service.
/// </summary>
public interface IBucketService
{
    /// <summary>
    /// Upload a file.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task UploadFileAsync(IFormFile file);
    Task<string> GetFileUrlAsync(string fileName);
    Task<List<string>> GetAllFilesAsync();
    
}