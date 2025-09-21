using Bogus;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Stages;

namespace discipline.centre.activity_rules.tests.shared_kernel.Application;

public static class CreateStageRequestDtoFakeDataFactory
{
    public static CreateStageRequestDto Get(int? index = null)
    {
        var faker = new Faker<CreateStageRequestDto>()
            .CustomInstantiator(v => new CreateStageRequestDto(
                v.Lorem.Word(),
                index ?? v.Random.Int()));
        
        return faker.Generate();
    }
}