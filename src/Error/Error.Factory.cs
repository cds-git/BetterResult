namespace BetterResult;

public readonly partial record struct Error
{
    /// <summary>
    /// An expected failure, such as a broken business rule.
    /// </summary>
    public static Error Failure(
       string code = "General.Failure",
       string message = "Failure",
       IReadOnlyDictionary<string, object>? metadata = null) =>
           new(ErrorType.Failure, code, message, metadata);

    /// <summary>
    /// An unexpected internal error, such as a caught exception.
    /// </summary>
    public static Error Unexpected(
        string code = "General.Unexpected",
        string message = "An unexpected error has occurred.",
        IReadOnlyDictionary<string, object>? metadata = null) =>
            new(ErrorType.Unexpected, code, message, metadata);

    /// <summary>
    /// One or more inputs failed validation.
    /// </summary>
    public static Error Validation(
        string code = "General.Validation",
        string message = "Validation error has occurred.",
        IReadOnlyDictionary<string, object>? metadata = null) =>
            new(ErrorType.Validation, code, message, metadata);

    /// <summary>
    /// The requested resource does not exist.
    /// </summary>
    public static Error NotFound(
        string code = "General.NotFound",
        string message = "Not Found",
        IReadOnlyDictionary<string, object>? metadata = null) =>
            new(ErrorType.NotFound, code, message, metadata);

    /// <summary>
    /// The operation clashes with the current state of the resource.
    /// </summary>
    public static Error Conflict(
        string code = "General.Conflict",
        string message = "Conflict",
        IReadOnlyDictionary<string, object>? metadata = null) =>
            new(ErrorType.Conflict, code, message, metadata);

    /// <summary>
    /// The caller is not authenticated. Use <see cref="Forbidden"/> when the caller is authenticated
    /// but lacks permission.
    /// </summary>
    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string message = "Unauthorized",
        IReadOnlyDictionary<string, object>? metadata = null) =>
            new(ErrorType.Unauthorized, code, message, metadata);

    /// <summary>
    /// The caller is authenticated but lacks permission. Use <see cref="Unauthorized"/> when the caller
    /// is not authenticated at all.
    /// </summary>
    public static Error Forbidden(
        string code = "General.Forbidden",
        string message = "Forbidden",
        IReadOnlyDictionary<string, object>? metadata = null) =>
            new(ErrorType.Forbidden, code, message, metadata);

    /// <summary>
    /// A dependency is down. Usually transient.
    /// </summary>
    public static Error Unavailable(
        string code = "General.Unavailable",
        string message = "The requested service was unavailable",
        IReadOnlyDictionary<string, object>? metadata = null) =>
            new(ErrorType.Unavailable, code, message, metadata);

    /// <summary>
    /// The operation ran out of time.
    /// </summary>
    public static Error Timeout(
        string code = "General.Timeout",
        string message = "Timeout",
        IReadOnlyDictionary<string, object>? metadata = null) =>
            new(ErrorType.Timeout, code, message, metadata);

    /// <summary>
    /// An error with an explicit <see cref="ErrorType"/>, for when none of the named factories fit.
    /// </summary>
    public static Error Create(
        ErrorType type,
        string code = "General.Custom",
        string message = "Custom error occurred",
        IReadOnlyDictionary<string, object>? metadata = null) =>
            new(type, code, message, metadata);
}
