namespace BetterResult;

/// <summary>
/// Provides static factory methods for creating Result instances.
/// </summary>
public static partial class Result
{
    /// <summary>
    /// Creates a successful result with no meaningful value.
    /// Equivalent to Result&lt;NoValue&gt;.Success(NoValue.Instance).
    /// </summary>
    /// <returns>A successful Result containing NoValue.</returns>
    public static Result<NoValue> Success() => NoValue.Instance;

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to wrap in a successful result.</param>
    /// <returns>A successful Result containing the specified value.</returns>
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <typeparam name="T">The type of the value that would have been returned on success.</typeparam>
    /// <param name="error">The error describing the failure.</param>
    /// <returns>A failed Result containing the specified error.</returns>
    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
}
