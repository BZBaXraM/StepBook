using AuthMiddleware.Jwt;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using StepBook.API.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddCors();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();


await app.UseOcelot();

app.Run();