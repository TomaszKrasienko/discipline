// using discipline.hangfire.create_activity_from_planned.Commands;
// using discipline.hangfire.shared.abstractions.Api;
//
// namespace discipline.hangfire.create_activity_from_planned;
//
// internal sealed class CreateActivityFromPlannedApi(
//     IAddPlannedTasksApi addPlannedTasksApi,
//     TimeProvider timeProvider) : ICreateActivityFromPlannedApi
// {
//     public async Task ExecuteTaskCreating(CancellationToken cancellationToken)
//     {
//         var plannedTasks = await addPlannedTasksApi
//             .GetPlannedTask(DateOnly.FromDateTime(timeProvider.GetUtcNow().Date), cancellationToken);
//
//         foreach (var plannedTask in plannedTasks)
//         {
//             var command = new CreateActivityFromActivityRuleCommand(plannedTask.ActivityRuleId, plannedTask.AccountId);
//
//             //TODO: Outbox pattern
//             
//             // List<Task> creatingTasks =
//             // [
//             //     brokerPublisher.SendAsync(command, cancellationToken),
//             //     addPlannedTasksApi.MarkAsPlanned(plannedTask.Id, cancellationToken)
//             // ];
//
//             // await Task.WhenAll(creatingTasks);
//         }
//     }
// }