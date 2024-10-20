using Messages.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Messages.API.Data;

public class MessageContext(DbContextOptions<MessageContext> options) : DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Connection> Connections => Set<Connection>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(x => x.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

        // Игнорируем ненужные сущности для этого контекста
        // modelBuilder.Ignore<UserLike>();
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            foreach (var property in entry.Properties)
            {
                if (property.CurrentValue is DateTime dateTime && dateTime.Kind == DateTimeKind.Local)
                {
                    property.CurrentValue = dateTime.ToUniversalTime();
                }
            }
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}