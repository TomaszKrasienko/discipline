using System.Text;
using discipline.hangfire.shared.abstractions.Serializer;
using Newtonsoft.Json;

namespace discipline.hangfire.infrastructure.Serializer;

internal sealed class NewtonsoftJsonSerializer : ISerializer
{
    public T? ToObject<T>(string json) where T : class
        => JsonConvert.DeserializeObject<T>(json);

    public T? ToObject<T>(byte[] bytes) where T : class
    {
        var json = Encoding.UTF8.GetString(bytes);
        return ToObject<T>(json);
    }

    public string ToJson(object obj)
        => JsonConvert.SerializeObject(obj);

    public byte[] ToByteJson<T>(T value) where T : class
        => Encoding.UTF8.GetBytes(ToJson(value));
}