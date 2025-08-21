using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.calendar.domain.Repositories;

public interface IReadUserCalendarRepository
{
    Task<UserCalendarDay?> GetByDayAsync(AccountId accountId, Day day, CancellationToken cancellationToken);
}