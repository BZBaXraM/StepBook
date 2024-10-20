using Messages.API.Data;
using Messages.API.Extensions;
using Messages.API.Hubs;
using Messages.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(builder.Configuration);
builder.Services.AuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddHttpClient("Account.API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/");
    client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
});

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddScoped<UserService>();

builder.Services.AddCors();

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
    .WithOrigins("http://localhost:4200")
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<MessageContext>();
    await context.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("DELETE FROM \"Connections\"");
}
catch (Exception e)
{
    var log = services.GetRequiredService<ILogger<Program>>();
    log.LogError(e, "An error occurred during migration");
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

await app.RunAsync();