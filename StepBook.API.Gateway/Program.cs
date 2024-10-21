using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using StepBook.API.Gateway.Extensions;
using StepBook.API.Gateway.Middleware;

var builder = WebApplication.CreateBuilder(args); // StepBook.API.Gateway/Program.cs

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();

app.UseWebSockets();

await app.UseOcelot();

app.Run();