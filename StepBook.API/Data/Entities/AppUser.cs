using Microsoft.AspNetCore.Identity;
using StepBook.API.Models;

namespace StepBook.API.Data.Entities;

public class AppUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}