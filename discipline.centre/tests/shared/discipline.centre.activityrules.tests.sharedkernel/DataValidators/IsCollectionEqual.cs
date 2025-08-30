namespace discipline.centre.activityrules.tests.sharedkernel.DataValidators;

public static class CollectionValidator
{
    public static bool IsEqual(this IReadOnlySet<int>? source, IReadOnlySet<int>? destination)
    {
        if (source?.Count != destination?.Count)
        {
            return false;
        }
        
        var sourceChanged = source!.Select(x => x)
            .OrderBy(x => x)
            .ToList();
        
        var destinationChanged = destination!.Select(x => x)
            .OrderBy(x => x)
            .ToList();
        
        return sourceChanged.SequenceEqual(destinationChanged);
    }
    
    public static bool IsEqual(this List<int>? source, List<int>? destination)
    {
        if (source?.Count != destination?.Count)
        {
            return false;
        }
        
        var sourceChanged = source!.Select(x => x)
            .OrderBy(x => x)
            .ToList();
        
        var destinationChanged = destination!.Select(x => x)
            .OrderBy(x => x)
            .ToList();
        
        return sourceChanged.SequenceEqual(destinationChanged);
    }
}