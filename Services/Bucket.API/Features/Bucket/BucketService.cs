using Amazon.S3;
using Amazon.S3.Model;

namespace Bucket.API.Features.Bucket;

public class BucketService(IAmazonS3 amazonS3) : IBucketService
{
    private const string BucketName = "stepbook-bucket";
    private const string BucketUrl = "https://stepbook-bucket.s3.eu-north-1.amazonaws.com/";

    /// <summary>
    /// Upload a file.
    /// </summary>
    /// <param name="file"></param>
    public async Task UploadFileAsync(IFormFile file)
    {
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = file.FileName,
            InputStream = file.OpenReadStream()
        };

        await amazonS3.PutObjectAsync(putObjectRequest);
    }

    /// <summary>
    /// Get file URL without pre-signed parameters.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public Task<string> GetFileUrlAsync(string fileName)
    {
        // Directly return the public URL to the file
        var fileUrl = $"{BucketUrl}{fileName}";
        return Task.FromResult(fileUrl);
    }

    /// <summary>
    /// Get all files.
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> GetAllFilesAsync()
    {
        var request = new ListObjectsV2Request
        {
            BucketName = BucketName
        };

        var response = await amazonS3.ListObjectsV2Async(request);

        return response.S3Objects.Select(x => $"{BucketUrl}{x.Key}").ToList();
    }
}