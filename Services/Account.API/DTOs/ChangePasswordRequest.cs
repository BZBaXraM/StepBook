namespace Account.API.DTOs;

/// <summary>
/// Change password request DTO
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// Current password 
    /// </summary>
    public string CurrentPassword { get; set; } = null!;

    /// <summary>
    /// New password
    /// </summary>
    public string NewPassword { get; set; } = null!;

    /// <summary>
    /// Confirm new password
    /// </summary>
    public string ConfirmNewPassword { get; set; } = null!;
}