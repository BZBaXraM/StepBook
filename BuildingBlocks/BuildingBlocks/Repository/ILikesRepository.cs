using BuildingBlocks.Shared;
using Likes.API.Shared;
using StepBook.Domain.DTOs;
using StepBook.Domain.Entities;

namespace Likes.API.Data;

public interface ILikesRepository
{
    Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
    Task<PageList<MemberDto>> GetUserLikes(LikesParams likesParams);
    Task<List<int>> GetCurrentUserLikeIds(int currentUserId);
    Task AddLikeAsync(UserLike like);
    void DeleteLike(UserLike like);
}