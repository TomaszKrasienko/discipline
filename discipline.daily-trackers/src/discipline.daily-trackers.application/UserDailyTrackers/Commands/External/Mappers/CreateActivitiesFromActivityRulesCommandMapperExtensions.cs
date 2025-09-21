using discipline.daily_trackers.application.UserDailyTrackers.DTOs.ExternalCommands;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.application.UserDailyTrackers.Commands.External;

public static class CreateActivitiesFromActivityRulesCommandMapperExtensions
{
    public static CreateActivitiesFromRulesCommand ToCommand(this CreateActivitiesFromActivityRulesCommand command)
        => new CreateActivitiesFromRulesCommand(
            DailyTrackerId.New(),
            AccountId.Parse(command.AccountId),
            command.Day,
            command.Activities.Select(x => x.ToSpecification()).ToList());

    private static ActivitySpecification ToSpecification(this CreateActivityDto command)
        => new(
            ActivityId.New(),
            new ActivityDetailsSpecification(command.Details.Title, command.Details.Note),
            ActivityRuleId.Parse(command.ActivityRuleId),
            command.Stages.Select(x => x.ToSpecification()).ToList());

    private static StageSpecification ToSpecification(this CreateActivityStageDto command)
        => new(
            StageId.New(),
            command.Title,
            command.Index);
}