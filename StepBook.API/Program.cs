var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddAuthorizationBuilder()
//     .AddPolicy("Admin", policy => policy.RequireRole("Admin"));


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Role" && c.Value == "Admin")));
});

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("/Logs/StepBook.log", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();


builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddSwagger(builder.Configuration);


builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseMiddleware<BlackListMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<StepContext>();
    await context.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("DELETE FROM  \"Connections\"");
}
catch (Exception e)
{
    var log = services.GetRequiredService<ILogger<Program>>();
    log.LogError(e, "An error occurred during migration");
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

await app.RunAsync();