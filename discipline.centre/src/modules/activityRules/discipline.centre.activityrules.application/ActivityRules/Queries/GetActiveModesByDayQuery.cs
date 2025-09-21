using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.domain.Enums;
using discipline.libs.cqrs.abstractions.Queries;

namespace discipline.centre.activityrules.application.ActivityRules.Queries;

public sealed record GetActiveModesByDayQuery(DateOnly Day) : IQuery<ActiveModesDto>;

internal sealed class GetActiveModesByDayQueryHandler : IQueryHandler<GetActiveModesByDayQuery, ActiveModesDto>
{
    public Task<ActiveModesDto> HandleAsync(GetActiveModesByDayQuery query, CancellationToken cancellationToken = default)
    {
        List<string> modes = [RuleMode.EveryDay.Value];
        
        var dayOfWeek = query.Day.DayOfWeek;
        
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday:
                modes.Add(RuleMode.FirstDayOfWeek.Value);
                break;
            case DayOfWeek.Sunday:
                modes.Add(RuleMode.LastDayOfWeek.Value);
                break;
        }

        var firstDayOfNextMonth = new DateOnly(
            query.Day.Month == 12 ? query.Day.Year + 1 : query.Day.Year, 
            query.Day.Month == 12 ? 1 :query.Day.Month + 1,
            1);
        var lastDayOfMonth = firstDayOfNextMonth.AddDays(-1).Day;

        if(query.Day.Day == 1)
        {
            modes.Add(RuleMode.FirstDayOfMonth.Value);
        }
        if(query.Day.Day == lastDayOfMonth)
        {
            modes.Add(RuleMode.LastDayOfMonth.Value);
        }
        
        var day = (query.Day.DayOfWeek == 0 ? 7 : (int)query.Day.DayOfWeek);
        
        return Task.FromResult(new ActiveModesDto()
        {
            Modes = modes,
            Day = day
        });
    }
}