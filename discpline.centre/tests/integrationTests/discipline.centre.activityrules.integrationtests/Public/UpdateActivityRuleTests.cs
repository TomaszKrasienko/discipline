// using System.Net;
// using System.Net.Http.Json;
// using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests;
// using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
// using discipline.centre.activityrules.domain;
// using discipline.centre.activityrules.domain.Enums;
// using discipline.centre.activityrules.infrastructure.DAL.Documents;
// using discipline.centre.activityrules.tests.sharedkernel.Domain;
// using discipline.centre.integrationTests.sharedKernel;
// using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
// using MongoDB.Driver;
// using Shouldly;
// using Xunit;
//
// namespace discipline.centre.activityrules.integrationTests.Public;
//
// [Collection("activity-rules-module-edit-activity-rule")]
// public sealed class UpdateActivityRuleTests() : BaseTestsController("activity-rules-module")
// {
//     [Fact]
//     public async Task Update_GivenExistingActivityRuleWithValidArguments_ShouldReturn204NoContentStatusCodeAndUpdateActivityRule()
//     {
//         // Arrange
//         var activityRule = ActivityRuleFakeDataFactory.Get();
//         var user = await AuthorizeWithFreeSubscriptionPicked();
//         var activityRuleDocument = activityRule.AsDocument();
//
//         await TestAppDb.GetCollection<ActivityRuleDocument>()
//             .InsertOneAsync(activityRuleDocument with {UserId = user.Id.ToString()});
//
//         var request = new UpdateActivityRuleDto(new ActivityRuleDetailsRequestDto("new_test_title", "new_test_note"),
//             new ActivityRuleModeRequestDto(RuleMode.Custom.Value, [0]));
//
//         // Act
//         var response = await HttpClient.PutAsJsonAsync($"api/activity-rules-module/activity-rules/{activityRule.Id.ToString()}", request);
//         
//         // Assert
//         response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
//         
//         var updatedActivityRuleDocument = await TestAppDb
//             .GetCollection<ActivityRuleDocument>()
//             .Find(x => x.Id.ToString() == activityRule.Id.ToString())
//             .SingleOrDefaultAsync(); 
//         updatedActivityRuleDocument.Details.Title.ShouldBe(request.Details.Title);
//         updatedActivityRuleDocument.Details.Note.ShouldBe(request.Details.Note);
//         updatedActivityRuleDocument.SelectedMode.Mode.ShouldBe(request.Mode.Mode);
//         updatedActivityRuleDocument.SelectedMode.DaysOfWeek!.First().ShouldBe(request.Mode.Days![0]);
//     }
//     
//     [Fact]
//     public async Task Update_GivenExistingActivityRuleWithInvalidArguments_ShouldReturn400BadRequestStatusCode()
//     {
//         // Arrange
//         var activityRule = ActivityRuleFakeDataFactory.Get();
//         await TestAppDb.GetCollection<ActivityRuleDocument>()
//             .InsertOneAsync(activityRule.AsDocument());
//
//         await AuthorizeWithFreeSubscriptionPicked();
//         var request = new UpdateActivityRuleDto(new ActivityRuleDetailsRequestDto("new_test_title", null),
//             new ActivityRuleModeRequestDto("test", null));
//
//         // Act
//         var response = await HttpClient.PutAsJsonAsync($"api/activity-rules-module/activity-rules/{activityRule.Id.ToString()}", request);
//         
//         // Assert
//         response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
//     }
//     
//     [Fact]
//     public async Task Update_GivenInvalidCommand_ShouldReturn422UnprocessableEntityStatusCode()
//     {
//         // Arrange
//         var request = new UpdateActivityRuleDto(new ActivityRuleDetailsRequestDto(string.Empty, "new_test_note"),
//             new ActivityRuleModeRequestDto(RuleMode.Custom.Value, [0]));
//         await AuthorizeWithFreeSubscriptionPicked();
//         
//         // Act
//         var response = await HttpClient.PutAsJsonAsync($"api/activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}", request);
//         
//         // Assert
//         response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
//     }
//     
//     [Fact]
//     public async Task Update_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
//     {
//         // Arrange
//         var request = new UpdateActivityRuleDto(new ActivityRuleDetailsRequestDto("test_title", null),
//             new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
//
//         // Act
//         var response = await HttpClient.PutAsJsonAsync($"api/activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}", request);
//         
//         // Assert
//         response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
//     }
//     
//     [Fact]
//     public async Task Update_AuthorizedByUserWithoutSubscription_ShouldReturn403ForbiddenStatusCode()
//     {
//         // Arrange
//         var request = new UpdateActivityRuleDto(new ActivityRuleDetailsRequestDto("test_title", null),
//             new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
//         await AuthorizeWithoutSubscription();
//         
//         // Act
//         var response = await HttpClient.PutAsJsonAsync($"api/activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}", request);
//         
//         // Assert
//         response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
//     }
// }