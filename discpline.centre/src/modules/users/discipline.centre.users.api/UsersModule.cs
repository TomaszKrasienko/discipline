using discipline.centre.shared.abstractions.Modules;
using discipline.centre.users.api.Endpoints;
using discipline.centre.users.domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.users.api;

internal sealed class UsersModule : IModule
{
    internal const string ModuleName = "users-module";
    public string Name => "Users";

    public void Register(IServiceCollection services, IConfiguration configuration)
        => services
            .AddInfrastructure(ModuleName, configuration)
            .AddDomain();

    public void Use(WebApplication app)
    {
        app.MapUsersEndpoints();
    }
}