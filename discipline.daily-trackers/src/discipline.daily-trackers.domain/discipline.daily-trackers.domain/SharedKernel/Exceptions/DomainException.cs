namespace discipline.daily_trackers.domain.SharedKernel.Exceptions;

public sealed class DomainException(
    string code, params object[]? args) : DisciplineException(code, args);