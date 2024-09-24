namespace Account.API.Features.Account;

/// <summary>
/// Represents the data transfer object for the reset password.
/// </summary>
public class ResetPasswordRequest
{
    /// <summary>
    /// The email of the user.
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// The code of the user.
    /// </summary>
    public string Code { get; set; } = null!;
    /// <summary>
    /// The new password of the user.
    /// </summary>
    public string NewPassword { get; set; } = null!;
}