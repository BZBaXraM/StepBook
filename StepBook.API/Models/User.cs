namespace StepBook.API.Models;

public class User
{
    public int Id { get; init; }
    public string UserName { get; set; } = null!;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public string? Gender { get; set; }
    public string? Introduction { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public ICollection<Photo> Photos { get; set; } = [];

    // public int GetAge() 
    // => DateOfBirth.CalculateAge();
}