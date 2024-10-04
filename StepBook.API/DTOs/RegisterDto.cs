namespace StepBook.API.DTOs;

/// <summary>
/// DTO for user registration.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// The user's email
    /// </summary>
    public required string Email { get; init; } = null!;

    /// <summary>
    /// The username
    /// </summary>
    public required string Username { get; init; } = null!;

    /// <summary>
    /// The user's first name
    /// </summary>
    public required string FirstName { get; init; } = null!;

    /// <summary>
    /// The user's last name
    /// </summary>
    public required string LastName { get; init; } = null!;

    /// <summary>
    /// The user's
    /// </summary>
    public string Gender { get; init; } = null!;

    /// <summary>
    /// The user's date of birth
    /// </summary>
    public DateTime DateOfBirth { get; init; }

    /// <summary>
    /// The user's city
    /// </summary>
    public string City { get; init; } = null!;

    /// <summary>
    /// The user's country
    /// </summary>
    public string Country { get; init; } = null!;

    /// <summary>
    /// The password
    /// </summary>
    public required string Password { get; set; } = null!;
}