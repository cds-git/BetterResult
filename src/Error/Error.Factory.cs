namespace BetterResult;

public readonly partial record struct Error
{
    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Failure"/> from a code and message.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="message">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Failure(
       string code = "General.Failure",
       string message = "Failure",
       Dictionary<string, object>? metadata = null) =>
           new(ErrorType.Failure, code, message, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Unexpected"/> from a code and message.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="message">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Unexpected(
        string code = "General.Unexpected",
        string message = "An unexpected error has occurred.",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Unexpected, code, message, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Validation"/> from a code and message.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="message">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Validation(
        string code = "General.Validation",
        string message = "Validation error has occurred.",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Validation, code, message, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.NotFound"/> from a code and message.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="message">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error NotFound(
        string code = "General.NotFound",
        string message = "Not Found",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.NotFound, code, message, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Conflict"/> from a code and message.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="message">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Conflict(
        string code = "General.Conflict",
        string message = "Conflict",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Conflict, code, message, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Unauthorized"/> from a code and message.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="message">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string message = "Unauthorized",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Unauthorized, code, message, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Unavailable"/> from a code and message.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="message">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Unavailable(
        string code = "General.Unavailable",
        string message = "The requested service was unavailable",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Unavailable, code, message, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Timeout"/> from a code and message.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="message">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Timeout(
        string code = "General.Timeout",
        string message = "Timeout",
        Dictionary<string, object>? metadata = null) =>
            new(ErrorType.Timeout, code, message, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> with the given <paramref name="type"/>,
    /// <paramref name="code"/>, and <paramref name="message"/>.
    /// </summary>
    /// <param name="type"><see cref="ErrorType"/> value which represents the type of error that occurred.</param>
    /// <param name="code">The unique error code.</param>
    /// <param name="message">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Create(
        ErrorType type,
        string code = "General.Custom",
        string message = "Custom error occurred",
        Dictionary<string, object>? metadata = null) =>
            new(type, code, message, metadata);
}