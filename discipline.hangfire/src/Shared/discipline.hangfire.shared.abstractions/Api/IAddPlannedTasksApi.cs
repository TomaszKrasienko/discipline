using discipline.hangfire.shared.abstractions.ViewModels;

namespace discipline.hangfire.shared.abstractions.Api;

public interface IAddPlannedTasksApi
{
    Task ExecuteTaskPlanning(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<PlannedTaskViewModel>> GetPlannedTask(DateOnly date, CancellationToken cancellationToken);
    Task MarkAsPlanned(Ulid plannedTaskId, CancellationToken cancellationToken);
}