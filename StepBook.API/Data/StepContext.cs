using StepBook.API.Controllers;

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
    /// Messages table.
    /// </summary>
    public DbSet<Message> Messages => Set<Message>();

    /// <summary>
    /// Groups table.
    /// </summary>
    public DbSet<Group> Groups => Set<Group>();

    /// <summary>
    /// Connections table.
    /// </summary>
    public DbSet<Connection> Connections => Set<Connection>();

    public DbSet<BlackListedUser> BlackListedUsers => Set<BlackListedUser>();

    /// <summary>
    /// Configure the database context.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.TargetUserId });

        modelBuilder.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.LikedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserLike>()
            .HasOne(s => s.TargetUser)
            .WithMany(l => l.LikedByUsers)
            .HasForeignKey(s => s.TargetUserId)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(x => x.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BlackListedUser>()
            .HasKey(b => new { b.UserId, b.BlackListedUserId });

        modelBuilder.Entity<BlackListedUser>()
            .HasOne(b => b.User)
            .WithMany(u => u.BlackListedUsers)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BlackListedUser>()
            .HasOne(b => b.BlackList)
            .WithMany(u => u.BlackListedByUsers)
            .HasForeignKey(b => b.BlackListedUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}