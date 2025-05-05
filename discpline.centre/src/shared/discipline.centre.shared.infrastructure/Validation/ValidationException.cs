using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.infrastructure.Validation;

public sealed class ValidationException(IReadOnlyDictionary<string, string> errors) 
    : DisciplineException("ValidationException")
{
    public IReadOnlyDictionary<string, string> Errors => errors;
}