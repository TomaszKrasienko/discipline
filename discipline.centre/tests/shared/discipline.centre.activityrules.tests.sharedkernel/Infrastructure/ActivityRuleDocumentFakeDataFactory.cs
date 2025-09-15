using Bogus;
using discipline.centre.activity_rules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activity_rules.tests.shared_kernel.Infrastructure;

internal static class ActivityRuleDocumentFakeDataFactory
{
    internal static ActivityRuleDocument Get(bool withNote = false,
        HashSet<int>? selectedDays = null)
    {
        var modesWithoutDays = RuleMode.AvailableModes.Where(x
            => !x.IsDaysRequired);
        var faker = new Faker();

        var mode = selectedDays is null ? faker.PickRandom(modesWithoutDays) : RuleMode.Custom;

        return new ActivityRuleDocument()
        {
            Id = ActivityRuleId.New().ToString(),
            AccountId = UserId.New().ToString(),
            Details = new ActivityRuleDetailsDocument(faker.Random.String2(length: 10),
                withNote ? faker.Lorem.Word() : null),
            SelectedMode = new ActivityRuleSelectedModeDocument(mode.Value, selectedDays),
            Stages = []
        };
    }

    internal static ActivityRuleDocument WithStage(this ActivityRuleDocument document)
    {
        var faker = new Faker();
        var stages = document.Stages.ToList(); 
        stages.Add(new StageDocument(StageId.New().ToString(), faker.Random.String2(10), stages.Count + 1));
        return document with { Stages = stages };
    }
}