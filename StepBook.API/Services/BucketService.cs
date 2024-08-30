namespace StepBook.API.Services;

/// <summary>
/// Bucket service.
/// </summary>
/// <param name="amazonS3"></param>
public class BucketService(IAmazonS3 amazonS3) : IBucketService
{
    /// <summary>
    /// Upload a file.
    /// </summary>
    /// <param name="file"></param>
    public async Task UploadFileAsync(IFormFile file)
    {
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = "stepbook-bucket",
            Key = file.FileName,
            InputStream = file.OpenReadStream()
        };

        await amazonS3.PutObjectAsync(putObjectRequest);
    }
}