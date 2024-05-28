namespace StepBook.API.DTOs;

/// <summary>
/// The user data transfer object
/// </summary>
public class UserDto
{
    public string Username { get; set; } = null!;
    public string Token { get; set; } = null!;
}