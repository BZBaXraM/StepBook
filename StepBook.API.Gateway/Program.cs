// /src/StepBook.API.Gateway/Program.cs

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://account-api:6060";
        options.RequireHttpsMetadata = false;
        options.Audience = "account-api";
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("DefaultPolicy", policy => { policy.RequireAuthenticatedUser(); });

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();