using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses.Modes;
using discipline.centre.activityrules.domain.Enums;
using discipline.libs.cqrs.abstractions.Queries;

namespace discipline.centre.activityrules.application.ActivityRules.Queries;

public sealed record GetRuleModesQuery : IQuery<IReadOnlyCollection<RuleModeResponseDto>>;

internal sealed class GetRuleModesQueryHandler : IQueryHandler<GetRuleModesQuery, IReadOnlyCollection<RuleModeResponseDto>>
{
    public Task<IReadOnlyCollection<RuleModeResponseDto>> HandleAsync(GetRuleModesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = RuleMode.AvailableModes
            .Select(x => new RuleModeResponseDto(x.Value, x.IsDaysRequired)).ToArray();
        return Task.FromResult<IReadOnlyCollection<RuleModeResponseDto>>(result);
    } 
}