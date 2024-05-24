using System.ComponentModel.DataAnnotations;

namespace StepBook.API.DTOs;

/// <summary>
/// Login DTO
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Email
    /// </summary>
    [DataType(DataType.EmailAddress)]
    [Required]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Password
    /// </summary>
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; } = null!;
}