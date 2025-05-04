using System.Globalization;
using System.Reflection;
using discipline.centre.shared.abstractions.Exceptions.Services;
using discipline.centre.shared.infrastructure.Exceptions;
using discipline.centre.shared.infrastructure.Exceptions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class ExceptionsServicesConfigurationExtension
{
    internal static IServiceCollection AddExceptionsHandling(this IServiceCollection services,
        IList<Assembly> assemblies)
    {
        services
            .AddProblemDetails()
            .AddExceptionHandler<ExceptionHandler>();
        
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        
        var supportedCultures = new[] { "en", "pl" };
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("en");
            options.SupportedCultures = supportedCultures.Select(x => new CultureInfo(x)).ToList();
            options.SupportedUICultures = supportedCultures.Select(x => new CultureInfo(x)).ToList();
        });

        services.AddSingleton<IErrorLocalizationService>(sp =>
        {
            var localizerFactory = sp.GetRequiredService<IStringLocalizerFactory>();
            return new ErrorLocalizationService(assemblies, localizerFactory);
        });

        return services;
    }

}