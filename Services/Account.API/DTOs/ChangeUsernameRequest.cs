namespace Account.API.DTOs;

/// <summary>
/// Change username request DTO
/// </summary>
public class ChangeUsernameRequest
{
    /// <summary>
    /// New username
    /// </summary>
    public string NewUsername { get; set; } = null!;
}