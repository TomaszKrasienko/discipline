using discipline.centre.activity_rules.tests.shared_kernel.DataValidators;
using discipline.centre.activity_rules.tests.shared_kernel.Domain;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unittests;

public sealed class ActivityRuleTests
{
    #region Create
    
    [Fact]
    public void GivenValidArgumentsForModeWithoutDays_WhenCreate_ThenReturnActivityRuleWithValues()
    {
        // Arrange
        var activityRuleId = ActivityRuleId.New();
        var accountId = AccountId.New();
        var details = new ActivityRuleDetailsSpecification(
            "test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(
            RuleMode.EveryDay,
            null);
        
        // Act
        var result = ActivityRule.Create(
            activityRuleId,
            accountId,
            details,
            mode);
        
        // Assert
        result.Id.ShouldBe(activityRuleId);
        result.AccountId.ShouldBe(accountId);
        result.Details.Title.ShouldBe(details.Title);
        result.Details.Note.ShouldBe(details.Note);
        result.Mode.Mode.ShouldBe(mode.Mode);
        result.Mode.Days.ShouldBeNull();
    }

    [Fact]
    public void GivenValidArgumentsForModeWithDays_WhenCreate_ThenReturnActivityRuleWithValues()
    {        
        // Arrange
        var activityRuleId = ActivityRuleId.New();
        var accountId = AccountId.New();
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.Custom, [1,2,3]);
        
        // Act
        var result = ActivityRule.Create(
            activityRuleId,
            accountId,
            details,
            mode);
        
