using Blazored.LocalStorage;
using discipline.ui.infrastructure.Storage.Abstractions;
using discipline.ui.infrastructure.Storage.Internals;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class StorageServicesConfigurationExtensions
{
    internal static IServiceCollection SetStorageServices(this IServiceCollection services)
        => services
            .AddBlazoredLocalStorageAsSingleton()
            .AddSingleton<ILocalStorageAccessor, LocalStorageAccessor>();
}