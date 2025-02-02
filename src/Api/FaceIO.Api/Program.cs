using Amazon;
using Amazon.Rekognition;
using Amazon.S3;
using FaceIO.Api.Settings;
using FaceIO.Commands.Common;
using FaceIO.Contracts.Common.Database.Context;
using FaceIO.Contracts.Common.Settings;
using FaceIO.Domain.Common.Database.Context;
using FaceIO.Domain.Customer.Repositories;
using FaceIO.Domain.Group.Repositories;
using FaceIO.Domain.Location.Repositories;
using FaceIO.Domain.Person.Repositories;
using FaceIO.Domain.PersonInGroup.Repositories;
using FaceIO.Queries.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddScoped<IFaceIODbContext, FaceIODbContext>();
builder.Services.AddDbContext<FaceIODbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FaceIODb")));

builder.Services.AddScoped<IAwsSettings>(_ => builder.Configuration.GetSection("AwsSettings").Get<AwsSettings>());

builder.Services.AddScoped<IAmazonS3>(serviceProvider =>
{
    var awsSetings = serviceProvider.GetRequiredService<IAwsSettings>();

    return new AmazonS3Client(awsSetings.AccessKey, awsSetings.AccessSecretKey, RegionEndpoint.EUCentral1);
});

builder.Services.AddCors(f => f.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()
           .SetIsOriginAllowed(_ => true);
}));

builder.Services.AddScoped<IAmazonRekognition>(serviceProvider =>
{
    var awsSetings = serviceProvider.GetRequiredService<IAwsSettings>();

    return new AmazonRekognitionClient(awsSetings.AccessKey, awsSetings.AccessSecretKey, RegionEndpoint.EUCentral1);
});

builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();
builder.Services.AddScoped<ILocationsRepository, LocationsRepository>();
builder.Services.AddScoped<IGroupsRepository, GroupsRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<IPersonsInGroupsRepository, PersonsInGroupsRepository>();

builder.Services.AddMediatR(new[] { typeof(QueriesAssemblyMarker).Assembly, typeof(CommandsAssemblyMarker).Assembly });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

app.Run();
