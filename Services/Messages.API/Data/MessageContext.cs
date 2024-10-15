using Microsoft.EntityFrameworkCore;
using StepBook.Domain.Entities;

namespace Messages.API.Data;

public class MessageContext(DbContextOptions<MessageContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

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
    }
}