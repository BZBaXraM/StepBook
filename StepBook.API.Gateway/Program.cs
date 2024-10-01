// /src/StepBook.API.Gateway/Program.cs

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();