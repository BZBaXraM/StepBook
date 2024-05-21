using Microsoft.EntityFrameworkCore;
using StepBook.API.Entities;

namespace StepBook.API.Data;

public class StepContext(DbContextOptions<StepContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
}