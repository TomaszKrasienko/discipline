namespace discipline.centre.shared.abstractions.Exceptions.Services;

public interface IErrorLocalizationService
{
    string GetMessage(string code);
}