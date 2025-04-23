using Bogus;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.tests.sharedkernel.Domain;

public static class ActivityRuleFakeDataFactory
{
    public static ActivityRule Get(bool withNote = false,
        HashSet<int>? selectedDays = null)
    {
        var modesWithoutDays = RuleMode.AvailableModes.Where(x
            => !x.IsDaysRequired);
        var faker = new Faker();

        var mode = selectedDays is null ? faker.PickRandom(modesWithoutDays) : RuleMode.Custom;

        return new ActivityRule(ActivityRuleId.New(), UserId.New(),
            Details.Create(faker.Random.String2(length: 10), withNote ? faker.Lorem.Word() : null),
            SelectedMode.Create(mode, selectedDays), []);
    }

    public static ActivityRule WithStage(this ActivityRule activityRule)
    {
        var faker = new Faker();

        activityRule.AddStage(faker.Random.String2(10));
        return activityRule;
    }
}