using System.ComponentModel.DataAnnotations;

namespace Account.API.Features.Account;

/// <summary>
/// Represents the data transfer object for the forget user password request.
/// </summary>
public class ForgetUserPasswordRequest
{
    /// <summary>
    /// The email of the user.
    /// </summary>
    [EmailAddress] public required string Email { get; set; } = null!;
    /// <summary>
    /// The client URI.
    /// </summary>
    public required string ClientURI { get; set; }
}