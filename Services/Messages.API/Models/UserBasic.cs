namespace Messages.API.Models;

public class UserBasic
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
}