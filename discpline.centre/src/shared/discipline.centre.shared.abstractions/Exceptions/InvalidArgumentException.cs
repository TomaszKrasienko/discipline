using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.abstractions.Exceptions;

public sealed class InvalidArgumentException(string code)
    : DisciplineException(code)
{
}