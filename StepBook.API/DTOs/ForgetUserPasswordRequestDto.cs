using System.ComponentModel.DataAnnotations;

namespace StepBook.API.DTOs;

public class ForgetUserPasswordRequestDto
{
    [EmailAddress] public required string Email { get; set; } = null!;
    public required string? ClientURI { get; set; }
}