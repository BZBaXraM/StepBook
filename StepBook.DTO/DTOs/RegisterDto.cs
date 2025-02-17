namespace StepBook.DTO.DTOs;

/// <summary>
/// DTO for user registration.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// The user's email
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The username
    /// </summary>
    public required string Username { get; init; }

    public required string FirstName { get; init; }
    public required string LastName { get; init; }

    /// <summary>
    /// The user's
    /// </summary>
    public required string Gender { get; init; }

    /// <summary>
    /// The user's date of birth
    /// </summary>
    public required DateTime DateOfBirth { get; init; }

    /// <summary>
    /// The user's city
    /// </summary>
    public required string City { get; init; }

    /// <summary>
    /// The user's country
    /// </summary>
    public required string Country { get; init; }

    /// <summary>
    /// The password
    /// </summary>
    public required string Password { get; set; }
}