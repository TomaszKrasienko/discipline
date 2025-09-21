namespace discipline.activity_scheduler.shared.abstractions.Exceptions;

public sealed class ParameterNullException(string code) : DisciplineException(code);