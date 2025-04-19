using Bogus;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.domain.Enums;

namespace discipline.centre.activityrules.tests.sharedkernel.Application;

public static class CreateActivityRuleDtoFakeDataFactory
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
        var days = Enum.GetValues<DayOfWeek>();
        
        dto with {}  
    } 
}