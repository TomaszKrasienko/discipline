using Bogus;
using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.tests.shared_kernel.Domain;

public static class ActivityFakeDataFactory
{
    public static Activity Get(
        bool withNote = false,
        bool withParent = false)
    {
        var faker = new Faker<Activity>().CustomInstantiator(x => 
            Activity.Create(
                ActivityId.New(),
                new ActivityDetailsSpecification(
                    x.Random.String(minLength: 3, maxLength: 30, minChar:'a', maxChar:'z'), 
                    withNote ? x.Lorem.Word() : null)));

        return faker.Generate();
    }

    public static Activity WithStages(
        this Activity activity,
        List<Stage>? stages = null)
    {
        var faker = new Faker();
        activity.AddStage(
            StageId.New(), 
            faker.Random.String(minLength: 3, maxLength: 30, minChar: 'a', maxChar: 'z'));
        
        return activity;
    }
}