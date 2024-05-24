using System.ComponentModel.DataAnnotations;
using StepBook.API.Models;

namespace StepBook.API.DTOs;

/// <summary>
/// Register DTO
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Username
    /// </summary>
    [Required]
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

    [Required] [DataType(DataType.Date)] public DateTime DateOfBirth { get; set; } = DateTime.Now;

    [Required] [DataType(DataType.Text)] public string KnownAs { get; set; } = null!;

    [Required] [DataType(DataType.Text)] public string Gender { get; set; } = null!;
   [DataType(DataType.Text)] public string? Introduction { get; set; }

    [Required] [DataType(DataType.Text)] public string LookingFor { get; set; } = null!;

    [Required] [DataType(DataType.Text)] public string Interests { get; set; } = null!;

    [Required] [DataType(DataType.Text)] public string Country { get; set; } = null!;

    [Required] [DataType(DataType.Text)] public string City { get; set; } = null!;

    [Required] [DataType(DataType.Text)] public string? PhotoUrl { get; set; }
}