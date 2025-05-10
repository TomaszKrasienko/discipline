using Bogus;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Stages;

namespace discipline.centre.activityrules.tests.sharedkernel.Application;

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