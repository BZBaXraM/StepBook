namespace StepBook.BLL.Services;

public interface IBucketService
{
    Task UploadFileAsync(IFormFile file);
    Task<string> GetFileUrlAsync(string fileName);
    Task<List<string>> GetAllFilesAsync();
}