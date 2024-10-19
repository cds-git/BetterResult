namespace BetterResult;

/// <summary>
/// Error types.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Indicates that error was of type <see cref="ErrorType.Failure"/>
    /// </summary>
    Failure,

    /// <summary>
    /// Indicates that error was of type <see cref="ErrorType.Unexpected"/>
    /// </summary>
    Unexpected,

    /// <summary>
    /// Indicates that error was of type <see cref="ErrorType.Validation"/>
    /// </summary>
    Validation,

    /// <summary>
    /// Indicates that error was of type <see cref="ErrorType.NotFound"/>
    /// </summary>
    NotFound,

    /// <summary>
    /// Indicates that error was of type <see cref="ErrorType.Conflict"/>
    /// </summary>
    Conflict,

    /// <summary>
    /// Indicates that error was of type <see cref="ErrorType.Unauthorized"/>
    /// </summary>
    Unauthorized,

    /// <summary>
    /// Indicates that error was of type <see cref="ErrorType.Forbidden"/>
    /// </summary>
    Forbidden,

    /// <summary>
    /// Indicates that error was of type <see cref="ErrorType.Unavailable"/>
    /// </summary>
    Unavailable
}