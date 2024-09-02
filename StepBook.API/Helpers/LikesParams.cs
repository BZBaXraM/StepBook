namespace StepBook.API.Helpers;

/// <summary>
/// The likes parameters
/// </summary>
public class LikesParams : PaginationParams
{
    /// <summary>
    /// The user id
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// The predicate
    /// </summary>
    public required string Predicate { get; set; } = "liked";
}