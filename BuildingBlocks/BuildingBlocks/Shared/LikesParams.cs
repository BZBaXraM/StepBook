using StepBook.Domain.Shared;

namespace Likes.API.Shared;

public class LikesParams : PaginationParams
{
    public int UserId { get; set; }
    public required string Predicate { get; set; } = "liked";
}