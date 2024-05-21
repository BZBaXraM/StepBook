using System.ComponentModel.DataAnnotations;

namespace StepBook.API.DTOs;

public class LoginDto
{
    [DataType(DataType.EmailAddress)]
    [Required]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; } = null!;
}