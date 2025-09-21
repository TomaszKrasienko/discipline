using System.Reflection;
using discipline.libs.cqrs;
using discipline.libs.cqrs.Abstractions;
using discipline.libs.cqrs.abstractions.Commands;
using discipline.libs.cqrs.abstractions.Queries;
using discipline.libs.cqrs.Attributes;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CqrsServicesConfigExtensions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(
                publicOnly:false,
                action: c 
                    => c.AssignableTo(typeof(ICommandHandler<>)).WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(publicOnly:false,
                action: c
                    => c.AssignableTo(typeof(IQueryHandler<,>)).WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.AddSingleton<ICqrsDispatcher, CqrsDispatcher>();
        return services;
    }
}