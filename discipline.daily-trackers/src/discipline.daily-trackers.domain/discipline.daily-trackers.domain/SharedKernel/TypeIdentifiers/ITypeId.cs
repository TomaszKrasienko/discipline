namespace discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

public interface IBaseTypeId<out TValue>
    where TValue : struct
{
    TValue Value { get; }
}

public interface ITypeId<out TType, out TValue> : IBaseTypeId<TValue> 
    where TType : struct, IBaseTypeId<TValue> 
    where TValue : struct
{
    static abstract TType New();
    static abstract TType Parse(string stringTypedId);
}