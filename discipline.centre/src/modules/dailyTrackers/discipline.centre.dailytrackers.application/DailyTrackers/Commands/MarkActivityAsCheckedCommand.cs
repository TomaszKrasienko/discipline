using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;

public sealed record MarkActivityAsCheckedCommand(AccountId AccountId, DailyTrackerId DailyTrackerId,
    ActivityId ActivityId) : ICommand;
    
internal sealed class MarkActivityAsCheckedCommandHandler(
    IReadWriteDailyTrackerRepository writeReadDailyTrackerRepository) : ICommandHandler<MarkActivityAsCheckedCommand>
{
    public async Task HandleAsync(MarkActivityAsCheckedCommand command, CancellationToken cancellationToken = default)
    {
        var dailyTracker = await writeReadDailyTrackerRepository.GetDailyTrackerByIdAsync(command.AccountId, command.DailyTrackerId, 
            cancellationToken);

        if (dailyTracker is null)
        {
            throw new NotFoundException("DailyTracker.NotFound", nameof(DailyTracker),
                command.DailyTrackerId.ToString(), command.AccountId.ToString());
        }
        
        dailyTracker.MarkActivityAsChecked(command.ActivityId);
        await writeReadDailyTrackerRepository.UpdateAsync(dailyTracker, cancellationToken);
    }
}