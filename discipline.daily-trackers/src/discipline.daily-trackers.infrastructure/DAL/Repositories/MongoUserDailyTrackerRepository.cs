using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Mappers;
using MongoDB.Driver;

namespace discipline.daily_trackers.infrastructure.DAL.Repositories;

internal sealed class MongoUserDailyTrackerRepository(
    DailyTrackerMongoContext context) : IReadWriteUserDailyTrackerRepository
{
    public async Task<UserDailyTracker?> GetByDayAsync(AccountId accountId, Day day,
        CancellationToken cancellationToken = default)
        => (await context
            .GetCollection<UserDailyTrackerDocument>()
            .Find(x
                => x.Day == day &&
                   x.AccountId == accountId.ToString())
            .SingleOrDefaultAsync(cancellationToken))?.ToEntity();

    public Task<bool> DoesExistAsync(AccountId accountId, Day day, CancellationToken cancellationToken = default)
        => context
            .GetCollection<UserDailyTrackerDocument>()
            .Find(x
                => x.AccountId == accountId.ToString() &&
                   x.Day == day)
            .AnyAsync(cancellationToken);

    public Task AddAsync(UserDailyTracker dailyTracker, CancellationToken cancellationToken = default)
        => context
            .GetCollection<UserDailyTrackerDocument>()
            .InsertOneAsync(dailyTracker.ToDocument(),  cancellationToken: cancellationToken);

    public Task UpdateAsync(UserDailyTracker dailyTracker, CancellationToken cancellationToken = default)
        => context
            .GetCollection<UserDailyTrackerDocument>()
            .FindOneAndReplaceAsync(x 
                    => x.Id == dailyTracker.Id.ToString() &&
                       x.AccountId ==  dailyTracker.AccountId.ToString(), 
                dailyTracker.ToDocument(),
                cancellationToken: cancellationToken);
}