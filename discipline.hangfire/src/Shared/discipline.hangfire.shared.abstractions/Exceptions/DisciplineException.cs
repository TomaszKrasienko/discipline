namespace discipline.hangfire.shared.abstractions.Exceptions;

public abstract class DisciplineException(string code, params string[] parameters) : Exception;