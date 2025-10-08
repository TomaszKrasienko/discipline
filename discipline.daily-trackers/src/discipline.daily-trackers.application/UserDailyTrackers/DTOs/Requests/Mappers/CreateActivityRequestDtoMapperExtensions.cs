using discipline.daily_trackers.application.UserDailyTrackers.Commands;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Requests;

public static class CreateActivityRequestDtoMapperExtensions
{
    public static CreateActivityCommand ToCommand(
        this CreateActivityRequestDto request,
        AccountId accountId,
        ActivityId activityId)
        => new(
            accountId,
            request.Day,
            activityId,
            new ActivityDetailsSpecification(
                request.ActivityDetails.Title,
                request.ActivityDetails.Note));
}
