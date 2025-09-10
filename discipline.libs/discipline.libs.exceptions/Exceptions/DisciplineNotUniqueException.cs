namespace discipline.libs.exceptions.Exceptions;

public sealed class DisciplineNotUniqueException(
    string code, 
    params string[] parameters) : DisciplineBaseException(code, parameters);