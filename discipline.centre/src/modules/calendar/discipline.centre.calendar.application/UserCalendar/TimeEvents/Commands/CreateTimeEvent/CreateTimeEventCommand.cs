using discipline.centre.calendar.domain;
using discipline.centre.calendar.domain.Repositories;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.centre.calendar.application.UserCalendar.TimeEvents.Commands.CreateTimeEvent;

public sealed record CreateTimeEventCommand(AccountId AccountId, 
    DateOnly Day,
    CalendarEventId EventId, 
    string Title, 
    string? Description,
    TimeOnly TimeFrom,
    TimeOnly? TimeTo) : ICommand;
    
internal sealed class CreateTimeEventCommandHandler(
    IReadWriteUserCalendarRepository readWriteUserCalendarRepository) : ICommandHandler<CreateTimeEventCommand>
{
    public async Task HandleAsync(CreateTimeEventCommand command, CancellationToken cancellationToken)
    {
        var userCalendarDay = await readWriteUserCalendarRepository.GetByDayAsync(command.AccountId, command.Day, cancellationToken);

        if (userCalendarDay is null)
        {
            userCalendarDay = UserCalendarDay.CreateWithTimeEvent(UserCalendarId.New(), command.AccountId, command.Day,
                command.EventId, command.Title, command.Description, command.TimeFrom, command.TimeTo);
            
            await readWriteUserCalendarRepository.AddAsync(userCalendarDay, cancellationToken);
        }
        else
        {
            userCalendarDay.AddTimeEvent(command.EventId, command.Title, command.Description, command.TimeFrom, command.TimeTo);
            
            await readWriteUserCalendarRepository.UpdateAsync(userCalendarDay, cancellationToken);
        }
    }
}