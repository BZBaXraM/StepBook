namespace StepBook.API.DTOs;

/// <summary>
/// Change password request DTO
/// </summary>
public class ChangePasswordRequestDto
{
    /// <summary>
    /// Current password 
    /// </summary>
    public string? CurrentPassword { get; set; } 

    /// <summary>
    /// New password
    /// </summary>
    public string? NewPassword { get; set; } 

    /// <summary>
    /// Confirm new password
    /// </summary>
    public string? ConfirmNewPassword { get; set; } 
}