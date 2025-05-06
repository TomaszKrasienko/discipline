using discipline.hangfire.shared.abstractions.Serializer;
using Newtonsoft.Json;

namespace discipline.hangfire.infrastructure.Serializer;

internal sealed class NewtonsoftJsonSerializer : ISerializer
{
    public T? ToObject<T>(string json) where T : class
        => JsonConvert.DeserializeObject<T>(json);

    public string ToJson(object obj)
        => JsonConvert.SerializeObject(obj);
}