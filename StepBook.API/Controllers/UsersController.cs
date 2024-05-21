using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StepBook.API.Data;
using StepBook.API.Models;

namespace StepBook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(StepContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpDelete("delete-all")]
    public async Task<IActionResult> DeleteAllUsers()
    {
        context.Users.RemoveRange(context.Users);
        await context.SaveChangesAsync();
        return Ok();
    }
}