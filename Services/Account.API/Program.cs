using Account.API.Extensions;
using Account.API.Middleware;
using AuthMiddleware.Jwt;
using BlackListMiddleware = Account.API.Middleware.BlackListMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(builder.Configuration); // Custom Swagger setup

builder.Services.AuthenticationAndAuthorization(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<BlackListMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

await app.RunAsync();