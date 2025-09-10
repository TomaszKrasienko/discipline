namespace discipline.libs.exceptions.Exceptions;

public abstract class DisciplineBaseException(
    string code,
    params string[] parameters) : Exception
{
    public string Code { get; } = code;
    public string[] Parameters { get; set; } = parameters;
}