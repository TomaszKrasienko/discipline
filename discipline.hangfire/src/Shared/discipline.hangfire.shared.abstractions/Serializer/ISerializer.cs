namespace discipline.hangfire.shared.abstractions.Serializer;

/// <summary>
/// Provides methods for serializing and deserializing objects to and from JSON format.
/// </summary>
public interface ISerializer
{
    /// <summary>
    /// Deserializes a JSON-formatted string into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize into. Must be a reference type.</typeparam>
    /// <param name="json">The JSON-formatted string to deserialize.</param>
    /// <returns>An instance of type <typeparamref name="T"/> populated with data from the JSON string, or <c>null</c> if deserialization fails.</returns>
    T? ToObject<T>(string json) where T : class;
    
    /// <summary>
    /// Serializes an object into JSON-formatted string.
    /// </summary>
    /// <param name="obj">Object to serialize</param>
    /// <returns>JSON-formatted string</returns>
    string ToJson(object obj);
}
