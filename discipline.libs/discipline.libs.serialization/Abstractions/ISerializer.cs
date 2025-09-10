namespace discipline.libs.serializers.Abstractions;

public interface ISerializer
{
    T? ToObject<T>(string json) where T : class;
    T? ToObject<T>(byte[] bytes) where T : class;
    string ToJson(object obj);
    byte[] ToByteJson<T>(T value) where T : class;
}
