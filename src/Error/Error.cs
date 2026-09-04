using System.Collections.ObjectModel;

namespace BetterResult;

/// <summary>
/// A failure: a type, a code, a message, and optional metadata.
/// </summary>
public readonly partial record struct Error
{
    private Error(ErrorType type, string code, string message, IReadOnlyDictionary<string, object>? metadata)
    {
        if (code is null) throw new ArgumentNullException(nameof(code));
        if (message is null) throw new ArgumentNullException(nameof(message));

        Type = type;
        Code = code;
        Message = message;

        if (metadata is null || metadata.Count == 0)
        {
            Metadata = null;
        }
        else
        {
            var copy = new Dictionary<string, object>(metadata.Count);
            foreach (var kvp in metadata)
                copy[kvp.Key] = kvp.Value;
            Metadata = new ReadOnlyDictionary<string, object>(copy);
        }
    }

    /// <summary>
    /// The error code, for example an HTTP status or a localization key.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// The error description.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// The error category.
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Optional context. Read-only: use the <c>WithMetadata</c> extensions to get a copy with more entries.
    /// </summary>
    /// <remarks>
    /// Metadata compares by reference in <see cref="Equals(Error)"/> and <see cref="GetHashCode"/>, not by
    /// content. Two errors built separately with equivalent metadata are not equal, so compare
    /// <see cref="Type"/>, <see cref="Code"/>, and <see cref="Message"/> instead.
    /// </remarks>
    public IReadOnlyDictionary<string, object>? Metadata { get; }

    /// <summary>
    /// Gets the metadata value for <paramref name="key"/>, or <c>default(T)</c> if there is none.
    /// </summary>
    /// <typeparam name="T">The expected type of the value.</typeparam>
    /// <param name="key">The metadata key.</param>
    /// <exception cref="InvalidCastException">The value exists but is not a <typeparamref name="T"/>.</exception>
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
    /// Gets the first metadata value of type <typeparamref name="T"/>, or <c>default(T)</c> if there is none.
    /// </summary>
    /// <typeparam name="T">The type to look for.</typeparam>
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
