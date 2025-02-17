using StepBook.BLL.Services;

namespace StepBook.API.Controllers;

/// <summary>
/// Buckets controller.
/// </summary>
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class BucketsController(IBucketService service) : ControllerBase
{
    /// <summary>
    /// Upload a file.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("upload-file")]
    public async Task<IActionResult> UploadFile(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        await service.UploadFileAsync(file);
        var fileUrl = await service.GetFileUrlAsync(file.FileName);
        return Ok(fileUrl);
    }


    /// <summary>
    /// Get file URL.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [HttpGet("get-file-url")]
    public async Task<IActionResult> GetFileUrl(string fileName)
    {
        var url = await service.GetFileUrlAsync(fileName);
        return Ok(url);
    }

    /// <summary>
    /// Get all files.
    /// </summary>
    /// <returns></returns>
    [HttpGet("get-all-files")]
    public async Task<IActionResult> GetAllFiles()
    {
        var files = await service.GetAllFilesAsync();
        return Ok(files);
    }
    
    /// <summary>
    /// Delete file from cloud
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [HttpDelete("delete-file")]
    public async Task<IActionResult> DeleteFile(string fileName)
    {
        await service.DeleteFileAsync(fileName);
        return Ok();
    }
}