namespace StepBook.API.Controllers;

/// <summary>
/// Controller for managing reports.
/// </summary>
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReportController(IReportRepository reportRepository, IUserRepository userRepository) : ControllerBase
{
    /// <summary>
    /// Add a report to a user.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="reportDto"></param>
    /// <returns></returns>
    [HttpPost("add-report-to-user/{username}")] 
    public async Task<ActionResult> AddReportToUser([FromRoute] string username, [FromBody] ReportCreateDto reportDto)
    {
        var reporterId = User.GetUserId();
        var reportedUser = await userRepository.GetUserByUsernameAsync(username);

        if (reportedUser == null) return NotFound("User not found.");

        if (reportedUser.Id == reporterId) return BadRequest("You cannot report yourself.");

        var report = new Report
        {
            ReporterId = reporterId,
            ReportedId = reportedUser.Id,
            Reason = reportDto.Reason
        };

        await reportRepository.AddReportAsync(report);

        return Ok();
    }
}