namespace BetterResult;

/// <summary>
/// Represents an error.
/// </summary>
public readonly partial record struct Error
{
    private Error(ErrorType type, string code, string description, Dictionary<string, object>? metadata)
    {
        Type = type;
        Code = code;
        Description = description;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets the unique error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Dictionary<string, object>? Metadata { get; }

    /// <summary>
    /// Retrieves the metadata associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the metadata value.</typeparam>
    /// <param name="key">The key associated with the metadata.</param>
    /// <returns>The metadata value associated with the key, or throws an exception if the key is not found or type mismatched.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the key does not exist in the metadata.</exception>
    /// <exception cref="InvalidCastException">Thrown if the metadata cannot be cast to the specified type.</exception>
    public T GetMetadataByKey<T>(string key)
    {
        if (Metadata is null || !Metadata.TryGetValue(key, out var value))
        {
            throw new KeyNotFoundException($"The key '{key}' was not found in the metadata.");
        }

        if (value is T typedValue)
        {
            return typedValue;
        }

        throw new InvalidCastException($"The metadata for key '{key}' is not of type {typeof(T).Name}.");
    }

    /// <summary>
    /// Retrieves the first metadata value that matches the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the metadata value.</typeparam>
    /// <returns>The first metadata value of the specified type, or throws an exception if no such value is found.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if no metadata of the specified type is found.</exception>
    public T GetMetadataByType<T>()
    {
        if (Metadata is null)
        {
            throw new KeyNotFoundException($"No metadata found of type {typeof(T).Name}.");
        }

        foreach (var entry in Metadata.Values)
        {
            if (entry is T typedValue)
            {
                return typedValue;
            }
        }

        throw new KeyNotFoundException($"No metadata found of type {typeof(T).Name}.");
    }
}