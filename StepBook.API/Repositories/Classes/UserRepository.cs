using StepBook.API.Repositories.Interfaces;

namespace StepBook.API.Repositories.Classes;

/// <summary>
/// Service for the User
/// </summary>
public class UserRepository(StepContext context, IMapper mapper) : IUserRepository
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<User>> GetUsersAsync()
        => await context.Users.Include(x => x.Photos).ToListAsync();

    /// <summary>
    /// Update a user
    /// </summary>
    /// <param name="user"></param>
    public async Task UpdateUserAsync(User user)
    {
        context.Entry(user).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Get a user by their username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<User> GetUserByUserNameAsync(string? username)
        => (await context.Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.UserName == username))!;

    /// <summary>
    /// Save all changes
    /// </summary>
    /// <returns></returns>
    public async Task<bool> SaveAllAsync()
        => await context.SaveChangesAsync() > 0;


    /// <summary>
    /// Get a user by their id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User> GetUserByIdAsync(int id)
        => (await context.Users.FirstOrDefaultAsync(x => x.Id == id))!;

    /// <summary>
    /// Get all members
    /// </summary>
    /// <returns></returns>
    public async Task<PageList<MemberDto>> GetMembersAsync(PageParams pageParams)
    {
        var query = context.Users
            .AsQueryable();

        query = query.Where(x => x.UserName != pageParams.CurrentUsername);
        query = query.Where(x => x.Gender == pageParams.Gender);

        var minDob = DateTime.Today.AddYears(-pageParams.MaxAge - 1);
        var maxDob = DateTime.Today.AddYears(-pageParams.MinAge);

        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

        query = pageParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive)
        };

        return await PageList<MemberDto>.CreateAsync(
            query.ProjectTo<MemberDto>(mapper.ConfigurationProvider).AsNoTracking(),
            PaginationParams.PageNumber,
            pageParams.PageSize);
    }

    /// <summary>
    /// Get a member by their username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<MemberDto> GetMemberAsync(string username)
        => (await context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync())!;
}