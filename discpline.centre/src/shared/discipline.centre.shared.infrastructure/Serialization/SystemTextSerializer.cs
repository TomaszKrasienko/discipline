using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using discipline.centre.shared.abstractions.Serialization;

namespace discipline.centre.shared.infrastructure.Serialization;

internal sealed class SystemTextSerializer : ISerializer
{
    private readonly JsonSerializerOptions _options = new ()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = false,
        Converters = { new JsonStringEnumConverter() }
    };

    public string ToJson<T>(T value) where T : class
        => JsonSerializer.Serialize(value, _options);

    public byte[] ToByteJson<T>(T value) where T : class
        => Encoding.UTF8.GetBytes(ToJson(value));

    public object? ToObject(string json, Type type)
        => JsonSerializer.Deserialize(json, type, _options);

    public T? ToObject<T>(string json) where T : class
        => JsonSerializer.Deserialize<T>(json, _options);

    public T? ToObject<T>(byte[] byteJson) where T : class
        => ToObject<T>(Encoding.UTF8.GetString(byteJson));
}