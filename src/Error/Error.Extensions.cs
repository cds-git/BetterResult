namespace BetterResult;

/// <summary>
/// Extension methods for <see cref="Error"/>. Each returns a new error; the original is untouched.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Prepends <paramref name="message"/> to the existing message, keeping type, code, and metadata.
    /// </summary>
    public static Error WithMessage(this Error error, string message)
    {
        string newDescription = $"{message}: {error.Message}";
        return Error.Create(error.Type, error.Code, newDescription, error.Metadata);
    }

    /// <summary>
    /// Replaces the code, keeping type, message, and metadata. Handy for re-mapping an external error
    /// into your own code space.
    /// </summary>
    public static Error WithCode(this Error error, string code) =>
        Error.Create(error.Type, code, error.Message, error.Metadata);

    /// <summary>
    /// Replaces the type, keeping code, message, and metadata. Handy for re-categorising an error
    /// during translation.
    /// </summary>
    public static Error WithType(this Error error, ErrorType type) =>
        Error.Create(type, error.Code, error.Message, error.Metadata);

    /// <summary>
    /// Merges <paramref name="metadata"/> into the existing metadata. Duplicate keys are overwritten.
    /// </summary>
    public static Error WithMetadata(this Error error, Dictionary<string, object>? metadata) =>
        MergeMetadata(error, metadata);

    /// <summary>
    /// Merges <paramref name="metadata"/> into the existing metadata. Duplicate keys are overwritten.
    /// </summary>
    public static Error WithMetadata(this Error error, IReadOnlyDictionary<string, object>? metadata) =>
        MergeMetadata(error, metadata);

    private static Error MergeMetadata(Error error, IReadOnlyDictionary<string, object>? metadata)
    {
        var mergedMetadata = CopyOrEmpty(error.Metadata);

        if (metadata is not null)
        {
            foreach (var kvp in metadata)
                mergedMetadata[kvp.Key] = kvp.Value;
        }

        return Error.Create(error.Type, error.Code, error.Message, mergedMetadata);
    }

    /// <summary>
    /// Adds or overwrites a single metadata entry. A null value is ignored.
    /// </summary>
    public static Error WithMetadata<T>(this Error error, string key, T? metadata)
    {
        var mergedMetadata = CopyOrEmpty(error.Metadata);

        if (metadata is not null)
            mergedMetadata[key] = metadata;

        return Error.Create(error.Type, error.Code, error.Message, mergedMetadata);
    }

    private static Dictionary<string, object> CopyOrEmpty(IReadOnlyDictionary<string, object>? source)
    {
        if (source is null) return [];
        var copy = new Dictionary<string, object>(source.Count);
        foreach (var kvp in source)
            copy[kvp.Key] = kvp.Value;
        return copy;
    }

    /// <summary>
    /// Adds or overwrites a metadata entry keyed by the full name of <typeparamref name="T"/>.
    /// The namespace is included so types sharing a simple name do not collide. A null value is ignored.
    /// </summary>
    public static Error WithMetadata<T>(this Error error, T? metadata)
    {
        if (metadata is null)
            return error;

        var key = typeof(T).FullName ?? typeof(T).Name;
        return error.WithMetadata(key, metadata);
    }
}
