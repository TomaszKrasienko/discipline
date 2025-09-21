

using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.Abstractions;
using discipline.libs.events.abstractions;
// ReSharper disable once CheckNamespace
using discipline.centre.activityrules.application.ActivityRules.Commands.External;

namespace Microsoft.Extensions.DependencyInjection;

public static class ActivityRulesServicesInfrastructureConfigExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string moduleName)
        => services
            .AddDal(moduleName)
            .AddConsumer<CreateActivityForAccountFromActivityRuleCommand>(sp =>
            {
                return (async (msg, ct, mt) =>
                {
                    var rules = msg
                        .ActivityRuleId
                        .Select(ActivityRuleId.Parse)
                        .ToList();

                    var command = new CreateActivitiesFromActivityRuleCommand(
                        rules,
                        AccountId.Parse(msg.AccountId));
                        
                    using var scope = sp.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<ICqrsDispatcher>();
                    await handler.HandleAsync(command, ct);
                });
            });
}