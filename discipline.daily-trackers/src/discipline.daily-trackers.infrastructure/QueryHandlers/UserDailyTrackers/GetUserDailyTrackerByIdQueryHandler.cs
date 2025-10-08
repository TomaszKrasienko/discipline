using discipline.daily_trackers.application.UserDailyTrackers.DTOs.Responses;
using discipline.daily_trackers.application.UserDailyTrackers.Queries;
using discipline.daily_trackers.infrastructure.DAL;
using discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.libs.cqrs.abstractions.Queries;
using MongoDB.Driver;

namespace discipline.daily_trackers.infrastructure.QueryHandlers.UserDailyTrackers;

internal sealed class GetUserDailyTrackerByIdQueryHandler(
    DailyTrackerMongoContext context) : IQueryHandler<GetUserDailyTrackerByIdQuery, UserDailyTrackerResponseDto?>
{
    public async Task<UserDailyTrackerResponseDto?> HandleAsync(GetUserDailyTrackerByIdQuery query, CancellationToken cancellationToken = default)
    {
        var dailyTrackerDocument = await context
            .GetCollection<UserDailyTrackerDocument>()
            .Find(x
                => x.AccountId == query.AccountId.ToString() && 
                   x.Id == query.UserDailyTrackerId.ToString())
            .SingleOrDefaultAsync(cancellationToken);
        
        return dailyTrackerDocument?.ToResponseDto();
    }
}