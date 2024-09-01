namespace StepBook.API.Models;

/// <summary>
/// User like
/// </summary>
public class UserLike
{
    /// <summary>
    /// Source user
    /// </summary>
    public User SourceUser { get; set; } = null!;

    /// <summary>
    /// Source user id
    /// </summary>
    public Guid SourceUserId { get; set; }

    /// <summary>
    /// Target user
    /// </summary>
    public User TargetUser { get; set; } = null!;

    /// <summary>
    /// Target user id
    /// </summary>
    public Guid TargetUserId { get; set; }
}