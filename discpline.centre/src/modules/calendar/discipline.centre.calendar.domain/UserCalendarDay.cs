using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.calendar.domain;

public sealed class UserCalendarDay : AggregateRoot<UserCalendarId, Ulid>
{
    private readonly HashSet<BaseCalendarEvent> _events = new();
    public AccountId AccountId { get; }
    public Day Day { get; }
    public IReadOnlySet<BaseCalendarEvent> Events => _events;

    private UserCalendarDay(UserCalendarId id, AccountId accountId, Day day) : base(id)
    {
        AccountId = accountId;
        Day = day;
    }

    /// <summary>
    /// Use only for Mongo purpose!
    /// </summary>
    public UserCalendarDay(UserCalendarId id, AccountId accountId, Day day, HashSet<BaseCalendarEvent> events)
        : this(id, accountId, day)
    {
        _events = events;
    }

    public static UserCalendarDay CreateWithImportantDate(UserCalendarId id,
        AccountId accountId, 
        DateOnly day,
        CalendarEventId eventId,
        string title, 
        string? description)
    {
        var userCalendarDay = new UserCalendarDay(id, accountId, day);
        userCalendarDay._events.Add(ImportantDateEvent.Create(eventId, title, description));
        return userCalendarDay;
    }

    public static UserCalendarDay CreateWithTimeEvent(UserCalendarId id,
        AccountId accountId,
        DateOnly day,
        CalendarEventId eventId,
        string title,
        string? description,
        TimeOnly timeFrom,
        TimeOnly? timeTo)
    {
        var userCalendarDay = new UserCalendarDay(id, accountId, day);
        userCalendarDay._events.Add(TimeEvent.Create(eventId, timeFrom, timeTo, title, description));
        return userCalendarDay;
    }

    public void AddImportantDate(CalendarEventId eventId,
        string title,
        string? description)
    {
        if (_events.Any(x => x is ImportantDateEvent && x.Content.Title == title))
        {
            throw new DomainException("UserCalendarDay.ImportantDateEvent.AlreadyRegistered",
                $"Important date event with title {title} already registered on: {Day.Value}");
        }
        
        _events.Add(ImportantDateEvent.Create(eventId, title, description));
    }

    public void AddTimeEvent(CalendarEventId eventId,
        string title,
        string? description,
        TimeOnly timeFrom,
        TimeOnly? timeTo)
    {
        if (_events.Any(x => x is TimeEvent && x.Content.Title == title))
        {
            throw new DomainException("UserCalendarDay.TimeEvent.AlreadyRegistered",
                $"Time event with title {title} already registered on: {Day.Value}");
        }
        
        _events.Add(TimeEvent.Create(eventId, timeFrom, timeTo, title, description));
    }
}