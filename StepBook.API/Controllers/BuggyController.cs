using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepBook.API.Data;
using StepBook.API.Models;

namespace StepBook.API.Controllers;

/// <summary>
/// Controller for testing error handling
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public class BuggyController(StepContext context) : ControllerBase
{
    [Authorize]
    [HttpGet("auth")]
    public async Task<ActionResult<string>> GetSecret()
    {
        await Task.CompletedTask; // No async operation here, so we just return a completed Task
        return "Secret text";
    }

    [HttpGet("not-found")]
    public async Task<ActionResult<User>> GetNotFound()
    {
        var thing = await context.Users.FindAsync(-1);

        if (thing == null)
            return NotFound();

        return Ok(thing);
    }

    [HttpGet("server-error")]
    public async Task<ActionResult<string>> GetServerError()
    {
        var thing = await context.Users.FindAsync(-1);

        var thingToReturn = thing?.ToString();

        return thingToReturn!;
    }

    [HttpGet("bad-request")]
    public async Task<ActionResult<string>> GetBadRequest()
    {
        await Task.CompletedTask;
        return BadRequest("This was not a good request.");
    }
}