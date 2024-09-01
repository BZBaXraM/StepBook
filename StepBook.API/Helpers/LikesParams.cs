namespace StepBook.API.Helpers;

public class LikesParams : PaginationParams
{
    public Guid UserId { get; set; }
    public required string Predicate { get; set; } = "liked";
}