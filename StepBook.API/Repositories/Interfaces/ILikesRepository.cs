namespace StepBook.API.Repositories.Interfaces;

/// <summary>
/// The likes service interface
/// </summary>
public interface ILikesRepository
{
    /// <summary>
    /// Get a user like
    /// </summary>
    /// <param name="sourceUserId"></param>
    /// <param name="likedUserId"></param>
    /// <returns></returns>
    Task<UserLike> GetUserLikeAsync(int sourceUserId, int likedUserId);

    /// <summary>
    /// Get a user with their likes
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<User> GetUserWithLikesAsync(int userId);

    /// <summary>
    /// Get a user's likes with pagination
    /// </summary>
    /// <param name="likeParams"></param>
    /// <returns></returns>
    Task<PageList<LikeDto>> GetUserLikesAsync(LikeParams likeParams);
}