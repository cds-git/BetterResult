namespace BetterResult;

/// <summary>
/// Represents the broad categories of errors that an operation can produce.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// A general, expected failure (for example, a business‐rule violation).
    /// </summary>
    Failure,

    /// <summary>
    /// An unexpected internal error (for example, a null reference or system exception).
    /// </summary>
    Unexpected,

    /// <summary>
    /// One or more inputs did not pass validation rules.
    /// </summary>
    Validation,

    /// <summary>
    /// The requested resource could not be found.
    /// </summary>
    NotFound,

    /// <summary>
    /// The operation conflicts with the current state of the resource (for example, a version mismatch).
    /// </summary>
    Conflict,

    /// <summary>
    /// The caller is not authenticated, or the supplied credentials are missing or invalid.
    /// Re-authenticating may allow the operation to succeed. Use <see cref="Forbidden"/> when the
    /// caller is authenticated but lacks permission.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// A dependent service or subsystem is currently unavailable (typically a transient condition).
    /// </summary>
    Unavailable,

    /// <summary>
    /// The operation did not complete within the allotted time.
    /// </summary>
    Timeout,

    /// <summary>
    /// The caller is authenticated but does not have permission to perform the operation.
    /// Re-authenticating will not help. Use <see cref="Unauthorized"/> when the caller is not
    /// authenticated at all.
    /// </summary>
    /// <remarks>
    /// Declared last so that the numeric values of the preceding members remain stable for
    /// callers that persist or transmit <see cref="ErrorType"/> as an integer.
    /// </remarks>
    Forbidden
}