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

    /// <summary>
    /// Likes table.
    /// </summary>
    public DbSet<UserLike> Likes => Set<UserLike>();

    /// <summary>
    /// Configure the database context.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserLike>()
            .HasKey(x => new { x.SourceUserId, x.LikedUserId });

        modelBuilder.Entity<UserLike>()
            .HasOne(x => x.SourceUser)
            .WithMany(x => x.LikedUsers)
            .HasForeignKey(x => x.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserLike>()
            .HasOne(x => x.LikedUser)
            .WithMany(x => x.LikedByUsers)
            .HasForeignKey(x => x.LikedUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}