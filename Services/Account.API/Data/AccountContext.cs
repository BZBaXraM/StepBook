using Account.API.Models;

namespace Account.API.Data;

public class AccountContext(DbContextOptions<AccountContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        // Игнорируем ненужные для этого контекста сущности
        // modelBuilder.Ignore<UserLike>();
        // modelBuilder.Ignore<Message>();
    }
}