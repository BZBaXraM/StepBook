namespace StepBook.API.Helpers;

/// <inheritdoc />
public class LikeParams : PaginationParams
{
    /// <summary>
    /// The user id
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// The predicate
    /// </summary>
    public string Predicate { get; set; } = string.Empty;
}