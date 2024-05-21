using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StepBook.API.Data;
using StepBook.API.Entities;

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
    public async Task<IActionResult> AddUser(AppUser user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, AppUser user)
    {
        var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (existingUser == null)
        {
            return NotFound();
        }

        existingUser.UserName = user.UserName;
        await context.SaveChangesAsync();
        return Ok(existingUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (existingUser == null)
        {
            return NotFound();
        }

        context.Users.Remove(existingUser);
        await context.SaveChangesAsync();
        return Ok(existingUser);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers(string username)
    {
        var users = await context.Users.Where(x => x.UserName.Contains(username.ToLower())).ToListAsync();
        return Ok(users);
    }

    [HttpDelete("delete-all")]
    public async Task<IActionResult> DeleteAllUsers()
    {
        context.Users.RemoveRange(context.Users);
        await context.SaveChangesAsync();
        return Ok();
    }
}