namespace BetterResult;

/// <summary>
/// Represents an error.
/// </summary>
public readonly partial record struct Error
{
    private Error(ErrorType type, string code, string message, Dictionary<string, object>? metadata)
    {
        Type = type;
        Code = code;
        Message = message;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets the unique error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Dictionary<string, object>? Metadata { get; }

    /// <summary>
    /// Retrieves the metadata value associated with the given key, or returns <c>default(T)</c> if the key is not present.
    /// </summary>
    /// <typeparam name="T">The expected type of the metadata value.</typeparam>
    /// <param name="key">The key of the metadata entry to retrieve.</param>
    /// <returns>
    /// The metadata value cast to <typeparamref name="T"/>, or <c>default(T)</c> if the metadata dictionary is <c>null</c> or the key does not exist.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// Thrown if the metadata value exists but cannot be cast to <typeparamref name="T"/>.
    /// </exception>
    public T? GetMetadata<T>(string key)
    {
        if (Metadata is null || !Metadata.TryGetValue(key, out var value))
        {
            return default;
        }

        return value is T typedValue
            ? typedValue
            : throw new InvalidCastException($"The metadata for key '{key}' is not of type {typeof(T).Name}.");
    }

    /// <summary>
    /// Retrieves the first metadata value that can be cast to the specified type, or returns <c>default(T)</c> if none is found.
    /// </summary>
    /// <typeparam name="T">The type of the metadata value to search for.</typeparam>
    /// <returns>
    /// The first metadata value cast to <typeparamref name="T"/>, 
    /// or <c>default(T)</c> if the metadata dictionary is <c>null</c> or contains no entries of the requested type.
    /// </returns>
    public T? GetMetadata<T>()
    {
        if (Metadata is null)
        {
            return default;
        }

        foreach (var entry in Metadata.Values)
        {
            if (entry is T typedValue)
            {
                return typedValue;
            }
        }

        return default;
    }
}