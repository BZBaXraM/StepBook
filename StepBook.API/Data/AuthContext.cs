using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StepBook.API.Data.Entities;

namespace StepBook.API.Data;

public class AuthContext(DbContextOptions<AuthContext> options) : IdentityDbContext<AppUser>(options)
{
    public override DbSet<AppUser> Users => Set<AppUser>();
}