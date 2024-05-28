using Microsoft.EntityFrameworkCore;

namespace StepBook.API.Data;

/// <summary>
/// Database context for the application.
/// </summary>
public class StepContext : DbContext
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="options"></param>
    public StepContext(DbContextOptions<StepContext> options) : base(options)
    {
    }

    /// <summary>
    /// Users table.
    /// </summary>
    public DbSet<User> Users => Set<User>();
}