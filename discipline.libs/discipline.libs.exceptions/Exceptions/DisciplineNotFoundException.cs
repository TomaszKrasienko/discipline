namespace discipline.libs.exceptions.Exceptions;

public sealed class DisciplineNotFoundException(
    string source,
    string model,
    params string[] parameters) 
        : DisciplineBaseException(
            parameters.Length == 0 
                ? $"{source}.{model}.NotFound"
                : $"{source}.{model}.{string.Join('.', parameters)}.NotFound",
            parameters);