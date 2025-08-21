namespace discipline.centre.shared.abstractions.SharedKernel;

public abstract class DisciplineException(string code, params object[]? args)
    : Exception()
{
    public string Code => code;
    public object[]? Args => args;
}