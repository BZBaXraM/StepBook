using BuildingBlocks.Shared;
using StepBook.Domain.DTOs;
using StepBook.Domain.Entities;

namespace BuildingBlocks.Repository;

public interface ILikesRepository
{
    Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
    Task<PageList<MemberDto>> GetUserLikes(LikesParams likesParams);
    Task<List<int>> GetCurrentUserLikeIds(int currentUserId);
    Task AddLikeAsync(UserLike like);
    void DeleteLike(UserLike like);
}