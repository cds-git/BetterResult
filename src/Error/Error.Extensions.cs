namespace BetterResult;

/// <summary>
/// Extension methods for <see cref="Error"/>.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Creates a new <see cref="Error"/> with the specified message prepended to the current error's description.
    /// </summary>
    /// <param name="error">The current error.</param>
    /// <param name="message">The message to prepend to the current error's description.</param>
    /// <returns>A new <see cref="Error"/> with the updated message.</returns>
    public static Error WithMessage(this Error error, string message)
    {
        string newDescription = $"{message}: {error.Description}";
        return Error.Custom(error.Type, error.Code, newDescription, error.Metadata);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> by merging the provided metadata dictionary with the current error's metadata.
    /// Existing keys in the current metadata will be overridden by the new values.
    /// </summary>
    /// <param name="error">The current error.</param>
    /// <param name="metadata">The new metadata to merge with the existing metadata.</param>
    /// <returns>A new <see cref="Error"/> with the updated metadata.</returns>
    public static Error WithMetadata(this Error error, Dictionary<string, object> metadata)
    {
        var mergedMetadata = error.Metadata ?? [];

        foreach (var kvp in metadata)
        {
            mergedMetadata[kvp.Key] = kvp.Value;  // Override existing keys
        }

        return Error.Custom(error.Type, error.Code, error.Description, mergedMetadata);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> by adding or updating a specific metadata key-value pair.
    /// </summary>
    /// <param name="error">The current error.</param>
    /// <param name="key">The metadata key to add or update.</param>
    /// <param name="metadata">The metadata value to add or update.</param>
    /// <typeparam name="T">The type of the metadata value.</typeparam>
    /// <returns>A new <see cref="Error"/> with the updated metadata.</returns>
    public static Error WithMetadata<T>(this Error error, string key, T metadata)
    {
        if (metadata is null) throw new ArgumentNullException(nameof(metadata));

        var mergedMetadata = error.Metadata ?? [];

        mergedMetadata[key] = metadata; // Override existing keys

        return Error.Custom(error.Type, error.Code, error.Description, mergedMetadata);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> by adding or updating metadata using the type name of the metadata as the key.
    /// </summary>
    /// <param name="error">The current error.</param>
    /// <param name="metadata">The metadata value to add or update.</param>
    /// <typeparam name="T">The type of the metadata value.</typeparam>
    /// <returns>A new <see cref="Error"/> with the updated metadata.</returns>
    public static Error WithMetadata<T>(this Error error, T metadata)
    {
        string key = typeof(T).Name;
        return error.WithMetadata(key, metadata);
    }
}