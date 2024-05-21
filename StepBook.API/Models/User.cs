namespace StepBook.API.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}