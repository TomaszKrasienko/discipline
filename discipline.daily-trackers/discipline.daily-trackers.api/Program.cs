using System.Reflection;
using discipline.daily_trackers.application.UserDailyTrackers.DTOs.Requests;
using discipline.daily_trackers.infrastructure.Configuration;
using discipline.libs.cqrs.Abstractions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

var basePath = AppContext.BaseDirectory;
var assemblies = Directory.GetFiles(basePath, "discipline.daily-trackers*.dll")
    .Select(Assembly.LoadFrom)
    .ToList();

builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddInfrastructure(builder.Configuration, assemblies)
    .AddDomain();

var app = builder.Build();

app.MapPost(
    "/api/user-daily-trackers/activities",
    async (CreateActivityRequestDto request,
        IValidator<CreateActivityRequestDto> validator,
        ICqrsDispatcher dispatcher,
        IHttpContextAccessor accessor) =>)

app.UseHttpsRedirection();
app.UseInfrastructure();

app.Run();