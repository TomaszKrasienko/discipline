using discipline.daily_trackers.infrastructure.Constraint;


// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class ConstraintsServicesConfigurationExtensions
{
    internal static IServiceCollection AddConstraints(this IServiceCollection services)
        => services.AddRouting(options =>
        {
            options.ConstraintMap.Add("ulid", typeof(UlidRouteConstraint));
            options.ConstraintMap.Add("dateonly", typeof(DateOnlyRouteConstraint));
        });
}