using Bogus;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.domain.Enums;

namespace discipline.centre.activityrules.tests.sharedkernel.Application;

public static class ActivityRuleRequestDtoFakeDataFactory
{
    public static ActivityRuleRequestDto Get(bool withNote = false)
    {
        var noDayRuleModes = RuleMode.AvailableModes
            .Where(x => !x.IsDaysRequired)
            .Select(x => x.Value);
        
        var faker = new Faker<ActivityRuleRequestDto>()
            .CustomInstantiator(v => new ActivityRuleRequestDto(
                new ActivityRuleDetailsRequestDto(v.Lorem.Word(), withNote ? v.Lorem.Sentence() : null),
                new ActivityRuleModeRequestDto(v.PickRandom(noDayRuleModes), null)));

        return faker.Generate();
    }

    public static ActivityRuleRequestDto WithCustomMode(this ActivityRuleRequestDto dto)
    {
        var days = Enum.GetValues<DayOfWeek>().Select(x => (int)x).ToList();
        var faker = new Faker();

        var numberOfDays = faker.Random.Int(days.Count);
        var selectedDays = faker.PickRandom(days, numberOfDays).ToList();

        return dto with { Mode = new ActivityRuleModeRequestDto(RuleMode.Custom.Value, selectedDays) };
    } 
}