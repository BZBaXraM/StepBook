namespace StepBook.API.Repositories.Interfaces;

/// <summary>
/// The likes service interface
/// </summary>
public interface ILikesRepository
{
    Task<UserLike?> GetUserLike(Guid sourceUserId, Guid targetUserId);
    Task<PageList<MemberDto>> GetUserLikes(LikesParams likesParams);
    Task<IEnumerable<Guid>> GetCurrentUserLikeIds(Guid currentUserId);
    void DeleteLike(UserLike like);
    void AddLike(UserLike like);
}