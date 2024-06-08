namespace StepBook.API.Services.Classes;

/// <summary>
/// Represents a service for likes.
/// </summary>
/// <param name="context"></param>
public class LikesService(StepContext context) : IAsyncLikesService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LikesService"/> class.
    /// </summary>
    /// <param name="sourceUserId"></param>
    /// <param name="likedUserId"></param>
    /// <returns></returns>
    public async Task<UserLike> GetUserLikeAsync(int sourceUserId, int likedUserId)
        => (await context.Likes.FindAsync(sourceUserId, likedUserId))!;


    /// <summary>
    ///  Gets a user with their likes.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<User> GetUserWithLikesAsync(int userId)
        => (await context.Users
            .Include(x => x.LikedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId))!;

    /// <summary>
    ///    Gets a user's likes.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<IEnumerable<LikeDto>> GetUserLikesAsync(string predicate, int userId)
    {
        var users = context.Users.OrderBy(x => x.UserName).AsQueryable();
        var likes = context.Likes.AsQueryable();

        switch (predicate)
        {
            case "liked":
                likes = likes.Where(x => x.SourceUserId == userId);
                users = likes.Select(x => x.LikedUser);
                break;
            case "likedBy":
                likes = likes.Where(x => x.LikedUserId == userId);
                users = likes.Select(x => x.SourceUser);
                break;
            default:
                throw new ArgumentException("Invalid predicate");
        }

        return await users.Select(user => new LikeDto
        {
            Username = user.UserName,
            Age = user.DateOfBirth.CalculateAge(),
            KnownAs = user.KnownAs!,
            PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)!.Url,
            City = user.City!,
            Id = user.Id
        }).ToListAsync();
    }
}