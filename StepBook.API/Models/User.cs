using StepBook.API.Extensions;

namespace StepBook.API.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string KnownAs { get; set; } = null!;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.Now;
    public string Gender { get; set; } = null!;
    public string? Introduction { get; set; } // Mark as nullable
    public string? LookingFor { get; set; } // Mark as nullable
    public string? Interests { get; set; } // Mark as nullable
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? PhotoUrl { get; set; } // Mark as nullable

    public int GetAge()
        => DateOfBirth.CalculateAge();
}