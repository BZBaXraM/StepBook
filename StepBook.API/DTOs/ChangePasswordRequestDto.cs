namespace StepBook.API.DTOs;

/// <summary>
/// Change password request data transfer object.
/// </summary>
public class ChangePasswordRequestDto
{
    /// <summary>
    /// The current password.
    /// </summary>
    public string CurrentPassword { get; set; } = null!;

    /// <summary>
    /// The new password.
    /// </summary>
    public string NewPassword { get; set; } = null!;

    /// <summary>
    /// The confirmation new password.
    /// </summary>
    public string ConfirmNewPassword { get; set; } = null!;
}