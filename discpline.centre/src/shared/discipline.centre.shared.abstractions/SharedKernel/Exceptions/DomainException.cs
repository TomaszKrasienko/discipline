namespace discipline.centre.shared.abstractions.SharedKernel.Exceptions;

public sealed class DomainException(
    string code, params object[]? args) : DisciplineException(code, args);