using System.ComponentModel.DataAnnotations;

namespace StepBook.API.DTOs;

/// <summary>
/// Data Transfer Object for user registration.
/// </summary>
public class RegisterDto
{
   public required string Username { get; set; } = null!;
   public required string Password { get; set; } = null!;
}