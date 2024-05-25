using Microsoft.EntityFrameworkCore;
using StepBook.API.Models;

namespace StepBook.API.Data;

public class StepContext : DbContext
{
    public StepContext(DbContextOptions<StepContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}