using System.ComponentModel.DataAnnotations;

namespace StepBook.Domain.DTOs;

/// <summary>
/// Represents the data transfer object for the forget user password request.
/// </summary>
public class ForgetUserPasswordRequestDto
{
    /// <summary>
    /// The email of the user.
    /// </summary>
    [EmailAddress] public required string Email { get; set; }
    /// <summary>
    /// The client URI.
    /// </summary>
    public required string ClientURI { get; set; }
}