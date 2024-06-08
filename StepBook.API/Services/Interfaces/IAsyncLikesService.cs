namespace StepBook.API.Services.Interfaces;

public interface IAsyncLikesService
{
    Task<UserLike> GetUserLikeAsync(int sourceUserId, int likedUserId);
    Task<User> GetUserWithLikesAsync(int userId);
    Task<IEnumerable<LikeDto>> GetUserLikesAsync(string predicate, int userId); 
}