using discipline.daily_trackers.application.UserDailyTrackers.Commands;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Requests;

public static class CreateEmptyUserDailyTrackerRequestDtoMapperExtensions
{
    public static CreateUserDailyTrackerCommand ToCommand(this CreateEmptyUserDailyTrackerRequestDto dto)
        => new(
            DailyTrackerId.Parse(dto.DailyTrackerId),
            AccountId.Parse(dto.AccountId),
            dto.Day);
}