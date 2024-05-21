using Microsoft.EntityFrameworkCore;
using StepBook.API.Data.Entities;
using StepBook.API.Models;

namespace StepBook.API.Data;

public class StepContext(DbContextOptions<StepContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}