namespace StepBook.API.Models;

/// <summary>
/// Represents a user in the application.
/// </summary>
public class User
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    /// <summary>
    /// The password hash of the user.
    /// </summary>
    public byte[] PasswordHash { get; set; }

    /// <summary>
    /// The password salt of the user.
    /// </summary>
    public byte[] PasswordSalt { get; set; }

    /// <summary>
    /// The email of the user.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// The email of the user.
    /// </summary>
    public string? KnownAs { get; set; }

    /// <summary>
    /// Created date of the user.
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last active date of the user.
    /// </summary>
    public DateTime LastActive { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gender of the user.
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// The user's self-introduction.
    /// </summary>
    public string? Introduction { get; set; }

    /// <summary>
    /// City of the user.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Country of the user.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// The user's photo URL.
    /// </summary>
    public ICollection<Photo> Photos { get; set; } = [];

    /// <summary>
    /// List of users liked by the current user.
    /// </summary>
    public ICollection<UserLike> LikedByUsers { get; set; } = [];

    /// <summary>
    /// List of users that liked the current user.
    /// </summary>
    public ICollection<UserLike> LikedUsers { get; set; } = [];

    /// <summary>
    /// List of messages sent by the current user.
    /// </summary>
    public ICollection<Message> MessagesSent { get; set; } = [];

    /// <summary>
    /// List of messages received by the current user.
    /// </summary>
    public ICollection<Message> MessagesReceived { get; set; } = [];

    /// <summary>
    /// Refresh token of the user.
    /// </summary>
    public string? RefreshToken { get; set; }
    /// <summary>
    /// Is the email of the user confirmed?
    /// </summary>
    public bool IsEmailConfirmed { get; set; }
    /// <summary>
    /// The email confirmation token of the user.
    /// </summary>
    public string? EmailConfirmationToken { get; set; }
}