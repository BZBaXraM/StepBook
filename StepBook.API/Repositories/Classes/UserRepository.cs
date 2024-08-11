using StepBook.API.Repositories.Interfaces;

namespace StepBook.API.Repositories.Classes;

/// <summary>
/// Service for the User
/// </summary>
public class UserRepository(StepContext context, IMapper mapper) : IUserRepository
{
    /// <summary>
    /// Update the user
    /// </summary>
    /// <param name="user"></param>
    public void Update(User user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await context.Users
            .Include(p => p.Photos)
            .AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Get the user by the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await context.Users
            .Include(p => p.Photos)
            .AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Get the user by the username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
            .Include(p => p.Photos)
            .AsNoTracking().FirstOrDefaultAsync(u => u.UserName == username);
    }

    /// <summary>
    /// Get the members
    /// </summary>
    /// <param name="pageParams"></param>
    /// <returns></returns>
    public async Task<PageList<MemberDto>> GetMembersAsync(PageParams pageParams)
    {
        var query = context.Users.AsQueryable().AsNoTracking();

        query = query.Where(u => u.UserName != pageParams.CurrentUsername);

        if (pageParams.Gender is null)
        {
            query = query.Where(x => x.Gender == pageParams.Gender);
        }

        var minDob = DateTime.Today.AddYears(-pageParams.MaxAge - 1);
        var maxDob = DateTime.Today.AddYears(-pageParams.MinAge);

        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob).AsQueryable();

        query = pageParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive)
        };

        return await PageList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider),
            pageParams.PageNumber, pageParams.PageSize);
    }

    /// <summary>
    /// Get the member by the username and if it is the current user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="isCurrentUser"></param>
    /// <returns></returns>
    public async Task<MemberDto?> GetMemberAsync(string username, bool isCurrentUser)
    {
        var query = context.Users.AsNoTracking().AsQueryable();

        query = query.Where(u => u.UserName == username);

        if (isCurrentUser)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query.ProjectTo<MemberDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    /// <summary>
    /// Get the user by the photo id
    /// </summary>
    /// <param name="photoId"></param>
    /// <returns></returns>
    public async Task<User?> GetUserByPhotoId(int photoId)
    {
        return await context.Users
            .Include(p => p.Photos)
            .AsNoTracking().FirstOrDefaultAsync(u => u.Photos.Any(p => p.Id == photoId));
    }
}