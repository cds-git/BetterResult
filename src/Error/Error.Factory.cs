namespace BetterResult;

public readonly partial record struct Error
{
    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Failure"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Failure(
       string code = "General.Failure",
       string description = "Failure",
       Dictionary<string, object>? metadata = null) =>
           new(ErrorType.Failure, code, description, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Unexpected"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Unexpected(
        string code = "General.Unexpected",
        string description = "An unexpected error has occurred.",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Unexpected, code, description, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Validation"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Validation(
        string code = "General.Validation",
        string description = "Validation error has occurred.",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Validation, code, description, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.NotFound"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error NotFound(
        string code = "General.NotFound",
        string description = "Not Found",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.NotFound, code, description, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Conflict"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Conflict(
        string code = "General.Conflict",
        string description = "Conflict",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Conflict, code, description, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Unauthorized"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string description = "Unauthorized",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Unauthorized, code, description, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Forbidden"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Forbidden(
        string code = "General.Forbidden",
        string description = "Forbidden",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Forbidden, code, description, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Unavailable"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Unavailable(
        string code = "General.Unavailable",
        string description = "The requested service was unavailable",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Unavailable, code, description, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> with the given <paramref name="type"/>,
    /// <paramref name="code"/>, and <paramref name="description"/>.
    /// </summary>
    /// <param name="type"><see cref="ErrorType"/> value which represents the type of error that occurred.</param>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Custom(
        ErrorType type,
        string code = "General.Custom",
        string description = "Custom error occurred",
        Dictionary<string, object>? metadata = null) =>
            new(type, code, description, metadata);
}