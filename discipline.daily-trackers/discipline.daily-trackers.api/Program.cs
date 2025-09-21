using System.Reflection;
using discipline.daily_trackers.infrastructure.Configuration;

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

app.UseHttpsRedirection();
app.UseInfrastructure();

app.Run();