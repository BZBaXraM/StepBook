using StepBook.DAL.Entities;
using StepBook.DTO.DTOs;

namespace StepBook.DAL.Repositories.Interfaces;

/// <summary>
/// The likes service interface
/// </summary>
public interface ILikesRepository
{
    Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
    Task<PageList<MemberDto>> GetUserLikes(LikesParams likesParams);
    Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
    Task AddLikeAsync(UserLike like);
    void DeleteLike(UserLike like);
}