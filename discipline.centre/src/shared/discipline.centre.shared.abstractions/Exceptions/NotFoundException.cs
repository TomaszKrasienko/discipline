using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.abstractions.Exceptions;

public sealed class NotFoundException(string code, params object[] args)
    : DisciplineException(code, args)
{
}