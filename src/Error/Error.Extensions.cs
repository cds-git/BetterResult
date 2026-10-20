namespace BetterResult;

/// <summary>
/// Extension methods for <see cref="Error"/>. Each returns a new error of the same runtime type, so a
/// custom error stays itself. The original is untouched.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Prepends <paramref name="message"/> to the existing message, keeping type, code, and metadata.
    /// </summary>
    public static Error WithMessage(this Error error, string message)
    {
        return error with { Message = $"{message}: {error.Message}" };
    }

    /// <summary>
    /// Replaces the code, keeping type, message, and metadata. Handy for re-mapping an external error
    /// into your own code space.
    /// </summary>
    public static Error WithCode(this Error error, string code) => error with { Code = code };

    /// <summary>
    /// Replaces the type, keeping code, message, and metadata. Handy for re-categorising an error
    /// during translation.
    /// </summary>
    public static Error WithType(this Error error, ErrorType type) => error with { Type = type };

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

    /// <summary>
    /// Reports both errors together as an <see cref="ErrorList"/>. Lists are flattened, so combining
    /// into an existing list adds to it.
    /// </summary>
    public static Error Combine(this Error error, Error other) => new[] { error, other }.Combine();

    /// <summary>
    /// Reports the errors together as an <see cref="ErrorList"/>. A single error comes back unchanged
    /// and lists are flattened.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="errors"/> is empty or contains null.</exception>
    public static Error Combine(this IEnumerable<Error> errors)
    {
        var inputs = errors.ToList();
        if (inputs.Count is 0)
            throw new ArgumentException("Cannot combine an empty set of errors.", nameof(errors));
        if (inputs.Any(e => e is null))
            throw new ArgumentException("Cannot combine a null error.", nameof(errors));
        if (inputs.Count is 1)
            return inputs[0];

        return new ErrorList(inputs.SelectMany(e => e is ErrorList list ? list.Errors : [e]).ToList().AsReadOnly());
    }

    private static Error MergeMetadata(Error error, IReadOnlyDictionary<string, object>? metadata)
    {
        var mergedMetadata = CopyOrEmpty(error.Metadata);

        if (metadata is not null)
        {
            foreach (var kvp in metadata)
                mergedMetadata[kvp.Key] = kvp.Value;
        }

        return error with { Metadata = mergedMetadata };
    }

    /// <summary>
    /// Adds or overwrites a single metadata entry. A null value is not stored.
    /// </summary>
    public static Error WithMetadata<T>(this Error error, string key, T? metadata)
    {
        var mergedMetadata = CopyOrEmpty(error.Metadata);

        if (metadata is not null)
            mergedMetadata[key] = metadata;

        return error with { Metadata = mergedMetadata };
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
    /// The namespace is included so types sharing a simple name do not collide. A null value returns
    /// the error unchanged.
    /// </summary>
    public static Error WithMetadata<T>(this Error error, T? metadata)
    {
        if (metadata is null)
            return error;

        var key = typeof(T).FullName ?? typeof(T).Name;
        return error.WithMetadata(key, metadata);
    }
}
