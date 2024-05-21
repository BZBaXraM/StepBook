using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StepBook.API.Data;
using StepBook.API.Data.Entities;
using StepBook.API.Services.Classes;
using StepBook.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext
builder.Services.AddDbContext<StepContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AuthContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityConnection")));

// Register Identity services
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthContext>()
    .AddDefaultTokenProviders();

JwtConfig jwtConfig = new();
builder.Configuration.GetSection("JWT").Bind(jwtConfig);
builder.Services.AddSingleton(jwtConfig);
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();