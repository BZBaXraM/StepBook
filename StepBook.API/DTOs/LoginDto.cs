namespace StepBook.API.DTOs;

public class LoginDto
{
    public required string Username { get; init; } = null!;
    public required string Password { get; init; } = null!;
}