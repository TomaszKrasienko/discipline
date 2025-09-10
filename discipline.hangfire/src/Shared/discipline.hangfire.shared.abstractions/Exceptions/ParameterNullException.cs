namespace discipline.hangfire.shared.abstractions.Exceptions;

public sealed class ParameterNullException(string code) : DisciplineException(code);