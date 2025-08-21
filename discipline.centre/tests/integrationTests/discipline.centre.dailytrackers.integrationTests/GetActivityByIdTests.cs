// using System.Net;
// using discipline.centre.dailytrackers.application.DailyTrackers.DTOs.Responses;
// using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
// using discipline.centre.integrationTests.sharedKernel;
// using discipline.centre.integrationTests.sharedKernel.Serialization;
// using Shouldly;
// using Xunit;
//
// namespace discipline.centre.dailytrackers.integrationTests;
//
// [Collection("daily-trackers-module-get-by-id")]
// public sealed class GetActivityByIdTests() : BaseTestsController("daily-trackers-module")
// {
//     [Fact]
//     public async Task GivenExistingActivityId_ShouldReturn200OkStatusCodeWithActivityDto()
//     {
//         //arrange
//         var user = await AuthorizeWithFreeSubscriptionPicked();
//         
//         var dailyTracker = new DailyTrackerDocument
//         {
//             DailyTrackerId = Ulid.NewUlid().ToString(),
//             Day = DateOnly.FromDateTime(DateTime.Now),
//             UserId = user.Id.ToString(),
//             Activities = [new ActivityDocument
//             {
//                 ActivityId = Ulid.NewUlid().ToString(),
//                 IsChecked = false,
//                 Title = "test_activity",
//                 Stages = [
//                     new StageDocument
//                     {
//                         StageId = Ulid.NewUlid().ToString(),
//                         Title = "test_stage",
//                         Index = 1
//                     }
//                 ]
//             }]
//         };
//         
//         await TestAppDb.GetCollection<DailyTrackerDocument>().InsertOneAsync(dailyTracker);
//         
//         //act
//         var response = await HttpClient.GetAsync($"api/daily-trackers-module/daily-trackers/activities/{dailyTracker.Activities.First().ActivityId.ToString()}");
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.OK);
//
//         var result = await response.Content.ReadAsStringAsync();
//         var activity = SerializerForTests.Deserialize<ActivityDto>(result);
//         activity!.ActivityId.ShouldBe(dailyTracker.Activities.First().ActivityId);
//     }
//
//     [Fact]
//     public async Task GivenNotExistingActivityId_ShouldReturn404NotFound()
//     {
//         //arrange
//         _ = await AuthorizeWithFreeSubscriptionPicked();
//         
//         //act
//         var response = await HttpClient.GetAsync($"api/daily-trackers-module/daily-trackers/activities/{Ulid.NewUlid().ToString()}");
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
//     }
//
//     [Fact]
//     public async Task Unauthorized_ShouldReturn401Unauthorized()
//     {
//         //act
//         var response = await HttpClient.GetAsync($"api/daily-trackers-module/daily-trackers/activities/{Ulid.NewUlid().ToString()}");
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
//     }
//     
//     [Fact]
//     public async Task AuthorizedByUserWithStatusCreated_ShouldReturn403Forbidden()
//     {
//         //arrange
//         _ = await AuthorizeWithoutSubscription();
//         
//         //act
//         var response = await HttpClient.GetAsync($"api/daily-trackers-module/daily-trackers/activities/{Ulid.NewUlid().ToString()}");
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
//     }
// }