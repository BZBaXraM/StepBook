using Microsoft.EntityFrameworkCore;
using StepBook.API.Models;

namespace StepBook.API.Data;

/// <inheritdoc />
public class StepContext(DbContextOptions<StepContext> options) : DbContext(options)
{
    /// <summary>
    /// Users table
    /// </summary>
    public DbSet<User> Users => Set<User>();
}