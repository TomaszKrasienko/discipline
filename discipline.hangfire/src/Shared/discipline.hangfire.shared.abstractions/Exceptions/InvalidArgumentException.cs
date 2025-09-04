namespace discipline.hangfire.shared.abstractions.Exceptions;

public sealed class InvalidArgumentException(string code, params string[] parameters) : DisciplineException(code);