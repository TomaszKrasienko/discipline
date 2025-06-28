using System.Reflection;
using discipline.centre.shared.abstractions.Exceptions.Services;
using Microsoft.Extensions.Localization;

namespace discipline.centre.shared.infrastructure.Exceptions.Services;

internal sealed class ErrorLocalizationService : IErrorLocalizationService
{
    private readonly List<IStringLocalizer> _localizers = [];

    public ErrorLocalizationService(IList<Assembly> assemblies,
        IStringLocalizerFactory stringLocalizerFactory)
    {
        var appAssemblies = assemblies.Where(x 
            => (x.GetName().Name?.StartsWith("discipline.centre", StringComparison.OrdinalIgnoreCase) ?? false)
            && (x.GetName().Name?.EndsWith("api", StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
        
        foreach (var assembly in appAssemblies)
        {
            _localizers.Add(stringLocalizerFactory.Create("ErrorMessages", assembly.GetName().Name!));
        }
    }
    
    public string GetMessage(string code)
    {
        var errorMessage = _localizers
            .Select(x => x.GetString(code))
            .SingleOrDefault(x => !x.Value.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (errorMessage is null)
        {
            return "Unknown error";
        }
        
        return errorMessage.Value;
    }
}