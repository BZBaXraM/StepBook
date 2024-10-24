namespace StepBook.Models.Models;

/// <summary>
/// Represents a user in the application.
/// </summary>
public class User
{
    /// <summary>
    /// The id of the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The email of the user.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The login username of the user.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// The password of the user.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// The email of the user.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// The user's known as name.
    /// </summary>
    public required string KnownAs { get; set; }

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
    public required string Gender { get; set; }

    /// <summary>
    /// The user's self-introduction.
    /// </summary>
    public string? Introduction { get; set; }

    /// <summary>
    /// The user's interests.
    /// </summary>
    public string? Interests { get; set; }

    /// <summary>
    /// The user's looking for.
    /// </summary>
    public string? LookingFor { get; set; }

    /// <summary>
    /// City of the user.
    /// </summary>
    public required string City { get; set; }

    /// <summary>
    /// Country of the user.
    /// </summary>
    public required string Country { get; set; }

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
    /// Is the email of the user confirmed?
    /// </summary>
    public bool IsEmailConfirmed { get; set; }

    /// <summary>
    /// The email confirmation token of the user.
    /// </summary>
    public string? EmailConfirmationToken { get; set; }

    /// <summary>
    /// The forgot password token of the user.
    /// </summary>
    public string? ForgotPasswordToken { get; set; }

    /// <summary>
    /// The random code of the user.
    /// </summary>
    public string? RandomCode { get; set; }

    /// <summary>
    /// Refresh token of the user.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Refresh token expire time.
    /// </summary>
    public DateTime RefreshTokenExpireTime { get; set; }

    /// <summary>
    /// The email confirmation code of the user.
    /// </summary>
    public string? EmailConfirmationCode { get; set; }
}