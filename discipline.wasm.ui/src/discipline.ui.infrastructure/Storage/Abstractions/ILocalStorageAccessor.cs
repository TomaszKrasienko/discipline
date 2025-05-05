namespace discipline.ui.infrastructure.Storage.Abstractions;

public interface ILocalStorageAccessor
{
    Task SetItemAsync<T>(T t) where T : class;
    Task<T> GetItemAsync<T>() where T : class;
    Task RemoveAsync<T>() where T : class;
}