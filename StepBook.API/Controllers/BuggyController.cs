namespace StepBook.API.Controllers;

/// <summary>
/// This controller is used to test the exception middleware.
/// </summary>
/// <param name="context"></param>
public class BuggyController(StepContext context) : ControllerBase
{
    /// <summary>
    ///     This method is used to test the exception middleware.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetAuth()
    {
        return "secret text";
    }

    /// <summary>
    ///    This method is used to test the exception middleware.
    /// </summary>
    /// <returns></returns>
    [HttpGet("not-found")]
    public ActionResult<User> GetNotFound()
    {
        var thing = context.Users.Find(-1);

        if (thing == null) return NotFound();

        return thing;
    }

    /// <summary>
    ///    This method is used to test the exception middleware.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpGet("server-error")]
    public ActionResult<User> GetServerError()
    {
        var thing = context.Users.Find(-1) ?? throw new Exception("A bad thing has happened");

        return thing;
    }

    /// <summary>
    ///   This method is used to test the exception middleware.
    /// </summary>
    /// <returns></returns>
    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was not a good request");
    }
}