namespace discipline.hangfire.shared.abstractions.Exceptions;

public abstract class DisciplineException(string code, params string[] parameters) : Exception
{
    public string Code { get; } = code;
    public string[]? Parameters { get; } = parameters;
}