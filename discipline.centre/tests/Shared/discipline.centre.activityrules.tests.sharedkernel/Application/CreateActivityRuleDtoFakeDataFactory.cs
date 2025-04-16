using Bogus;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Create;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;

namespace discipline.centre.activityrules.tests.sharedkernel.Application;

public static class CreateActivityRuleDtoFakeDataFactory
{
    public static ActivityRuleRequestDto Get()
    {
        //todo: add stages
        var mode = new Faker()
            .PickRandom<string>(SelectedMode.AvailableModes.Keys);

        var random = new Random();
        var selectedDaysCount = random.Next(1, 6);
        List<int> days = [];
        for (int i = 0; i < selectedDaysCount; i++)
        {
            days.Add(i);
        }

        var faker = new Faker<ActivityRuleRequestDto>()
            .CustomInstantiator(v => new ActivityRuleRequestDto(
                new ActivityRuleDetailsSpecification(v.Lorem.Word(), v.Lorem.Word()),
                mode,
                mode == SelectedMode.CustomMode ? days : null,
                null));

        return faker.Generate(1).Single();
    }
}