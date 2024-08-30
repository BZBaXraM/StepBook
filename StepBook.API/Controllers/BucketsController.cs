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
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        await service.UploadFileAsync(file);
        return Ok();
    }
}