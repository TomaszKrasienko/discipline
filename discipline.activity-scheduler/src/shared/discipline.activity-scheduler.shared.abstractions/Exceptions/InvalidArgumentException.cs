namespace discipline.activity_scheduler.shared.abstractions.Exceptions;

public sealed class InvalidArgumentException(
    string code,
    params string[] parameters) : DisciplineException(code, parameters);