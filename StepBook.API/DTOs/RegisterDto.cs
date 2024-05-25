using System.ComponentModel.DataAnnotations;

namespace StepBook.API.DTOs;

/// <summary>
/// Data Transfer Object for user registration.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    [Required]
    public string Username { get; set; } = null!;

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    [DataType(DataType.EmailAddress)]
    [Required]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date of birth of the user.
    /// </summary>
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the known alias of the user.
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    public string KnownAs { get; set; } = null!;

    /// <summary>
    /// Gets or sets the gender of the user.
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    public string Gender { get; set; } = null!;

    /// <summary>
    /// Gets or sets the introduction of the user.
    /// </summary>
    [DataType(DataType.Text)]
    public string? Introduction { get; set; }

    /// <summary>
    /// Gets or sets what the user is looking for.
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    public string LookingFor { get; set; } = null!;

    /// <summary>
    /// Gets or sets the interests of the user.
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    public string Interests { get; set; } = null!;

    /// <summary>
    /// Gets or sets the country of the user.
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    public string Country { get; set; } = null!;

    /// <summary>
    /// Gets or sets the city of the user.
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    public string City { get; set; } = null!;
}