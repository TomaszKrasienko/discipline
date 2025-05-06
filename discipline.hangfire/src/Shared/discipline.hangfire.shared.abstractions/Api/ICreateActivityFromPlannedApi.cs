namespace discipline.hangfire.shared.abstractions.Api;

public interface ICreateActivityFromPlannedApi
{
    Task ExecuteTaskCreating(CancellationToken cancellationToken);
}