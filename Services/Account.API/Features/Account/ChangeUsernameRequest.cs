namespace Account.API.Features.Account;

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