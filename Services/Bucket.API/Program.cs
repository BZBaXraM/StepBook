using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Bucket.API.Features.Bucket;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var awsSetting = builder.Configuration.GetSection("AWS");
var credentials = new BasicAWSCredentials(awsSetting["AccessKey"], awsSetting["Secret"]);
var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Credentials = credentials;
awsOptions.Region = RegionEndpoint.EUNorth1;
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<IBucketService, BucketService>();

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