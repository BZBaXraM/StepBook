using System.ComponentModel.DataAnnotations;

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
    /// The user's known as name
    /// </summary>
    public required string KnownAs { get; init; } = null!;

    /// <summary>
    /// The user's
    /// </summary>
    public required string Gender { get; init; } = null!;

    /// <summary>
    /// The user's date of birth
    /// </summary>
    public required DateTime DateOfBirth { get; init; }

    /// <summary>
    /// The user's city
    /// </summary>
    public required string City { get; init; } = null!;

    /// <summary>
    /// The user's country
    /// </summary>
    public required string Country { get; init; } = null!;

    /// <summary>
    /// The password
    /// </summary>
   // [StringLength(11, MinimumLength = 11)]
    public required string Password { get; set; } = null!;
}