        // Assert
        result.Id.ShouldBe(activityRuleId);
        result.AccountId.ShouldBe(accountId);
        result.Details.Title.ShouldBe(details.Title);
        result.Details.Note.ShouldBe(details.Note);
        result.Mode.Mode.ShouldBe(mode.Mode);
        result.Mode.Days.IsEqual(mode.Days).ShouldBeTrue();
    }

    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleDetailsEmptyTitle()
    {
        var (activityId, accountId, details, mode) = GetFilledParamsForCreate(); 
        
        // Act
        var exception = Record.Exception(() => ActivityRule.Create(
            activityId,
            accountId, 
            details with {Title = string.Empty},
            mode));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.EmptyTitle");
    }

    [Fact]
    public void GivenTitleLongerThan30Characters_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleDetailsTitleTooLonge()
    {
        var (activityId, accountId, details, mode) = GetFilledParamsForCreate(); 
        
        // Act
        var exception = Record.Exception(() => ActivityRule.Create(
            activityId,
            accountId, 
            details with {Title = new string('t', 31)},
            mode));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.TitleTooLong");
    }

    [Fact]
    public void GivenModeWithRequiredDaysAndNullDays_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRulesModeRuleModeRequireSelectedDays()
    {
        // Arrange
        var (activityId, accountId, details, _) = GetFilledParamsForCreate();
        
        // Act
        var exception = Record.Exception(() => ActivityRule.Create(
            activityId,
            accountId,
            details,
            new ActivityRuleModeSpecification(Mode: RuleMode.Custom, Days: null)));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRules.Mode.RuleModeRequireSelectedDays");
    }

    [Fact]
    public void GivenSelectedDaysOutOfRange_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleModeSelectedDayOutOfRange()
    {
        // Arrange
        var  (activityId, accountId, details, _) = GetFilledParamsForCreate();
        
        // Act
        var exception = Record.Exception(() => ActivityRule.Create(
            activityId,
            accountId,
            details,
            new ActivityRuleModeSpecification(Mode: RuleMode.Custom, Days: [8])));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRules.Mode.RuleModeSelectedDayOutOfRange");
    }

    private static ActivityRuleCreateTestsParams GetFilledParamsForCreate()
    {
        var activityRuleId = ActivityRuleId.New();
        var accountId = AccountId.New();
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.Custom, [1,2,3]);
        return new ActivityRuleCreateTestsParams(activityRuleId, accountId, details, mode);
    }
    
    #endregion
    #region AddStage
    
    [Fact]
    public void GivenUniqueTitle_WhenAddStage_ThenAddsStageWithValidIndex()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        activityRule.AddStage(StageId.New(), "test_stage_1");
        var stageId = StageId.New();
        const string title = "test_stage_title2";
        
        // Act
        activityRule.AddStage(stageId, title);
        
        // Assert
        var stage = activityRule.Stages.SingleOrDefault(x 
            => x.Id == stageId 
               && x.Title == title);
        stage.ShouldNotBeNull();
        stage.Index.Value.ShouldBe(2);
    }

    [Fact]
    public void GivenFirstStage_WhenAddStage_ThenAddsStageWithFirstIndex()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var id = StageId.New();
        const string title = "test_stage_title";
        
        // Act
        activityRule.AddStage(id, title);
        
        // Assert
        var stage = activityRule.Stages.SingleOrDefault(x 
            => x.Id == id
               && x.Title == title);
        stage.ShouldNotBeNull();
        stage.Index.Value.ShouldBe(1);
    }

    [Fact]
    public void GivenNotUniqueTitle_WhenAddStage_ThenThrowsDomainExceptionWithCodeActivityRuleStagesStageTitleMustBeUnique()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        const string title = "test_stage_title";
        activityRule.AddStage(StageId.New(), title);
        
        // Act
        var exception = Record.Exception(() => activityRule.AddStage(StageId.New(), title));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stages.StageTitleMustBeUnique");
    }
    
    #endregion
    #region Update
    
    [Fact]
    public void GivenValidArgumentsForModeWithoutDays_WhenUpdate_ThenReturnActivityRuleWithValues()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.EveryDay, null);
        
        // Act
        activityRule.Update(details, mode);
        
        // Assert
        activityRule.Details.Title.ShouldBe(details.Title);
        activityRule.Details.Note.ShouldBe(details.Note);
        activityRule.Mode.Mode.ShouldBe(mode.Mode);
        activityRule.Mode.Days.ShouldBeNull();
    }

    [Fact]
    public void GivenValidArgumentsForModeWithDays_WhenUpdate_ThenReturnActivityRuleWithValues()
    {        
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.Custom, [1,2,3]);
        
        // Act
        activityRule.Update(details, mode);
        
        // Assert
        activityRule.Details.Title.ShouldBe(details.Title);
        activityRule.Details.Note.ShouldBe(details.Note);
        activityRule.Mode.Mode.ShouldBe(mode.Mode);
        activityRule.Mode.Days.IsEqual(mode.Days).ShouldBeTrue();
    }

    [Fact]
    public void GivenEmptyTitle_WhenUpdate_ThenThrowDomainExceptionWithCodeActivityRuleDetailsEmptyTitle()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var (details, mode) = GetFilledParamsForUpdate(); 
        
        // Act
        var exception = Record.Exception(() => activityRule.Update(details with {Title = string.Empty}, mode));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.EmptyTitle");
    }

    [Fact]
    public void GivenTitleLongerThan30Characters_WhenUpdate_ThenThrowDomainExceptionWithCodeActivityRuleDetailsTitleTooLonge()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var (details, mode) = GetFilledParamsForUpdate();
        
        // Act
        var exception = Record.Exception(() => activityRule.Update(details with {Title = new string('t', 31)}, mode));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.TitleTooLong");
    }

    [Fact]
    public void GivenModeWithRequiredDaysAndNullDays_WhenUpdate_ThenThrowDomainExceptionWithCodeActivityRulesModeRuleModeRequireSelectedDays()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var (details, mode) = GetFilledParamsForUpdate();
        
        // Act
        var exception = Record.Exception(() => activityRule.Update(details, 
            new ActivityRuleModeSpecification(Mode: RuleMode.Custom, Days: null)));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRules.Mode.RuleModeRequireSelectedDays");
    }

    [Fact]
    public void GivenSelectedDaysOutOfRange_WhenUpdate_ThenThrowDomainExceptionWithCodeActivityRuleModeSelectedDayOutOfRange()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var (details, mode) = GetFilledParamsForUpdate();
        
        // Act
        var exception = Record.Exception(() => activityRule.Update(details, new ActivityRuleModeSpecification(Mode: RuleMode.Custom, Days: [8])));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRules.Mode.RuleModeSelectedDayOutOfRange");
    }

    private static ActivityRuleEditTestsParams GetFilledParamsForUpdate()
    {
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.Custom, [1,2,3]);
        return new ActivityRuleEditTestsParams(details, mode);
    }
    
    #endregion
    #region RemoveStage
    
    [Fact]
    public void GivenExistingStage_WhenRemoveStage_ThenRemovesStage()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get()
            .WithStage();

        var stage = activityRule.Stages.Single();
        
        // Act
        activityRule.RemoveStage(stage.Id);
        
        // Assert
        activityRule.Stages.Any(x => x.Id == stage.Id).ShouldBeFalse();
    }

    [Fact]
    public void GivenExistingStage_WhenRemoveStage_ThenShouldSetNewIndexOnExistingStages()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get()
            .WithStage()
            .WithStage()
            .WithStage();
        
        var stage = activityRule.Stages.First();
        
        // Act
        activityRule.RemoveStage(stage.Id);
        
        // Assert
        var existingStages = activityRule.Stages
            .OrderBy(x => x.Index.Value)
            .ToList();
        
        existingStages[0].Index.Value.ShouldBe(1);
        existingStages[1].Index.Value.ShouldBe(2);
    }
    
    #endregion
}

public sealed record ActivityRuleCreateTestsParams(
    ActivityRuleId ActivityRuleId,
    AccountId AccountId,
    ActivityRuleDetailsSpecification Details,
    ActivityRuleModeSpecification Mode);
    
public record ActivityRuleEditTestsParams(
    ActivityRuleDetailsSpecification Details,
    ActivityRuleModeSpecification Mode);