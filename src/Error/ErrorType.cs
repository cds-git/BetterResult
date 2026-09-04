namespace BetterResult;

/// <summary>
/// The categories an <see cref="Error"/> can fall into.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// An expected failure, such as a broken business rule.
    /// </summary>
    Failure,

    /// <summary>
    /// An unexpected internal error, such as a caught exception.
    /// </summary>
    Unexpected,

    /// <summary>
    /// One or more inputs failed validation.
    /// </summary>
    Validation,

    /// <summary>
    /// The requested resource does not exist.
    /// </summary>
    NotFound,

    /// <summary>
    /// The operation clashes with the current state of the resource, such as a version mismatch.
    /// </summary>
    Conflict,

    /// <summary>
    /// The caller is not authenticated. Credentials are missing or invalid, so re-authenticating may
    /// help. Use <see cref="Forbidden"/> when the caller is authenticated but lacks permission.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// A dependency is down. Usually transient.
    /// </summary>
    Unavailable,

    /// <summary>
    /// The operation ran out of time.
    /// </summary>
    Timeout,

    /// <summary>
    /// The caller is authenticated but lacks permission, so re-authenticating will not help.
    /// Use <see cref="Unauthorized"/> when the caller is not authenticated at all.
    /// </summary>
    /// <remarks>
    /// Declared last to keep the numeric values of the other members stable for callers that store or
    /// send <see cref="ErrorType"/> as an integer.
    /// </remarks>
    Forbidden
}
