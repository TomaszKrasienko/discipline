using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.abstractions.Exceptions;

public sealed class NotUniqueException(string code, string param)
    : DisciplineException(code, param);