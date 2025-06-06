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
            => (x.GetName().Name?.StartsWith("discipline.centre.activityrules.api", StringComparison.OrdinalIgnoreCase) ?? false)
            && (x.GetName().Name?.EndsWith("api", StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
        
        foreach (var assembly in appAssemblies)
        {
            _localizers.Add(stringLocalizerFactory.Create("ErrorMessages", assembly.GetName().Name!));
        }
    }
    
    public string GetMessage(string code)
    {
        //var errorMessage = _localizers.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x[code]));
        var errorMessage = _localizers.First();

        if (errorMessage is null)
        {
            return "Unexpected error";
        }
        
        return errorMessage[code];
    }
}