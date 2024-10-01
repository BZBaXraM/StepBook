using Likes.API.Data;
using Likes.API.Mappings;
using Likes.API.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LikeContext>(options =>
{
    var connectionString =
        "Host=localhost;Port=5434;Database=LikeDb;Username=postgres;Password=postgres;Include Error Detail=true";
    options.UseNpgsql(connectionString);
});

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped<ILikesRepository, LikesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();