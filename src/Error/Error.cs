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
}