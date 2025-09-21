using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.daily_trackers.application.UserDailyTrackers.Commands.External;

public static class CreateEmptyUserDailyTrackerMapperExtensions
{
    public static CreateUserDailyTrackerCommand ToCommand(this CreateEmptyUserDailyTrackerCommand dto)
        => new(DailyTrackerId.New(),
            AccountId.Parse(dto.AccountId),
            dto.Day);
}