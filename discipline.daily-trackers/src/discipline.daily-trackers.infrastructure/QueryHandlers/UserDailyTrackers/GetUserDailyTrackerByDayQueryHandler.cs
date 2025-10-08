using discipline.daily_trackers.application.UserDailyTrackers.DTOs.Responses;
using discipline.daily_trackers.application.UserDailyTrackers.Queries;
using discipline.daily_trackers.infrastructure.DAL;
using discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.libs.cqrs.abstractions.Queries;
using MongoDB.Driver;

namespace discipline.daily_trackers.infrastructure.QueryHandlers.UserDailyTrackers;

internal sealed class GetUserDailyTrackerByDayQueryHandler(
    DailyTrackerMongoContext context) : IQueryHandler<GetUserDailyTrackerByDayQuery, UserDailyTrackerResponseDto?>
{
    public async Task<UserDailyTrackerResponseDto?> HandleAsync(GetUserDailyTrackerByDayQuery query, CancellationToken cancellationToken = default)
    {
        var dailyTrackerDocument = await context
            .GetCollection<UserDailyTrackerDocument>()
            .Find(x
                => x.AccountId == query.AccountId.ToString() &&
                   x.Day == query.Day)
            .SingleOrDefaultAsync(cancellationToken);
        
        return dailyTrackerDocument?.ToResponseDto();
    }
}