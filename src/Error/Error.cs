namespace BetterResult;

/// <summary>
/// Represents an error.
/// </summary>
public readonly partial record struct Error
{
    private Error(ErrorType type, string code, string message, IReadOnlyDictionary<string, object>? metadata)
    {
        Type = type;
        Code = code;
        Message = message;

        if (metadata is null)
        {
            Metadata = null;
        }
        else
        {
            var copy = new Dictionary<string, object>(metadata.Count);
            foreach (var kvp in metadata)
                copy[kvp.Key] = kvp.Value;
            Metadata = copy;
        }
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
    public IReadOnlyDictionary<string, object>? Metadata { get; }

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

    /// <summary>
    /// Determines whether this <see cref="Error"/> is structurally equal to another, including key/value-level
    /// comparison of <see cref="Metadata"/>. Two errors with separately-allocated metadata dictionaries that
    /// hold the same entries compare equal.
    /// </summary>
    public bool Equals(Error other)
    {
        if (Type != other.Type) return false;
        if (!string.Equals(Code, other.Code, StringComparison.Ordinal)) return false;
        if (!string.Equals(Message, other.Message, StringComparison.Ordinal)) return false;
        return MetadataEquals(Metadata, other.Metadata);
    }

    /// <summary>
    /// Returns a hash code consistent with <see cref="Equals(Error)"/>. Order of metadata entries does not affect the result.
    /// </summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = (hash * 31) + (int)Type;
            hash = (hash * 31) + (Code is null ? 0 : Code.GetHashCode());
            hash = (hash * 31) + (Message is null ? 0 : Message.GetHashCode());

            if (Metadata is not null)
            {
                int metaHash = 0;
                foreach (var kvp in Metadata)
                {
                    int keyHash = kvp.Key.GetHashCode();
                    int valueHash = kvp.Value?.GetHashCode() ?? 0;
                    metaHash ^= (keyHash * 397) ^ valueHash;
                }
                hash = (hash * 31) + metaHash;
            }

            return hash;
        }
    }

    private static bool MetadataEquals(IReadOnlyDictionary<string, object>? a, IReadOnlyDictionary<string, object>? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        if (a.Count != b.Count) return false;

        foreach (var kvp in a)
        {
            if (!b.TryGetValue(kvp.Key, out var bValue)) return false;
            if (!Equals(kvp.Value, bValue)) return false;
        }

        return true;
    }
}