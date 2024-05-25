using StepBook.API.Extensions;
using StepBook.API.Models;

public class User
{
    public int Id { get; init; }
    public string UserName { get; set; } = null!;
    public string Email { get; init; } = null!;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime DateOfBirth { get; init; }
    public string KnownAs { get; init; } = null!;
    public DateTime Created { get; init; } = DateTime.Now;
    public DateTime LastActive { get; init; } = DateTime.Now;
    public string Gender { get; init; } = null!;
    public string? Introduction { get; init; }
    public string? LookingFor { get; init; }
    public string? Interests { get; init; }
    public string Country { get; init; } = null!;
    public string City { get; init; } = null!;
    public ICollection<Photo>? Photos { get; init; } 
    
    public int GetAge() => DateOfBirth.CalculateAge();
}