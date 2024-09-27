using StepBook.Domain.Shared;

namespace BuildingBlocks.Shared;

public class LikesParams : PaginationParams
{
    public int UserId { get; set; }
    public required string Predicate { get; set; } = "liked";
}