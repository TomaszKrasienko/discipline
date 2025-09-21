using Bogus;
using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.tests.shared_kernel.Domain;

public static class UserDailyTrackerFakeDataFactory
{
    public static UserDailyTracker Get()    
    {
        var faker = new Faker<UserDailyTracker>()
            .CustomInstantiator(x => UserDailyTracker.Create(
                DailyTrackerId.New(), 
                AccountId.New(),
                x.Date.RecentDateOnly(),
                DailyTrackerId.New()
            ));

        return faker.Generate();
    }
}