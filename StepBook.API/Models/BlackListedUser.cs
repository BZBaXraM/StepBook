namespace StepBook.API.Models;

/// <summary>
/// Represents a blacklisted user in the application.
/// </summary>
public class BlackListedUser
{
    /// <summary>
    /// The user id.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// The user.
    /// </summary>
    public User User { get; set; } = default!;

    /// <summary>
    /// The blacklisted user id.
    /// </summary>
    public int BlackListedUserId { get; set; }

    /// <summary>
    /// The blacklisted user.
    /// </summary>
    public User BlackList { get; set; } = default!;
}