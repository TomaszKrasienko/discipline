using Bogus;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.domain.Enums;

namespace discipline.centre.activityrules.tests.sharedkernel.Application;

public static class UpdateActivityRuleDtoFakeDataFactory
{
    public static UpdateActivityRuleDto Get(bool withNote = false)
    {
        var noDayRuleModes = RuleMode.AvailableModes
            .Where(x => !x.IsDaysRequired)
            .Select(x => x.Value);
        
        var faker = new Faker<UpdateActivityRuleDto>()
            .CustomInstantiator(v => new UpdateActivityRuleDto(
                new ActivityRuleDetailsRequestDto(v.Lorem.Word(), withNote ? v.Lorem.Sentence() : null),
                new ActivityRuleModeRequestDto(v.PickRandom(noDayRuleModes), null)));

        return faker.Generate();
    }
    
    public static UpdateActivityRuleDto WithCustomMode(this UpdateActivityRuleDto dto)
    {
        var days = Enum.GetValues<DayOfWeek>().Select(x => (int)x).ToList();
        var faker = new Faker();

        var numberOfDays = faker.Random.Int(days.Count);
        var selectedDays = faker.PickRandom(days, numberOfDays).ToList();

        return dto with { Mode = new ActivityRuleModeRequestDto(RuleMode.Custom.Value, selectedDays) };
    } 
}