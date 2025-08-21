using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.Queries;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.activityrules.api;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class ActivityRulesModule : IModule
{
    internal const string ModuleName = "activity-rules-module";
    
    public string Name => "ActivityRules";

    public void Register(IServiceCollection services, IConfiguration configuration)
        => services.AddInfrastructure(ModuleName);
    

    public void Use(WebApplication app)
    {
        app
            .MapActivityRulesEndpoints()
            .MapActivityRulesInternalEndpoints();
        
        app.UseModuleRequest()
            .MapModuleRequest<GetActivityRuleByIdQuery, ActivityRuleResponseDto>("activity-rules/get",(query, sp) 
                => sp.GetRequiredService<ICqrsDispatcher>().SendAsync(query, CancellationToken.None));
    }
}