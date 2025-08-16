using System.Text.Json;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace discipline.centre.integrationTests.sharedKernel;

public sealed class TestRedisCache : IDisposable
{
    private RedisContainer? _redisContainer;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    public string ConnectionString { get; }
    
    public TestRedisCache()
    {
        CreateContainer();
        _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisContainer!.GetConnectionString());
        ConnectionString = _redisContainer.GetConnectionString();
    }

    private void CreateContainer()
    {
        _redisContainer = new RedisBuilder()
            .Build();
        _redisContainer.StartAsync().GetAwaiter().GetResult();
    }

    public T? GetValueAsync<T>(string key) where T : class
    {
        var db = _connectionMultiplexer.GetDatabase();
        var value = db.HashGet(key, "data");
        
        if (!value.HasValue)
        {
            return null;
        }
        
        var stringValue = value.ToString();
        
        return JsonSerializer.Deserialize<T>(stringValue);
    }
    
    public void Dispose()
    {
        _redisContainer?.StopAsync().GetAwaiter().GetResult();
    }
}