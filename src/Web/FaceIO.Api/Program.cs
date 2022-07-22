using FaceIO.Commands.Common;
using FaceIO.Contracts.Common.Database.Context;
using FaceIO.Domain.Common.Database.Context;
using FaceIO.Domain.Customer.Repositories;
using FaceIO.Domain.Group.Repositories;
using FaceIO.Domain.GroupLocation.Repositories;
using FaceIO.Domain.Location.Repositories;
using FaceIO.Domain.Person.Repositories;
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

builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();
builder.Services.AddScoped<ILocationsRepository, LocationsRepository>();
builder.Services.AddScoped<IGroupsRepository, GroupsRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<IGroupLocationsRepository, GroupLocationsRepository>();

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

app.Run();
